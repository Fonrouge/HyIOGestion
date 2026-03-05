using Domain.Entities;
using Domain.Repositories;
using Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL.Persistence.MicrosoftSQL
{
    public class SaleDetailRepository : ISaleDetailRepository
    {
        private SqlTransaction _currentTransaction;
        private readonly IApplicationSettings _appSettings;

        public SaleDetailRepository(IApplicationSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void SetTransaction(object transaction)
        {
            _currentTransaction = (SqlTransaction)transaction;
        }

        public Task CreateAsync(SaleDetail entity) =>
            ExecuteNonQueryAsync(
                @"INSERT INTO SaleDetails (Id, SaleId, ProductId, Quantity, UnitPrice)
                  VALUES (@Id, @SaleId, @ProductId, @Quantity, @UnitPrice)",
                cmd => SetParameters(cmd, entity));

        public Task UpdateAsync(SaleDetail entity) =>
            ExecuteNonQueryAsync(
                @"UPDATE SaleDetails SET SaleId = @SaleId, ProductId = @ProductId,
                    Quantity = @Quantity, UnitPrice = @UnitPrice
                  WHERE Id = @Id",
                cmd => SetParameters(cmd, entity));

        public Task DeleteAsync(Guid entityId) =>
            ExecuteNonQueryAsync("DELETE FROM SaleDetails WHERE Id = @Id",
                cmd => cmd.Parameters.AddWithValue("@Id", entityId));

        public async Task<SaleDetail> GetByIdAsync(Guid id)
        {
            SaleDetail detail = null;
            await ExecuteReaderAsync(
                "SELECT Id, SaleId, ProductId, Quantity, UnitPrice FROM SaleDetails WHERE Id = @Id",
                cmd => cmd.Parameters.AddWithValue("@Id", id),
                reader => detail = Map(reader));
            return detail;
        }

        public async Task<IEnumerable<SaleDetail>> GetAllAsync()
        {
            var details = new List<SaleDetail>();
            await ExecuteReaderAsync(
                "SELECT Id, SaleId, ProductId, Quantity, UnitPrice FROM SaleDetails",
                null,
                reader => details.Add(Map(reader)));
            return details;
        }

        // --- INFRAESTRUCTURA ---
        private async Task ExecuteNonQueryAsync(string query, Action<SqlCommand> setParams)
        {
            // (mismo código que tenías, sin cambios)
            SqlConnection conn = _currentTransaction?.Connection ?? new SqlConnection(_appSettings.EntitiesConnection);
            bool ownConnection = _currentTransaction == null;
            try
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    if (_currentTransaction != null) cmd.Transaction = _currentTransaction;
                    setParams?.Invoke(cmd);
                    if (ownConnection) await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            finally
            {
                if (ownConnection) conn.Dispose();
            }
        }

        private async Task ExecuteReaderAsync(string query, Action<SqlCommand> setParams, Action<SqlDataReader> mapAction)
        {
            // (mismo código que tenías)
            SqlConnection conn = _currentTransaction?.Connection ?? new SqlConnection(_appSettings.EntitiesConnection);
            bool ownConnection = _currentTransaction == null;
            try
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    if (_currentTransaction != null) cmd.Transaction = _currentTransaction;
                    setParams?.Invoke(cmd);

                    if (ownConnection) await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync()) mapAction(reader);
                    }
                }
            }
            finally
            {
                if (ownConnection) conn.Dispose();
            }
        }

        private void SetParameters(SqlCommand cmd, SaleDetail entity)
        {
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@SaleId", entity.SaleId);
            cmd.Parameters.AddWithValue("@ProductId", entity.ProductId);
            cmd.Parameters.AddWithValue("@Quantity", entity.Quantity.Value);
            cmd.Parameters.AddWithValue("@UnitPrice", entity.UnitPrice.Value);
        }

        private SaleDetail Map(SqlDataReader r)
        {
            int qty = (int)r["Quantity"];
            decimal price = (decimal)r["UnitPrice"];

            return SaleDetail.Reconstitute(
                id: (Guid)r["Id"],
                saleId: (Guid)r["SaleId"],
                productId: (Guid)r["ProductId"],
                quantityRaw: qty,
                unitPriceRaw: price,
                subtotal: qty * price
            );
        }
    }
}