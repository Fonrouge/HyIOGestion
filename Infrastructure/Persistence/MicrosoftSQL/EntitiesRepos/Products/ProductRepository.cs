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
            // 1. Insertar el Producto
            string query = @"
                INSERT INTO Products 
                    (Id_Product, Name, Description, Price, Stock, IsActive, CreatedAt, IsDeleted)
                VALUES 
                    (@Id, @Name, @Description, @Price, @Stock, @IsActive, @CreatedAt, @IsDeleted)";

            await ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));

            // 2. Insertar las relaciones en ProductsCategories
            await SyncCategories(entity.Id, entity.Categories);
        }

        public async Task UpdateAsync(Product entity)
        {
            // 1. Actualizar el Producto
            string query = @"
                UPDATE Products
                SET Name = @Name, 
                    Description = @Description, 
                    Price = @Price,
                    Stock = @Stock, 
                    IsActive = @IsActive, 
                    CreatedAt = @CreatedAt,
                    IsDeleted = @IsDeleted
                WHERE Id_Product = @Id";

            await ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));

            // 2. Limpiar las relaciones viejas y guardar las nuevas
            string deleteRelations = "DELETE FROM ProductsCategories WHERE Id_Product = @Id";
            await ExecuteNonQueryAsync(deleteRelations, cmd =>
                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entity.Id }));

            await SyncCategories(entity.Id, entity.Categories);
        }

        /// <summary>
        /// Soft Delete (consistente con ISoftDeletable y la clase Employee)
        /// </summary>
        public async Task DeleteAsync(Guid entityId)
        {
            string query = @"
                UPDATE Products 
                SET IsDeleted = 1, 
                    IsActive = 0 
                WHERE Id_Product = @Id";

            await ExecuteNonQueryAsync(query, cmd =>
                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entityId }));
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            Product product = null;

            string query = @"
                SELECT p.Id_Product, p.Name, p.Description, p.Price, p.Stock, p.IsActive, p.CreatedAt, p.IsDeleted,
                       c.Id_Category, c.Name as CategoryName, c.Description as CategoryDesc
                FROM Products p
                LEFT JOIN ProductsCategories pc ON p.Id_Product = pc.Id_Product
                LEFT JOIN Categories c ON pc.Id_Category = c.Id_Category
                WHERE p.Id_Product = @Id 
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

            string query = @"
                SELECT p.Id_Product, p.Name, p.Description, p.Price, p.Stock, p.IsActive, p.CreatedAt, p.IsDeleted,
                       c.Id_Category, c.Name as CategoryName, c.Description as CategoryDesc
                FROM Products p
                LEFT JOIN ProductsCategories pc ON p.Id_Product = pc.Id_Product
                LEFT JOIN Categories c ON pc.Id_Category = c.Id_Category
                WHERE p.IsDeleted = 0
                ORDER BY p.Name";

            await ExecuteReaderAsync(query, null, reader =>
            {
                Guid productId = (Guid)reader["Id_Product"];

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

            // Reutilizamos la query de GetById pero filtrando por Name
            string query = @"
                             SELECT p.Id_Product, p.Name, p.Description, p.Price, p.Stock, p.IsActive, p.CreatedAt, p.IsDeleted,
                                    c.Id_Category, c.Name as CategoryName, c.Description as CategoryDesc
                             FROM Products p
                             LEFT JOIN ProductsCategories pc ON p.Id_Product = pc.Id_Product
                             LEFT JOIN Categories c ON pc.Id_Category = c.Id_Category
                             WHERE p.Name = @Name 
                               AND p.IsDeleted = 0";

            await ExecuteReaderAsync(query,
                // Usamos VarChar para ser consistentes con tu método SetParameters
                cmd => cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar) { Value = name }),
                reader =>
                {
                    // Si es la primera fila, creamos la instancia del Producto
                    if (product == null)
                        product = MapProduct(reader);

                    // En cada fila (tenga una o varias), intentamos mapear la categoría asociada
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

            if (string.IsNullOrEmpty(entity.Description?.Value))
            {
                cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar)
                {
                    Value = DBNull.Value
                });
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar)
                {
                    Value = entity.Description.Value
                });
            }

            cmd.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { Value = entity.Price.Value });
            cmd.Parameters.Add(new SqlParameter("@Stock", SqlDbType.Int) { Value = entity.Stock.Value });
            cmd.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = entity.Active });
            cmd.Parameters.Add(new SqlParameter("@CreatedAt", SqlDbType.DateTime2) { Value = entity.CreatedAt });
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = entity.IsDeleted });
        }

        private async Task SyncCategories(Guid productId, IEnumerable<Category> categories)
        {
            if (categories == null || !categories.Any()) return;

            string query = "INSERT INTO ProductsCategories (Id_Product, Id_Category) VALUES (@IdProduct, @IdCategory)";

            foreach (var category in categories)
            {
                await ExecuteNonQueryAsync(query, cmd =>
                {
                    cmd.Parameters.Add(new SqlParameter("@IdProduct", SqlDbType.UniqueIdentifier) { Value = productId });
                    cmd.Parameters.Add(new SqlParameter("@IdCategory", SqlDbType.UniqueIdentifier) { Value = category.Id });
                });
            }
        }

        /// <summary>
        /// Mapea desde la BD usando el factory Reconstitute (Rich Domain Model)
        /// </summary>
        private Product MapProduct(SqlDataReader reader)
        {
            return Product.Reconstitute(
                id: (Guid)reader["Id_Product"],
                rawName: reader["Name"].ToString(),
                rawDescription: reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty,

                // ✅ Convert.ToDecimal es "todoterreno" y evita el InvalidCastException
                rawPrice: Convert.ToDecimal(reader["Price"]),
                rawStock: Convert.ToDecimal(reader["Stock"]),

                categories: new List<Category>(),
                active: (bool)reader["IsActive"],
                createdAt: (DateTime)reader["CreatedAt"],
                isDeleted: reader["IsDeleted"] != DBNull.Value && (bool)reader["IsDeleted"]
            );
        }
        private void MapCategoryToProduct(SqlDataReader reader, Product product)
        {
            if (reader["Id_Category"] != DBNull.Value)
            {
                var category = new Category
                {
                    Id = (Guid)reader["Id_Category"],
                    Name = reader["CategoryName"].ToString(),
                    Description = reader["CategoryDesc"] != DBNull.Value
                        ? reader["CategoryDesc"].ToString()
                        : string.Empty
                };

                // Cast seguro porque sabemos que Categories es List<Category> internamente
                ((List<Category>)product.Categories).Add(category);
            }
        }
    }
}