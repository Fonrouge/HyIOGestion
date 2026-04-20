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
            string query = @"INSERT INTO Clients 
                             (Id, Name, LastName, ShipAddress, Email, Phone, TaxId, DocNumber, 
                              Observations, ShipCountry, ShipState, ShipZipCode, IsDeleted, DVH) 
                             VALUES 
                             (@Id, @Name, @LastName, @ShipAddress, @Email, @Phone, @TaxId, @DocNumber, 
                              @Observations, @ShipCountry, @ShipState, @ShipZipCode, @IsDeleted, @DVH)";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public Task UpdateAsync(Client entity)
        {
            string query = @"UPDATE Clients 
                             SET Name = @Name, 
                                 LastName = @LastName, 
                                 ShipAddress = @ShipAddress, 
                                 Email = @Email, 
                                 Phone = @Phone, 
                                 TaxId = @TaxId, 
                                 DocNumber = @DocNumber,
                                 Observations = @Observations,
                                 ShipCountry = @ShipCountry,
                                 ShipState = @ShipState,
                                 ShipZipCode = @ShipZipCode,
                                 IsDeleted = @IsDeleted,
                                 DVH = @DVH
                             WHERE Id = @Id";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public async Task DeleteAsync(Guid entityId)
        {
            // Corregido el typo: "SET IsDeleted = 1 = 0" -> "SET IsDeleted = 1"
            string query = "UPDATE Clients SET IsDeleted = 1 WHERE Id = @Id";
            await ExecuteNonQueryAsync(query, cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entityId }));
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            Client client = null;
            string query = @"SELECT Id, Name, LastName, ShipAddress, Email, Phone, TaxId, DocNumber, 
                                    Observations, ShipCountry, ShipState, ShipZipCode, IsDeleted, DVH 
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
            string query = @"SELECT Id, Name, LastName, ShipAddress, Email, Phone, TaxId, DocNumber, 
                                    Observations, ShipCountry, ShipState, ShipZipCode, IsDeleted, DVH 
                             FROM Clients c
                             WHERE c.IsDeleted = 0";

            await ExecuteReaderAsync(query, null, reader => clients.Add(Map(reader)));
            return clients;
        }

        public async Task<IEnumerable<Client>> GetAllDeletedAsync()
        {
            var clients = new List<Client>();
            string query = @"SELECT Id, Name, LastName, ShipAddress, Email, Phone, TaxId, DocNumber, 
                                    Observations, ShipCountry, ShipState, ShipZipCode, IsDeleted, DVH 
                             FROM Clients c
                             WHERE c.IsDeleted = 1";

            await ExecuteReaderAsync(query, null, reader => clients.Add(Map(reader)));
            return clients;
        }

        public async Task<Client> GetByDocNumberAsync(string taxId)
        {
            Client client = null;
            string query = @"SELECT Id, Name, LastName, ShipAddress, Email, Phone, TaxId, DocNumber, 
                                    Observations, ShipCountry, ShipState, ShipZipCode, IsDeleted, DVH 
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

            // Nuevos campos
            cmd.Parameters.Add(new SqlParameter("@Observations", SqlDbType.NVarChar) { Value = (object)entity.Observations?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@ShipCountry", SqlDbType.NVarChar) { Value = (object)entity.ShipCountry ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@ShipState", SqlDbType.NVarChar) { Value = (object)entity.ShipState ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@ShipZipCode", SqlDbType.NVarChar) { Value = (object)entity.ShipZipCode?.Value ?? DBNull.Value });

            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = entity.IsDeleted });

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
                reader["Observations"]?.ToString(),
                reader["ShipCountry"]?.ToString(),
                reader["ShipState"]?.ToString(),
                reader["ShipZipCode"]?.ToString(),
                (bool)reader["IsDeleted"],
                reader["DVH"].ToString()
            );
        }

        // Métodos ExecuteNonQueryAsync y ExecuteReaderAsync se mantienen iguales...
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