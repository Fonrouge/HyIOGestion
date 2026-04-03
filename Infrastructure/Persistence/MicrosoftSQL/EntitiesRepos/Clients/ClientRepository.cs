using Domain.Entities;
using Domain.Repositories;
using Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL.Persistence.MicrosoftSQL
{
    public class ClientRepository : IClientRepository
    {
        private SqlTransaction _currentTransaction;
        private readonly IApplicationSettings _appSettings;

        public ClientRepository(IApplicationSettings appSettings) => _appSettings = appSettings;

        public void SetTransaction(object transaction) => _currentTransaction = (SqlTransaction)transaction;

        public Task CreateAsync(Client entity)
        {
            // Agregado el campo DVH
            string query = @"INSERT INTO Clients 
                             (Id, Name, LastName, ShipAddress, Email, Phone, TaxId, DocNumber, IsDeleted, DVH) 
                             VALUES 
                             (@Id, @Name, @LastName, @ShipAddress, @Email, @Phone, @TaxId, @DocNumber, @IsDeleted, @DVH)";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public Task UpdateAsync(Client entity)
        {
            // Agregado el campo DVH
            string query = @"UPDATE Clients 
                             SET Name = @Name, 
                                 LastName = @LastName, 
                                 ShipAddress = @ShipAddress, 
                                 Email = @Email, 
                                 Phone = @Phone, 
                                 TaxId = @TaxId, 
                                 DocNumber = @DocNumber, 
                                 IsDeleted = @IsDeleted,
                                 DVH = @DVH
                             WHERE Id = @Id";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public async Task DeleteAsync(Guid entityId)
        {
            // El borrado lógico se mantiene igual. 
            // OJO: Si calculás DVH de forma global (Digito Verificador Vertical), 
            // a veces es necesario recalcular el DVH del registro borrado para mantener la consistencia.
            string query = "UPDATE Clients SET IsDeleted = 1 = 0 WHERE Id = @Id";
            await ExecuteNonQueryAsync(query, cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entityId }));
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            Client client = null;
            // Agregado el campo DVH al SELECT
            string query = @"SELECT Id, Name, LastName, ShipAddress, Email, Phone, TaxId, DocNumber, IsDeleted, DVH 
                             FROM Clients 
                             WHERE Id = @Id AND IsDeleted = 0";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }),
                reader => client = Map(reader));

            return client;
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            var clients = new List<Client>();
            // Agregado el campo DVH al SELECT
            string query = @"SELECT Id, Name, LastName, ShipAddress, Email, Phone, TaxId, DocNumber, IsDeleted, DVH 
                             FROM Clients c
                             WHERE c.IsDeleted = 0";

            await ExecuteReaderAsync(query, null, reader => clients.Add(Map(reader)));
            return clients;
        }

        public async Task<Client> GetByDocNumberAsync(string taxId)
        {
            Client client = null;

            // Agregado el campo DVH al SELECT
            string query = @"SELECT Id, Name, LastName, ShipAddress, Email, Phone, TaxId, DocNumber, IsDeleted, DVH 
                             FROM dbo.Clients 
                             WHERE DocNumber = @DocNumber";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@DocNumber", SqlDbType.NVarChar) { Value = taxId }),
                reader => client = Map(reader));

            return client;
        }

        // --- MÉTODOS PRIVADOS REFACTORIZADOS ---
        private void SetParameters(SqlCommand cmd, Client entity)
        {
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entity.Id });

            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar) { Value = entity.Name.Value });
            cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar) { Value = entity.LastName.Value });
            cmd.Parameters.Add(new SqlParameter("@ShipAddress", SqlDbType.NVarChar) { Value = entity.ShipAddress.Value });
            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = entity.Email.Value });
            cmd.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NVarChar) { Value = entity.Phone.Value });
            cmd.Parameters.Add(new SqlParameter("@TaxId", SqlDbType.NVarChar) { Value = entity.TaxId.Value });
            cmd.Parameters.Add(new SqlParameter("@DocNumber", SqlDbType.NVarChar) { Value = entity.DocNumber.Value });
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = entity.IsDeleted });

            // Agregado el parámetro del DVH validando si es null por las dudas
            cmd.Parameters.Add(new SqlParameter("@DVH", SqlDbType.VarChar)
            {
                Value = (object)entity.DVH?.Value ?? string.Empty
            });
        }

        private Client Map(SqlDataReader reader)
        {           
            return Client.Reconstitute
            (
                (Guid)reader["Id"],
                reader["Name"]?.ToString(),
                reader["LastName"]?.ToString(),
                reader["ShipAddress"]?.ToString(),
                reader["Email"]?.ToString(),
                reader["Phone"]?.ToString(),
                reader["TaxId"]?.ToString(),
                reader["DocNumber"]?.ToString(),
                (bool)reader["IsDeleted"],
                reader["DVH"].ToString()
            );
        }

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