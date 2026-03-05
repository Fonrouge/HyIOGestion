using Domain.Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL.Persistence.MicrosoftSQL
{
    public class PaymentRepository : IPaymentRepository
    {
        private SqlTransaction _currentTransaction;
        private readonly IApplicationSettings _appSettings;

        public PaymentRepository(IApplicationSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void SetTransaction(object transaction)
        {
            _currentTransaction = (SqlTransaction)transaction;
        }

        public Task CreateAsync(Payment entity)
        {
            // Agregamos DVH
            string query = @"INSERT INTO Payments (Id, Amount, CreationDate, EffectiveDate, ClientId, Method, Reference, DVH) 
                             VALUES (@Id, @Amount, @CreationDate, @EffectiveDate, @ClientId, @Method, @Reference, @DVH)";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public Task UpdateAsync(Payment entity)
        {
            // Agregamos DVH
            string query = @"UPDATE Payments
                             SET Amount = @Amount, 
                                 CreationDate = @CreationDate, 
                                 EffectiveDate = @EffectiveDate, 
                                 ClientId = @ClientId, 
                                 Method = @Method, 
                                 Reference = @Reference,
                                 DVH = @DVH
                             WHERE Id = @Id";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public Task DeleteAsync(Guid entityId)
        {
            // Como discutimos, los pagos rara vez se borran físicamente. 
            // Si en tu negocio es un borrado físico, esto está bien. 
            // Si decidís usar borrado lógico más adelante, tendrás que cambiar esto a un UPDATE.
            string query = "DELETE FROM Payments WHERE Id = @Id";
            return ExecuteNonQueryAsync(query, cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entityId }));
        }

        public async Task<Payment> GetByIdAsync(Guid id)
        {
            Payment payment = null;
            string query = "SELECT Id, Amount, CreationDate, EffectiveDate, ClientId, Method, Reference, DVH FROM Payments WHERE Id = @Id";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }),
                reader => payment = Map(reader));

            return payment;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            var payments = new List<Payment>();
            string query = "SELECT Id, Amount, CreationDate, EffectiveDate, ClientId, Method, Reference, DVH FROM Payments";

            await ExecuteReaderAsync(query, null, reader => payments.Add(Map(reader)));

            return payments;
        }

        // --- MÉTODOS PRIVADOS DE INFRAESTRUCTURA (EL MOTOR) ---
        // (ExecuteNonQueryAsync y ExecuteReaderAsync quedan exactamente iguales, no hay cambios allí).

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

        private void SetParameters(SqlCommand cmd, Payment entity)
        {
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entity.Id });

            // Extraemos .Value de los Value Objects
            cmd.Parameters.Add(new SqlParameter("@Amount", SqlDbType.Decimal) { Value = entity.Amount?.Value ?? 0m });
            cmd.Parameters.Add(new SqlParameter("@CreationDate", SqlDbType.DateTime2) { Value = entity.CreationDate }); // DateTime2 es más preciso y seguro en SQL Server moderno
            cmd.Parameters.Add(new SqlParameter("@EffectiveDate", SqlDbType.DateTime2) { Value = entity.EffectiveDate });
            cmd.Parameters.Add(new SqlParameter("@ClientId", SqlDbType.UniqueIdentifier) { Value = entity.ClientId });

            cmd.Parameters.Add(new SqlParameter("@Method", SqlDbType.VarChar) { Value = (object)entity.Method?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Reference", SqlDbType.VarChar) { Value = (object)entity.Reference?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@DVH", SqlDbType.VarChar) { Value = (object)entity.DVH ?? DBNull.Value });
        }

        private Payment Map(SqlDataReader reader)
        {
            // Usamos el Factory Method Reconstitute para crear la entidad saltándonos las validaciones de "nuevo ingreso"
            return Payment.Reconstitute(
                id: (Guid)reader["Id"],
                rawAmount: (decimal)reader["Amount"],
                creationDate: (DateTime)reader["CreationDate"],
                effectiveDate: (DateTime)reader["EffectiveDate"],
                clientId: (Guid)reader["ClientId"],
                rawMethod: reader["Method"] != DBNull.Value ? reader["Method"].ToString() : string.Empty,
                rawReference: reader["Reference"] != DBNull.Value ? reader["Reference"].ToString() : string.Empty,
                dvh: reader["DVH"] != DBNull.Value ? reader["DVH"].ToString() : string.Empty
            );
        }
    }
}