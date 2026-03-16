using Domain.Entities;
using Domain.Repositories;
using Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Persistence.MicrosoftSQL
{
    public class ProductRepository : IProductRepository
    {
        private SqlTransaction _currentTransaction;
        private readonly IApplicationSettings _appSettings;

        public ProductRepository(IApplicationSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void SetTransaction(object transaction)
        {
            _currentTransaction = (SqlTransaction)transaction;
        }

        public async Task CreateAsync(Product entity)
        {
            // Nota: Se asume que la columna en la DB es 'Id_Product' según tus screenshots anteriores
            string query = @"
                INSERT INTO Products
                    (Id, Name, Description, Price, Stock, IsActive, CreatedAt, IsDeleted, DVH)
                VALUES
                    (@Id, @Name, @Description, @Price, @Stock, @IsActive, @CreatedAt, @IsDeleted, @DVH)";

            await ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
            await SyncCategories(entity);
        }

        public async Task UpdateAsync(Product entity)
        {
            string query = @"
                UPDATE Products
                SET Name = @Name,
                    Description = @Description,
                    Price = @Price,
                    Stock = @Stock,
                    IsActive = @IsActive,
                    CreatedAt = @CreatedAt,
                    IsDeleted = @IsDeleted,
                    DVH = @DVH
                WHERE Id = @Id";

            await ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));

            //            // Limpiar relaciones antiguas
            //            string deleteRelations = "DELETE FROM ProductsCategories WHERE Id_Product = @Id";
            //         
            //            await ExecuteNonQueryAsync(deleteRelations, cmd =>
            //                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entity.Id }));
            //
            //            await SyncCategories(entity);
        }

        /// <summary>
        /// BORRADO FÍSICO (hard delete) - Elimina el producto y todas sus relaciones
        /// </summary>
        public async Task DeleteAsync(Guid entityId)
        {
            // Primero borramos la intermedia (Integridad referencial)
            string deleteRelations = "DELETE FROM ProductsCategories WHERE Id_Product = @Id";
            await ExecuteNonQueryAsync(deleteRelations, cmd =>
                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entityId }));

            string query = "DELETE FROM Products WHERE Id = @Id";
            await ExecuteNonQueryAsync(query, cmd =>
                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entityId }));
        }
        public async Task<Product> GetByIdAsync(Guid id)
        {
            Product product = null;
            string query = @"
                SELECT 
                    p.Id, p.Name, p.Description, p.Price, p.Stock, 
                    p.IsActive, p.CreatedAt, p.IsDeleted, p.DVH,
                    c.Id as CategoryId, 
                    c.Name as CategoryName, 
                    c.Description as CategoryDesc
                FROM Products p
                LEFT JOIN ProductsCategories pc ON p.Id = pc.Id_Product
                LEFT JOIN Categories c ON pc.Id_Category = c.Id
                WHERE p.Id = @Id
                  AND p.IsDeleted = 0";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }),
                reader =>
                {
                    if (product == null)
                        product = MapProduct(reader);
                    MapCategoryToProduct(reader, product);
                });

            return product;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var productDictionary = new Dictionary<Guid, Product>();

            string query = $@"
                              SELECT 
                                  p.Id, p.Name, p.Description, p.Price, p.Stock, 
                                  p.IsActive, p.CreatedAt, p.IsDeleted, p.DVH,
                                  c.Id as CategoryId, 
                                  c.Name as CategoryName, 
                                  c.Description as CategoryDesc
                              FROM Products p
                              LEFT JOIN ProductsCategories pc ON p.Id = pc.Id_Product
                              LEFT JOIN Categories c ON pc.Id_Category = c.Id
                              WHERE p.IsDeleted = 0
                              ORDER BY p.Name
                              ";

            await ExecuteReaderAsync(query, null, reader =>
            {
                Guid productId = (Guid)reader["Id"];
                if (!productDictionary.TryGetValue(productId, out Product product))
                {
                    product = MapProduct(reader);
                    productDictionary.Add(productId, product);
                }
                MapCategoryToProduct(reader, product);
            });

            return productDictionary.Values;
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            Product product = null;
            string query = @"
                SELECT 
                    p.Id, p.Name, p.Description, p.Price, p.Stock, 
                    p.IsActive, p.CreatedAt, p.IsDeleted, p.DVH,
                    c.Id as CategoryId, 
                    c.Name as CategoryName, 
                    c.Description as CategoryDesc
                FROM Products p
                LEFT JOIN ProductsCategories pc ON p.Id = pc.Id_Product
                LEFT JOIN Categories c ON pc.Id_Category = c.Id
                WHERE p.Name = @Name
                  AND p.IsDeleted = 0";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar) { Value = name }),
                reader =>
                {
                    if (product == null)
                        product = MapProduct(reader);
                    MapCategoryToProduct(reader, product);
                });

            return product;
        }

        // ====================== MÉTODOS PRIVADOS ======================

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
                        while (await reader.ReadAsync())
                            mapAction(reader);
                    }
                }
            }
            finally
            {
                if (!isExternalConn) conn.Dispose();
            }
        }

        private void SetParameters(SqlCommand cmd, Product entity)
        {
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entity.Id });
            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar) { Value = entity.Name.Value });
            cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar) { Value = (object)entity.Description?.Value ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { Value = entity.Price.Value });
            cmd.Parameters.Add(new SqlParameter("@Stock", SqlDbType.Decimal) { Value = entity.Stock.Value });
            cmd.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = entity.Active });
            cmd.Parameters.Add(new SqlParameter("@CreatedAt", SqlDbType.DateTime2) { Value = entity.CreatedAt });
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = entity.IsDeleted });

            // SOLUCIÓN A LA EXCEPCIÓN: Si el DVH es null en C#, enviamos DBNull a SQL
            // Si es un Value Object, accedemos a .Value
            cmd.Parameters.Add(new SqlParameter("@DVH", SqlDbType.VarChar)
            {
                Value = (object)entity.DVH?.Value ?? string.Empty
            });
        }

        private async Task SyncCategories(Product product)
        {
            if (product.Categories == null || !product.Categories.Any()) return;

            // Incluimos el campo DVH en la tabla intermedia
            string query = @"INSERT INTO ProductsCategories (Id_Product, Id_Category, DVH) 
                             VALUES (@IdProduct, @IdCategory, @DVH)";

            foreach (var category in product.Categories)
            {
                await ExecuteNonQueryAsync(query, cmd =>
                {
                    cmd.Parameters.Add(new SqlParameter("@IdProduct", SqlDbType.UniqueIdentifier) { Value = product.Id });
                    cmd.Parameters.Add(new SqlParameter("@IdCategory", SqlDbType.UniqueIdentifier) { Value = category.Id });

                    // Aquí la integridad de la relación: 
                    // Se suele hashear la combinación de ambos IDs o usar el DVH que la BLL asignó a la relación
                    cmd.Parameters.Add(new SqlParameter("@DVH", SqlDbType.VarChar)
                    {
                        Value = (object)product.DVH?.Value ?? string.Empty
                    });
                });
            }
        }

        private Product MapProduct(SqlDataReader reader)
        {
            return Product.Reconstitute
            (
                id: (Guid)reader["Id"],
                rawName: reader["Name"].ToString(),
                rawDescription: reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty,
                rawPrice: Convert.ToDecimal(reader["Price"]),
                rawStock: Convert.ToDecimal(reader["Stock"]),
                categories: new List<Category>(),
                active: (bool)reader["IsActive"],
                createdAt: (DateTime)reader["CreatedAt"],
                isDeleted: reader["IsDeleted"] != DBNull.Value && (bool)reader["IsDeleted"],
                dvh: reader["DVH"].ToString()
            );
        }

        private void MapCategoryToProduct(SqlDataReader reader, Product product)
        {
            if (reader["CategoryId"] != DBNull.Value)
            {
                var category = Category.Create
                (
                    name: reader["CategoryName"].ToString(),
                    description: reader["CategoryDesc"] != DBNull.Value
                        ? reader["CategoryDesc"].ToString()
                        : string.Empty
                );

                ((List<Category>)product.Categories).Add(category);
            }
        }
    }
}