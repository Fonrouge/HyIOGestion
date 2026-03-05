using Domain.BaseContracts;
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
                             (Id, Name, LastName, ShipAddress, WareHouseAddress, Email, Phone, TaxId, DocNumber, IsActive, IsDeleted) 
                             VALUES 
                             (@Id, @Name, @LastName, @ShipAddress, @WareHouseAddress, @Email, @Phone, @TaxId, @DocNumber, @IsActive, @IsDeleted)";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public Task UpdateAsync(Client entity)
        {
            string query = @"UPDATE Clients 
                             SET Name = @Name, 
                                 LastName = @LastName, 
                                 ShipAddress = @ShipAddress, 
                                 WareHouseAddress = @WareHouseAddress, 
                                 Email = @Email, 
                                 Phone = @Phone, 
                                 TaxId = @TaxId, 
                                 DocNumber = @DocNumber, 
                                 IsActive = @IsActive, 
                                 IsDeleted = @IsDeleted 
                             WHERE Id = @Id";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public async Task DeleteAsync(Guid entityId)
        {
            // Nota: En un Repo SQL, solemos ir directo al grano con el ID para evitar un Roundtrip (GetById) 
            // a menos que necesitemos validar algo muy específico del dominio antes de borrar.
            string query = "UPDATE Clients SET IsDeleted = 1, IsActive = 0 WHERE Id = @Id";
            await ExecuteNonQueryAsync(query, cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entityId }));
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            Client client = null;
            string query = @"SELECT Id, Name, LastName, ShipAddress, WareHouseAddress, Email, Phone, TaxId, DocNumber, IsActive, IsDeleted 
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
            string query = @"SELECT Id, Name, LastName, ShipAddress, WareHouseAddress, Email, Phone, TaxId, DocNumber, IsActive, IsDeleted 
                             FROM Clients 
                             WHERE IsDeleted = 0";

            await ExecuteReaderAsync(query, null, reader => clients.Add(Map(reader)));
            return clients;
        }

        public async Task<Client> GetByTaxIdAsync(string taxId)
        {
            Client client = null;

            // Es buena práctica usar un filtro de IsDeleted = 0 para no recuperar 
            // registros que técnicamente no deberían existir para la lógica de negocio activa.
            string query = @"SELECT Id, Name, LastName, ShipAddress, WareHouseAddress, Email, Phone, TaxId, DocNumber, IsActive, IsDeleted 
                     FROM Clients 
                     WHERE TaxId = @TaxId AND IsDeleted = 0";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@TaxId", SqlDbType.NVarChar) { Value = taxId }),
                reader => client = Map(reader));

            return client;
        }

        // --- MÉTODOS PRIVADOS REFACTORIZADOS ---
        private void SetParameters(SqlCommand cmd, Client entity)
        {
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entity.Id });

            // Accedemos a .Value porque SQL no entiende qué es un ClientNameVO
            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar) { Value = entity.Name.Value });
            cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar) { Value = entity.LastName.Value });
            cmd.Parameters.Add(new SqlParameter("@ShipAddress", SqlDbType.NVarChar) { Value = entity.ShipAddress.Value });
            cmd.Parameters.Add(new SqlParameter("@WareHouseAddress", SqlDbType.NVarChar) { Value = entity.WarehouseAddress.Value });
            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = entity.Email.Value });
            cmd.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NVarChar) { Value = entity.Phone.Value });
            cmd.Parameters.Add(new SqlParameter("@TaxId", SqlDbType.NVarChar) { Value = entity.TaxId.Value });
            cmd.Parameters.Add(new SqlParameter("@DocNumber", SqlDbType.NVarChar) { Value = entity.DocNumber.Value });

            cmd.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = entity.IsActive });
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = entity.IsDeleted });
        }

        private Client Map(SqlDataReader reader)
        {
            // Usamos RECONSTITUTE para saltarnos las validaciones de "Nueva Entidad"
            // y permitir que el dominio acepte datos que ya existen en la DB.
            return Client.Reconstitute(
                (Guid)reader["Id"],
                reader["Name"]?.ToString(),
                reader["LastName"]?.ToString(),
                reader["ShipAddress"]?.ToString(),
                reader["WareHouseAddress"]?.ToString(),
                reader["Email"]?.ToString(),
                reader["Phone"]?.ToString(),
                reader["TaxId"]?.ToString(),
                reader["DocNumber"]?.ToString(),
                (bool)reader["IsActive"],
                (bool)reader["IsDeleted"]
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

      