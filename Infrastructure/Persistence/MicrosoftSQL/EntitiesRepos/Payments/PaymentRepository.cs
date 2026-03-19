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
            string query = @"INSERT INTO Payments (Id, Amount, CreationDate, EffectiveDate, SaleId, Method, Reference, DVH) 
                             VALUES (@Id, @Amount, @CreationDate, @EffectiveDate, @SaleId, @Method, @Reference, @DVH)";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public Task UpdateAsync(Payment entity)
        {
            string query = @"UPDATE Payments
                     SET Amount = @Amount, 
                         CreationDate = @CreationDate, 
                         EffectiveDate = @EffectiveDate, 
                         SaleId = @SaleId, 
                         Method = @Method, 
                         Reference = @Reference,
                         DVH = @DVH,
                         IsDeleted = @IsDeleted
                     WHERE Id = @Id";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public Task DeleteAsync(Guid entityId)
        {
            // Borrado físico directo. La BLL decide cuándo invocarlo.
            string query = "DELETE FROM Payments WHERE Id = @Id";
            return ExecuteNonQueryAsync(query, cmd =>
                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entityId }));
        }

        public async Task<Payment> GetByIdAsync(Guid id)
        {
            Payment payment = null;
            string query = "SELECT Id, Amount, CreationDate, EffectiveDate, SaleId, Method, Reference, DVH, IsDeleted FROM Payments WHERE Id = @Id";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }),
                reader => payment = Map(reader));

            return payment;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            var payments = new List<Payment>();
            string query = "SELECT Id, Amount, CreationDate, EffectiveDate, SaleId, Method, Reference, DVH, IsDeleted FROM Payments";

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
            cmd.Parameters.Add(new SqlParameter("@Amount", SqlDbType.Decimal) { Value = entity.Amount?.Value ?? 0m });
            cmd.Parameters.Add(new SqlParameter("@CreationDate", SqlDbType.DateTime2) { Value = entity.CreationDate });
            cmd.Parameters.Add(new SqlParameter("@EffectiveDate", SqlDbType.DateTime2) { Value = entity.EffectiveDate });
            cmd.Parameters.Add(new SqlParameter("@SaleId", SqlDbType.UniqueIdentifier) { Value = entity.SaleId });

            // Acceso correcto a los Value Objects
            cmd.Parameters.Add(new SqlParameter("@Method", SqlDbType.VarChar) { Value = (object)entity.Method?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Reference", SqlDbType.VarChar) { Value = (object)entity.Reference?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@DVH", SqlDbType.VarChar) { Value = (object)entity.DVH?.Value ?? DBNull.Value });

            // La DAL simplemente persiste lo que la BLL mande en esta propiedad
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = entity.IsDeleted });
        }

        private Payment Map(SqlDataReader reader)
        {
            return Payment.Reconstitute(
                id: (Guid)reader["Id"],
                rawAmount: (decimal)reader["Amount"],
                creationDate: (DateTime)reader["CreationDate"],
                effectiveDate: (DateTime)reader["EffectiveDate"],
                saleId: (Guid)reader["SaleId"],
                rawMethod: reader["Method"]?.ToString(),
                rawReference: reader["Reference"]?.ToString(),
                dvh: reader["DVH"]?.ToString(),
                isDeleted: (bool)reader["IsDeleted"]
            );
        }
    }
}