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

        // Se usa string.Format para mantener la flexibilidad del nombre de la tabla (como hiciste en Employee)
        private const string SQL_INSERT = @"INSERT INTO {0} 
                                            (Id, CompanyName, ContactName, TaxId, Phone, Mail, Address, City, Observations, DVH, IsActive, IsDeleted) 
                                            VALUES 
                                            (@Id, @CompanyName, @ContactName, @TaxId, @Phone, @Mail, @Address, @City, @Observations, @DVH, @IsActive, @IsDeleted)";

        private const string SQL_UPDATE = @"UPDATE {0} 
                                            SET CompanyName = @CompanyName, 
                                                ContactName = @ContactName, 
                                                TaxId = @TaxId, 
                                                Phone = @Phone, 
                                                Mail = @Mail, 
                                                Address = @Address,
                                                City = @City,
                                                Observations = @Observations,
                                                DVH = @DVH,
                                                IsActive = @IsActive,
                                                IsDeleted = @IsDeleted
                                            WHERE Id = @Id";

        private const string SQL_SOFT_DELETE = "UPDATE {0} SET IsDeleted = 1, IsActive = 0 WHERE Id = @Id";
        private const string SQL_SELECT_BY_ID = "SELECT * FROM {0} WHERE Id = @Id AND IsDeleted = 0";
        private const string SQL_SELECT_ALL = "SELECT * FROM {0} WHERE IsDeleted = 0";
        private const string SQL_SELECT_BY_TAXID = "SELECT * FROM {0} WHERE TaxId = @TaxId AND IsDeleted = 0";


        public SupplierRepository(IApplicationSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void SetTransaction(object transaction)
        {
            _currentTransaction = (SqlTransaction)transaction;
        }

        public Task CreateAsync(Supplier entity)
        {
            string query = string.Format(SQL_INSERT, _appSettings.SupplierTableName);
            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public Task UpdateAsync(Supplier entity)
        {
            string query = string.Format(SQL_UPDATE, _appSettings.SupplierTableName);
            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public Task DeleteAsync(Guid entityId)
        {
            string query = string.Format(SQL_SOFT_DELETE, _appSettings.SupplierTableName);
            return ExecuteNonQueryAsync(query, cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entityId }));
        }

        public async Task<Supplier> GetByIdAsync(Guid id)
        {
            Supplier supplier = null;
            string query = string.Format(SQL_SELECT_BY_ID, _appSettings.SupplierTableName);

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }),
                reader => supplier = Map(reader));

            return supplier;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            var suppliers = new List<Supplier>();
            string query = string.Format(SQL_SELECT_ALL, _appSettings.SupplierTableName);

            await ExecuteReaderAsync(query, null, reader => suppliers.Add(Map(reader)));

            return suppliers;
        }

        public async Task<Supplier> GetByTaxIdAsync(string taxId)
        {
            Supplier supplier = null;
            string query = string.Format(SQL_SELECT_BY_TAXID, _appSettings.SupplierTableName);

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@TaxId", SqlDbType.NVarChar) { Value = taxId }),
                reader => supplier = Map(reader));

            return supplier;
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

        // --- MAPEO Y PARÁMETROS ---

        private void SetParameters(SqlCommand cmd, Supplier entity)
        {
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entity.Id });

            // Extraemos los valores primitivos (.Value) de los ValueObjects para guardarlos. 
            // Usamos NVARCHAR porque así está en la base de datos (foto)
            cmd.Parameters.Add(new SqlParameter("@CompanyName", SqlDbType.NVarChar) { Value = (object)entity.CompanyName?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@ContactName", SqlDbType.NVarChar) { Value = (object)entity.ContactName?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@TaxId", SqlDbType.NVarChar) { Value = (object)entity.TaxId?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NVarChar) { Value = (object)entity.Phone?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Mail", SqlDbType.NVarChar) { Value = (object)entity.Mail?.Value ?? DBNull.Value });

            // Nuevas columnas de la base de datos
            cmd.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar) { Value = (object)entity.Address?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@City", SqlDbType.NVarChar) { Value = (object)entity.City?.Value ?? DBNull.Value });

            // Observations ahora es un VO, así que le pedimos el .Value
            cmd.Parameters.Add(new SqlParameter("@Observations", SqlDbType.NVarChar) { Value = (object)entity.Observations?.Value ?? DBNull.Value });

            cmd.Parameters.Add(new SqlParameter("@DVH", SqlDbType.NVarChar) { Value = (object)entity.DVH ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = entity.Active }); // La base dice IsActive
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = entity.IsDeleted });
        }

        private Supplier Map(SqlDataReader reader)
        {
            // Helpers locales para lectura segura (evita excepciones de "Index was outside the bounds of the array" si la columna falta)
            bool HasColumn(SqlDataReader r, string columnName)
            {
                for (int i = 0; i < r.FieldCount; i++)
                {
                    if (r.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase)) return true;
                }
                return false;
            }

            string GetStringSafe(SqlDataReader r, string colName) =>
                HasColumn(r, colName) && r[colName] != DBNull.Value ? r[colName].ToString() : string.Empty;

            bool GetBoolSafe(SqlDataReader r, string colName) =>
                HasColumn(r, colName) && r[colName] != DBNull.Value && Convert.ToBoolean(r[colName]);


            // Usamos Reconstitute para instanciar la entidad a partir de la DB saltándonos validaciones de creación
            return Supplier.Reconstitute(
                id: (Guid)reader["Id"],
                rawCompanyName: GetStringSafe(reader, "CompanyName"),
                rawContactName: GetStringSafe(reader, "ContactName"),
                rawTaxId: GetStringSafe(reader, "TaxId"),
                rawPhone: GetStringSafe(reader, "Phone"),
                rawMail: GetStringSafe(reader, "Mail"),
                rawAddress: GetStringSafe(reader, "Address"), // <-- AGREGADO
                rawCity: GetStringSafe(reader, "City"),       // <-- AGREGADO
                observations: GetStringSafe(reader, "Observations"), // Pasa por el VO de observations en el Reconstitute
                dvh: GetStringSafe(reader, "DVH"),
                active: GetBoolSafe(reader, "IsActive"), // En la DB se llama IsActive, en C# se llama Active
                isDeleted: GetBoolSafe(reader, "IsDeleted")
            );
        }
    }
}