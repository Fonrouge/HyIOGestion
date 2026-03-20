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

        public void SetTransaction(object transaction) => _currentTransaction = (SqlTransaction)transaction;

        // --- ACCIONES DE PERSISTENCIA ---

        public Task CreateAsync(SaleDetail entity) =>
            ExecuteNonQueryAsync(
                @"INSERT INTO SaleDetails (Id, SaleId, ProductId, Quantity, UnitPrice, IsDeleted, DVH)
                  VALUES (@Id, @SaleId, @ProductId, @Quantity, @UnitPrice, @IsDeleted, @DVH)",
                cmd => SetParameters(cmd, entity));

        public async Task UpdateAsync(SaleDetail entity)
        {
            Console.WriteLine($"[DEBUG REPO SALEDETAIL] === INICIO Update ID: {entity.Id} ===");
            Console.WriteLine($"[DEBUG REPO SALEDETAIL] IsDeleted que llega: {entity.IsDeleted}");
            Console.WriteLine($"[DEBUG REPO SALEDETAIL] ¿Tiene transacción? {_currentTransaction != null}");

            await ExecuteNonQueryAsync(
                @"UPDATE SaleDetails
          SET SaleId = @SaleId, ProductId = @ProductId, Quantity = @Quantity,
              UnitPrice = @UnitPrice, IsDeleted = @IsDeleted, DVH = @DVH
          WHERE Id = @Id",
                cmd => SetParameters(cmd, entity));

            Console.WriteLine($"[DEBUG REPO SALEDETAIL] === FIN Update ID: {entity.Id} ===");
        }

        public Task DeleteAsync(Guid entityId) =>
            ExecuteNonQueryAsync("DELETE FROM SaleDetails WHERE Id = @Id",
                cmd => cmd.Parameters.AddWithValue("@Id", entityId));

        // --- MÉTODOS DE LECTURA ---

        public async Task<SaleDetail> GetByIdAsync(Guid id)
        {
            SaleDetail detail = null;
            string query = @"SELECT Id, SaleId, ProductId, Quantity, UnitPrice, SubTotal, IsDeleted, DVH 
                             FROM SaleDetails WHERE Id = @Id";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.AddWithValue("@Id", id),
                reader => detail = Map(reader));
            return detail;
        }

        public async Task<IEnumerable<SaleDetail>> GetAllAsync()
        {
            var details = new List<SaleDetail>();
            string query = @"SELECT Id, SaleId, ProductId, Quantity, UnitPrice, SubTotal, IsDeleted, DVH 
                             FROM SaleDetails";

            await ExecuteReaderAsync(query, null, reader => details.Add(Map(reader)));
            return details;
        }

        // --- MAPEO Y PARÁMETROS ---

        private void SetParameters(SqlCommand cmd, SaleDetail entity)
        {
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@SaleId", entity.SaleId);
            cmd.Parameters.AddWithValue("@ProductId", entity.ProductId);

            // Usamos .Value (decimal) para soportar fracciones
            cmd.Parameters.AddWithValue("@Quantity", entity.Quantity.Value);
            cmd.Parameters.AddWithValue("@UnitPrice", entity.UnitPrice.Value);

            // Campos de control
            cmd.Parameters.AddWithValue("@IsDeleted", entity.IsDeleted);
            cmd.Parameters.AddWithValue("@DVH", (object)entity.DVH?.Value ?? DBNull.Value);
        }

        private SaleDetail Map(SqlDataReader r)
        {
            // Recuperamos como decimal para mantener precisión
            decimal qty = Convert.ToDecimal(r["Quantity"]);
            decimal price = Convert.ToDecimal(r["UnitPrice"]);
            decimal subtotal = Convert.ToDecimal(r["SubTotal"]);

            // Reconstitución con los 8 parámetros sincronizados
            return SaleDetail.Reconstitute(
                id: (Guid)r["Id"],
                saleId: (Guid)r["SaleId"],
                productId: (Guid)r["ProductId"],
                quantityRaw: qty,
                unitPriceRaw: price,
                subtotal: subtotal,
                isDeleted: (bool)r["IsDeleted"],
                dvh: r["DVH"]?.ToString() ?? string.Empty
            );
        }

        // --- INFRAESTRUCTURA ---
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

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();   // ← cambiado
                    Console.WriteLine($"[DEBUG REPO SALEDETAIL] Filas afectadas: {rowsAffected}");  // ← nueva línea

                    return; // si tenías return type void
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

    
    }
}