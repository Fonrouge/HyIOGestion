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



        // Clase auxiliar para aplanar el resultado de la DB antes de hidratar el Agregado
        private class SaleHeader
        {
            public Guid Id { get; set; }
            public DateTime SaleDate { get; set; }
            public Guid ClientId { get; set; }
            public Guid EmployeeId { get; set; }
            public decimal TotalAmountRaw { get; set; }
            public bool Active { get; set; }
            public DateTime CreatedAt { get; set; }
            public bool IsDeleted { get; set; }
            public string DVH { get; set; } 
        }


        public SaleRepository(IApplicationSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void SetTransaction(object transaction) => _currentTransaction = (SqlTransaction)transaction;

        public async Task CreateAsync(Sale entity)
        {
            // Agregado campo DVH
            string query = @"
                INSERT INTO Sales 
                    (Id, SaleDate, ClientId, EmployeeId, TotalAmount, Active, CreatedAt, IsDeleted, DVH)
                VALUES 
                    (@Id, @SaleDate, @ClientId, @EmployeeId, @TotalAmount, @Active, @CreatedAt, @IsDeleted, @DVH)";

            await ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
            await SyncSaleDetails(entity.Id, entity.Items);
        }

        public async Task UpdateAsync(Sale entity)
        {
            // Agregado DVH e IsDeleted
            string query = @"
                UPDATE Sales 
                SET SaleDate   = @SaleDate,
                    ClientId   = @ClientId,
                    EmployeeId = @EmployeeId,
                    TotalAmount = @TotalAmount,
                    Active      = @Active,
                    IsDeleted   = @IsDeleted,
                    DVH         = @DVH
                WHERE Id = @Id";

            await ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));

            // Reemplazo de detalles (Patrón Agregado: se limpia y se vuelve a insertar)
            await DeleteExistingDetails(entity.Id);
            await SyncSaleDetails(entity.Id, entity.Items);
        }

        public async Task DeleteAsync(Guid entityId)
        {
            // Borrado FÍSICO. La BLL decide si usa este o el Update para borrado lógico.
            string deleteDetails = "DELETE FROM SaleDetails WHERE SaleId = @Id";
            string deleteSale = "DELETE FROM Sales WHERE Id = @Id";

            await ExecuteNonQueryAsync(deleteDetails, cmd => cmd.Parameters.AddWithValue("@Id", entityId));
            await ExecuteNonQueryAsync(deleteSale, cmd => cmd.Parameters.AddWithValue("@Id", entityId));
        }

        public async Task<Sale> GetByIdAsync(Guid id)
        {
            SaleHeader header = null;
            string query = @"SELECT Id, SaleDate, ClientId, EmployeeId, TotalAmount, Active, CreatedAt, IsDeleted, DVH 
                             FROM Sales WHERE Id = @Id";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.AddWithValue("@Id", id),
                reader => header = MapHeader(reader));

            return header == null ? null : await LoadFullAggregate(header);
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            var headers = new List<SaleHeader>();
            string query = @"SELECT Id, SaleDate, ClientId, EmployeeId, TotalAmount, Active, CreatedAt, IsDeleted, DVH 
                             FROM Sales ORDER BY SaleDate DESC";

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
            cmd.Parameters.AddWithValue("@DVH", (object)entity.DVH?.Value ?? DBNull.Value);
        }

        private SaleHeader MapHeader(SqlDataReader reader)
        {
            return new SaleHeader
            {
                Id = (Guid)reader["Id"],
                SaleDate = (DateTime)reader["SaleDate"],
                ClientId = (Guid)reader["ClientId"],
                EmployeeId = (Guid)reader["EmployeeId"],
                TotalAmountRaw = (decimal)reader["TotalAmount"],
                Active = (bool)reader["Active"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                IsDeleted = (bool)reader["IsDeleted"],
                DVH = reader["DVH"]?.ToString() ?? string.Empty
            };
        }

        private async Task<Sale> LoadFullAggregate(SaleHeader header)
        {
            var details = new List<SaleDetail>();
            // Agregado IsDeleted y DVH para los detalles
            string query = @"SELECT Id, SaleId, ProductId, Quantity, UnitPrice, SubTotal, IsDeleted, DVH 
                             FROM SaleDetails WHERE SaleId = @SaleId";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.AddWithValue("@SaleId", header.Id),
                reader => details.Add(MapDetail(reader)));

            // Reconstitución con los 10 parámetros requeridos
            return Sale.Reconstitute(
                id: header.Id,
                date: header.SaleDate,
                clientId: header.ClientId,
                employeeId: header.EmployeeId,
                totalAmountRaw: header.TotalAmountRaw,
                items: details,
                active: header.Active,
                createdAt: header.CreatedAt,
                isDeleted: header.IsDeleted,
                dvh: header.DVH
            );
        }

        private SaleDetail MapDetail(SqlDataReader reader)
        {
            return SaleDetail.Reconstitute
            (
                id: (Guid)reader["Id"],
                saleId: (Guid)reader["SaleId"],
                productId: (Guid)reader["ProductId"],
                quantityRaw: (decimal)reader["Quantity"],
                unitPriceRaw: (decimal)reader["UnitPrice"],
                subtotal: (decimal)reader["SubTotal"],
                isDeleted: (bool)reader["IsDeleted"],
                dvh: reader["DVH"]?.ToString() ?? string.Empty
            );
        }
        private async Task SyncSaleDetails(Guid saleId, IEnumerable<SaleDetail> details)
        {
            if (details == null || !details.Any()) return;

            // Incluimos DVH e IsDeleted en los detalles si así lo requiere tu diseño
            string query = @"INSERT INTO SaleDetails (Id, SaleId, ProductId, Quantity, UnitPrice, IsDeleted, DVH)
                             VALUES (@Id, @SaleId, @ProductId, @Quantity, @UnitPrice, @IsDeleted, @DVH)";

            foreach (var d in details)
            {
                await ExecuteNonQueryAsync(query, cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", d.Id);
                    cmd.Parameters.AddWithValue("@SaleId", saleId);
                    cmd.Parameters.AddWithValue("@ProductId", d.ProductId);
                    cmd.Parameters.AddWithValue("@Quantity", d.Quantity.Value);
                    cmd.Parameters.AddWithValue("@UnitPrice", d.UnitPrice.Value);
                    cmd.Parameters.AddWithValue("@IsDeleted", d.IsDeleted);
                    cmd.Parameters.AddWithValue("@DVH", (object)d.DVH?.Value ?? DBNull.Value);
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