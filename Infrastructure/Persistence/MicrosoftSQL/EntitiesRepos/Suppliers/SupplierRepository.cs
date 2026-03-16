using Domain.Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL.Persistence.MicrosoftSQL
{
    public class SupplierRepository : ISupplierRepository
    {
        private SqlTransaction _currentTransaction;
        private readonly IApplicationSettings _appSettings;

        // Queries actualizadas: Sin filtros IsDeleted y Borrado Físico en la DAL
        private const string SQL_INSERT = @"INSERT INTO {0} 
            (Id, CompanyName, ContactName, TaxId, Phone, Mail, Address, City, Observations, DVH, IsActive, IsDeleted) 
            VALUES 
            (@Id, @CompanyName, @ContactName, @TaxId, @Phone, @Mail, @Address, @City, @Observations, @DVH, @IsActive, @IsDeleted)";

        private const string SQL_UPDATE = @"UPDATE {0} 
            SET CompanyName = @CompanyName, ContactName = @ContactName, TaxId = @TaxId, 
                Phone = @Phone, Mail = @Mail, Address = @Address, City = @City, 
                Observations = @Observations, DVH = @DVH, IsActive = @IsActive, IsDeleted = @IsDeleted
            WHERE Id = @Id";

        private const string SQL_HARD_DELETE = "DELETE FROM {0} WHERE Id = @Id";
        private const string SQL_SELECT_BY_ID = "SELECT * FROM {0} WHERE Id = @Id";
        private const string SQL_SELECT_ALL = "SELECT * FROM {0}";
        private const string SQL_SELECT_BY_TAXID = "SELECT * FROM {0} WHERE TaxId = @TaxId";

        public SupplierRepository(IApplicationSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void SetTransaction(object transaction) => _currentTransaction = (SqlTransaction)transaction;

        
        
        // --- MÉTODOS PÚBLICOS ---

        public async Task<Supplier> GetByTaxIdAsync(string taxId)
        {
            Supplier supplier = null;
            string query = string.Format(SQL_SELECT_BY_TAXID, _appSettings.SupplierTableName);

            await ExecuteReaderAsync(query,
                // Usamos NVarChar para coincidir con la definición de la tabla
                cmd => cmd.Parameters.Add(new SqlParameter("@TaxId", SqlDbType.NVarChar) { Value = taxId }),
                reader => supplier = Map(reader));

            return supplier;
        }

        public Task CreateAsync(Supplier entity) =>
            ExecuteNonQueryAsync(string.Format(SQL_INSERT, _appSettings.SupplierTableName), cmd => SetParameters(cmd, entity));

        public Task UpdateAsync(Supplier entity) =>
            ExecuteNonQueryAsync(string.Format(SQL_UPDATE, _appSettings.SupplierTableName), cmd => SetParameters(cmd, entity));

        public Task DeleteAsync(Guid entityId) =>
            ExecuteNonQueryAsync(string.Format(SQL_HARD_DELETE, _appSettings.SupplierTableName),
                cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entityId }));

        public async Task<Supplier> GetByIdAsync(Guid id)
        {
            Supplier supplier = null;
            await ExecuteReaderAsync(string.Format(SQL_SELECT_BY_ID, _appSettings.SupplierTableName),
                cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }),
                reader => supplier = Map(reader));
            return supplier;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            var suppliers = new List<Supplier>();
            await ExecuteReaderAsync(string.Format(SQL_SELECT_ALL, _appSettings.SupplierTableName),
                null, reader => suppliers.Add(Map(reader)));
            return suppliers;
        }

        // --- MAPEO Y PARÁMETROS ---

        private void SetParameters(SqlCommand cmd, Supplier entity)
        {
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entity.Id });
            cmd.Parameters.Add(new SqlParameter("@CompanyName", SqlDbType.NVarChar) { Value = (object)entity.CompanyName?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@ContactName", SqlDbType.NVarChar) { Value = (object)entity.ContactName?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@TaxId", SqlDbType.NVarChar) { Value = (object)entity.TaxId?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NVarChar) { Value = (object)entity.Phone?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Mail", SqlDbType.NVarChar) { Value = (object)entity.Mail?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar) { Value = (object)entity.Address?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@City", SqlDbType.NVarChar) { Value = (object)entity.City?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Observations", SqlDbType.NVarChar) { Value = (object)entity.Observations?.Value ?? DBNull.Value });

            // CORREGIDO: Acceso al .Value del DVH
            cmd.Parameters.Add(new SqlParameter("@DVH", SqlDbType.NVarChar) { Value = (object)entity.DVH?.Value ?? DBNull.Value });

            cmd.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = entity.Active });
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = entity.IsDeleted });
        }

        private Supplier Map(SqlDataReader reader)
        {
            // Reconstitución limpia respetando la firma de la Entidad
            return Supplier.Reconstitute(
                id: (Guid)reader["Id"],
                rawCompanyName: reader["CompanyName"]?.ToString(),
                rawContactName: reader["ContactName"]?.ToString(),
                rawTaxId: reader["TaxId"]?.ToString(),
                rawPhone: reader["Phone"]?.ToString(),
                rawMail: reader["Mail"]?.ToString(),
                rawAddress: reader["Address"]?.ToString(),
                rawCity: reader["City"]?.ToString(),
                observations: reader["Observations"]?.ToString(),
                dvh: reader["DVH"]?.ToString(),
                active: (bool)reader["IsActive"],
                isDeleted: (bool)reader["IsDeleted"]
            );
        }
        // --- MÉTODOS PRIVADOS DE INFRAESTRUCTURA ---

        private async Task ExecuteNonQueryAsync(string query, Action<SqlCommand> parameterSetter)
        {
            SqlConnection conn = _currentTransaction?.Connection;
            bool isExternalConn = conn != null;

            if (!isExternalConn) conn = new SqlConnection(_appSettings.EntitiesConnection);

            try
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    if (isExternalConn) cmd.Transaction = _currentTransaction;
                    parameterSetter?.Invoke(cmd);
                    if (!isExternalConn) await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            finally
            {
                if (!isExternalConn) conn.Dispose();
            }
        }

        private async Task ExecuteReaderAsync(string query, Action<SqlCommand> parameterSetter, Action<SqlDataReader> mapAction)
        {
            SqlConnection conn = _currentTransaction?.Connection;
            bool isExternalConn = conn != null;

            if (!isExternalConn) conn = new SqlConnection(_appSettings.EntitiesConnection);

            try
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    if (isExternalConn) cmd.Transaction = _currentTransaction;
                    parameterSetter?.Invoke(cmd);
                    if (!isExternalConn) await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync()) mapAction(reader);
                    }
                }
            }
            finally
            {
                if (!isExternalConn) conn.Dispose();
            }
        }

    
    }
}