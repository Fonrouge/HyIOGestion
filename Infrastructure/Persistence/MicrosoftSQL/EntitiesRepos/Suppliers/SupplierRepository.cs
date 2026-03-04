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

        public SupplierRepository(IApplicationSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void SetTransaction(object transaction)
        {
            _currentTransaction = (SqlTransaction)transaction;
        }

        public Task Create(Supplier entity)
        {
            // Se agregaron DVH, Active e IsDeleted para reflejar el estado real del dominio
            string query = @"INSERT INTO Supplier 
                             (Id, CompanyName, ContactName, TaxId, Phone, Mail, Observations, DVH, Active, IsDeleted) 
                             VALUES 
                             (@Id, @CompanyName, @ContactName, @TaxId, @Phone, @Mail, @Observations, @DVH, @Active, @IsDeleted)";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public Task Update(Supplier entity)
        {
            string query = @"UPDATE Supplier 
                             SET CompanyName = @CompanyName, 
                                 ContactName = @ContactName, 
                                 TaxId = @TaxId, 
                                 Phone = @Phone, 
                                 Mail = @Mail, 
                                 Observations = @Observations,
                                 DVH = @DVH,
                                 Active = @Active,
                                 IsDeleted = @IsDeleted
                             WHERE Id = @Id";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public Task Delete(Guid entityId)
        {
            // Como implementas ISoftDeletable, usualmente Delete hace un UPDATE (borrado lógico).
            // Si prefieres borrado físico, puedes volver al "DELETE FROM Supplier WHERE Id = @Id".
            string query = "UPDATE Supplier SET IsDeleted = 1, Active = 0 WHERE Id = @Id";
            return ExecuteNonQueryAsync(query, cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entityId }));
        }

        public async Task<Supplier> GetById(Guid id)
        {
            Supplier supplier = null;
            string query = "SELECT Id, CompanyName, ContactName, TaxId, Phone, Mail, Observations, DVH, Active, IsDeleted FROM Supplier WHERE Id = @Id AND IsDeleted = 0";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }),
                reader => supplier = Map(reader));

            return supplier;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            var suppliers = new List<Supplier>();
            // Solo traemos los no eliminados lógicamente
            string query = "SELECT Id, CompanyName, ContactName, TaxId, Phone, Mail, Observations, DVH, Active, IsDeleted FROM Supplier WHERE IsDeleted = 0";

            await ExecuteReaderAsync(query, null, reader => suppliers.Add(Map(reader)));

            return suppliers;
        }

        public async Task<Supplier> GetByTaxIdAsync(string taxId)
        {
            Supplier supplier = null;
            string query = "SELECT Id, CompanyName, ContactName, TaxId, Phone, Mail, Observations, DVH, Active, IsDeleted FROM Supplier WHERE TaxId = @TaxId AND IsDeleted = 0";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@TaxId", SqlDbType.VarChar) { Value = taxId }),
                reader => supplier = Map(reader));

            return supplier;
        }

        // --- MÉTODOS PRIVADOS DE INFRAESTRUCTURA ---
        // (ExecuteNonQueryAsync y ExecuteReaderAsync quedan IGUAL, no los repito para no saturar)

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

            // Extraemos los valores primitivos (.Value) de los ValueObjects para guardarlos
            cmd.Parameters.Add(new SqlParameter("@CompanyName", SqlDbType.VarChar) { Value = (object)entity.CompanyName?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@ContactName", SqlDbType.VarChar) { Value = (object)entity.ContactName?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@TaxId", SqlDbType.VarChar) { Value = (object)entity.TaxId?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Phone", SqlDbType.VarChar) { Value = (object)entity.Phone?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Mail", SqlDbType.VarChar) { Value = (object)entity.Mail?.Value ?? DBNull.Value });

            cmd.Parameters.Add(new SqlParameter("@Observations", SqlDbType.VarChar) { Value = (object)entity.Observations ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@DVH", SqlDbType.VarChar) { Value = (object)entity.DVH ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit) { Value = entity.Active });
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = entity.IsDeleted });
        }

        private Supplier Map(SqlDataReader reader)
        {
            // Usamos Reconstitute para instanciar la entidad a partir de la DB saltándonos validaciones de creación
            return Supplier.Reconstitute(
                id: (Guid)reader["Id"],
                rawCompanyName: reader["CompanyName"] != DBNull.Value ? reader["CompanyName"].ToString() : string.Empty,
                rawContactName: reader["ContactName"] != DBNull.Value ? reader["ContactName"].ToString() : string.Empty,
                rawTaxId: reader["TaxId"] != DBNull.Value ? reader["TaxId"].ToString() : string.Empty,
                rawPhone: reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : string.Empty,
                rawMail: reader["Mail"] != DBNull.Value ? reader["Mail"].ToString() : string.Empty,
                observations: reader["Observations"] != DBNull.Value ? reader["Observations"].ToString() : string.Empty,
                dvh: reader["DVH"] != DBNull.Value ? reader["DVH"].ToString() : string.Empty,
                active: reader["Active"] != DBNull.Value && Convert.ToBoolean(reader["Active"]),
                isDeleted: reader["IsDeleted"] != DBNull.Value && Convert.ToBoolean(reader["IsDeleted"])
            );
        }
    }
}