using Domain.Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Persistence.MicrosoftSQL
{
    public class SaleRepository : ISaleRepository
    {
        private SqlTransaction _currentTransaction;
        private readonly IApplicationSettings _appSettings;

        private class SaleHeader
        {
            public Guid Id { get; set; }
            public DateTime SaleDate { get; set; }
            public Guid ClientId { get; set; }          // ← int (igual que en tu entidad Sale)
            public Guid EmployeeId { get; set; }
            public decimal TotalAmountRaw { get; set; }
            public bool Active { get; set; }
            public DateTime CreatedAt { get; set; }
            public bool IsDeleted { get; set; }
        }

        public SaleRepository(IApplicationSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void SetTransaction(object transaction) => _currentTransaction = (SqlTransaction)transaction;

        public async Task Create(Sale entity)
        {
            string query = @"
                INSERT INTO Sales 
                    (Id, SaleDate, ClientId, EmployeeId, TotalAmount, Active, CreatedAt, IsDeleted)
                VALUES 
                    (@Id, @SaleDate, @ClientId, @EmployeeId, @TotalAmount, @Active, @CreatedAt, @IsDeleted)";

            await ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
            await SyncSaleDetails(entity.Id, entity.Items);
        }

        public async Task Update(Sale entity)
        {
            string query = @"
                UPDATE Sales 
                SET SaleDate   = @SaleDate,
                    ClientId   = @ClientId,
                    EmployeeId = @EmployeeId,
                    TotalAmount = @TotalAmount,
                    Active     = @Active,
                    IsDeleted  = @IsDeleted
                WHERE Id = @Id";

            await ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
            await DeleteExistingDetails(entity.Id);
            await SyncSaleDetails(entity.Id, entity.Items);
        }

        public async Task Delete(Guid entityId)
        {
            string query = "UPDATE Sales SET IsDeleted = 1, Active = 0 WHERE Id = @Id";
            await ExecuteNonQueryAsync(query, cmd => cmd.Parameters.AddWithValue("@Id", entityId));
        }

        public async Task<Sale> GetById(Guid id)
        {
            SaleHeader header = null;

            string query = @"
                SELECT Id, SaleDate, ClientId, EmployeeId, TotalAmount, Active, CreatedAt, IsDeleted
                FROM Sales 
                WHERE Id = @Id AND IsDeleted = 0";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.AddWithValue("@Id", id),
                reader => header = MapHeader(reader));

            return header == null ? null : await LoadFullAggregate(header);
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            var headers = new List<SaleHeader>();

            string query = @"
                SELECT Id, SaleDate, ClientId, EmployeeId, TotalAmount, Active, CreatedAt, IsDeleted
                FROM Sales 
                WHERE IsDeleted = 0
                ORDER BY SaleDate DESC";

            await ExecuteReaderAsync(query, null, reader => headers.Add(MapHeader(reader)));

            var sales = new List<Sale>();
            foreach (var h in headers)
                sales.Add(await LoadFullAggregate(h));

            return sales;
        }

        // ====================== PRIVADOS ======================

        private async Task ExecuteNonQueryAsync(string query, Action<SqlCommand> setParams)
        {
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
                        while (await reader.ReadAsync())
                            mapAction(reader);
                    }
                }
            }
            finally
            {
                if (ownConnection) conn.Dispose();
            }
        }

        private void SetParameters(SqlCommand cmd, Sale entity)
        {
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@SaleDate", entity.Date.Value);
            cmd.Parameters.AddWithValue("@ClientId", entity.ClientId);
            cmd.Parameters.AddWithValue("@EmployeeId", entity.EmployeeId);
            cmd.Parameters.AddWithValue("@TotalAmount", entity.TotalAmount.Value);
            cmd.Parameters.AddWithValue("@Active", entity.Active);
            cmd.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);
            cmd.Parameters.AddWithValue("@IsDeleted", entity.IsDeleted);
        }

        private SaleHeader MapHeader(SqlDataReader reader)
        {
            return new SaleHeader
            {
                Id = (Guid)reader["Id"],
                SaleDate = (DateTime)reader["SaleDate"],
                ClientId = (Guid)reader["ClientId"],           // ← int (igual que en tu entidad)
                EmployeeId = (Guid)reader["EmployeeId"],
                TotalAmountRaw = (decimal)reader["TotalAmount"],
                Active = (bool)reader["Active"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                IsDeleted = (bool)reader["IsDeleted"]
            };
        }

        private async Task<Sale> LoadFullAggregate(SaleHeader header)
        {
            var details = new List<SaleDetail>();

            string query = @"
                SELECT Id, SaleId, ProductId, Quantity, UnitPrice, SubTotal
                FROM SaleDetails 
                WHERE SaleId = @SaleId";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.AddWithValue("@SaleId", header.Id),
                reader => details.Add(MapDetail(reader)));

            return Sale.Reconstitute(
                id: header.Id,
                date: header.SaleDate,
                clientId: header.ClientId,
                employeeId: header.EmployeeId,
                totalAmountRaw: header.TotalAmountRaw,
                items: details,
                active: header.Active,
                createdAt: header.CreatedAt,
                isDeleted: header.IsDeleted
            );
        }

        private SaleDetail MapDetail(SqlDataReader reader)
        {
            return SaleDetail.Reconstitute(
                id: reader.GetGuid(reader.GetOrdinal("Id")),
                saleId: reader.GetGuid(reader.GetOrdinal("SaleId")),
                productId: reader.GetGuid(reader.GetOrdinal("ProductId")),
                quantityRaw: reader.GetDecimal(reader.GetOrdinal("Quantity")),
                unitPriceRaw: reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                subtotal: reader.IsDBNull(reader.GetOrdinal("SubTotal"))
                             ? 0m
                             : reader.GetDecimal(reader.GetOrdinal("SubTotal"))
            );
        }

        private async Task SyncSaleDetails(Guid saleId, IEnumerable<SaleDetail> details)
        {
            if (details == null || !details.Any()) return;

            string query = @"
                INSERT INTO SaleDetails (Id, SaleId, ProductId, Quantity, UnitPrice)
                VALUES (@Id, @SaleId, @ProductId, @Quantity, @UnitPrice)";

            foreach (var d in details)
            {
                await ExecuteNonQueryAsync(query, cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", d.Id);
                    cmd.Parameters.AddWithValue("@SaleId", saleId);
                    cmd.Parameters.AddWithValue("@ProductId", d.ProductId);
                    cmd.Parameters.AddWithValue("@Quantity", d.Quantity.Value);
                    cmd.Parameters.AddWithValue("@UnitPrice", d.UnitPrice.Value);
                    // NO insertamos SubTotal → es columna calculada por la BD
                });
            }
        }

        private async Task DeleteExistingDetails(Guid saleId)
        {
            string query = "DELETE FROM SaleDetails WHERE SaleId = @SaleId";
            await ExecuteNonQueryAsync(query, cmd => cmd.Parameters.AddWithValue("@SaleId", saleId));
        }
    }
}