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
            // 1. Insertar el Producto (Agregado DVH)
            string query = @"
                INSERT INTO Products 
                    (Id_, Name, Description, Price, Stock, IsActive, CreatedAt, IsDeleted, DVH)
                VALUES 
                    (@Id, @Name, @Description, @Price, @Stock, @IsActive, @CreatedAt, @IsDeleted, @DVH)";

            await ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));

            // 2. Insertar las relaciones
            await SyncCategories(entity.Id, entity.Categories);
        }
        public async Task UpdateAsync(Product entity)
        {
            // 1. Actualizar el Producto (Agregado DVH e IsDeleted)
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

            // 2. Sincronizar categorías (Borrado y re-inserción simple para consistencia)
            string deleteRelations = "DELETE FROM ProductsCategories WHERE Id = @Id";
            await ExecuteNonQueryAsync(deleteRelations, cmd =>
                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entity.Id }));

            await SyncCategories(entity.Id, entity.Categories);
        }

        /// <summary>
        /// Borrado FÍSICO. La BLL decidirá si llama a este o al Update para Soft Delete.
        /// </summary>
        public async Task DeleteAsync(Guid entityId)
        {
            string query = "DELETE FROM Products WHERE Id = @Id";

            await ExecuteNonQueryAsync(query, cmd =>
                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entityId }));
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            Product product = null;
            // Se quita filtro de IsDeleted para permitir "Recycle Bin"
            string query = @"
                SELECT p.Id, p.Name, p.Description, p.Price, p.Stock, p.IsActive, p.CreatedAt, p.IsDeleted, p.DVH,
                       c.Id, c.Name as CategoryName, c.Description as CategoryDesc
                FROM Products p
                LEFT JOIN ProductsCategories pc ON p.Id = pc.Id_Product
                LEFT JOIN Categories c ON pc.Id_Category = c.Id
                WHERE p.Id = @Id";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }),
                reader =>
                {
                    if (product == null) product = MapProduct(reader);
                    MapCategoryToProduct(reader, product);
                });

            return product;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var productDictionary = new Dictionary<Guid, Product>();
            string query = @"
                SELECT p.Id, p.Name, p.Description, p.Price, p.Stock, p.IsActive, p.CreatedAt, p.IsDeleted, p.DVH,
                       c.Id, c.Name as CategoryName, c.Description as CategoryDesc
                FROM Products p
                LEFT JOIN ProductsCategories pc ON p.Id = pc.Id_Product
                LEFT JOIN Categories c ON pc.Id_Category = c.Id
                ORDER BY p.Name";

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

            // Eliminamos 'AND p.IsDeleted = 0' para permitir que la BLL gestione la papelera.
            // Aseguramos que traemos la columna DVH para la integridad horizontal.
            string query = @"
                            SELECT p.Id, p.Name, p.Description, p.Price, p.Stock, p.IsActive, p.CreatedAt, p.IsDeleted, p.DVH,
                                   c.Id, c.Name as CategoryName, c.Description as CategoryDesc
                            FROM Products p
                            LEFT JOIN ProductsCategories pc ON p.Id = pc.Id_Product
                            LEFT JOIN Categories c ON pc.Id_Category = c.Id
                            WHERE p.Name = @Name";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar) { Value = name ?? (object)DBNull.Value }),
                reader =>
                {
                    // Si es la primera fila del set de resultados, hidratamos la raíz del agregado (Product)
                    if (product == null)
                        product = MapProduct(reader);

                    // En cada iteración (una por cada categoría vinculada), agregamos el hijo a la colección
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
            cmd.Parameters.Add(new SqlParameter("@DVH", SqlDbType.VarChar) { Value = (object)entity.DVH?.Value ?? DBNull.Value });
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
            // CORREGIDO: Se incluyen los 10 parámetros requeridos por Reconstitute
            return Product.Reconstitute(
                id: (Guid)reader["Id"],
                rawName: reader["Name"].ToString(),
                rawDescription: reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty,
                rawPrice: Convert.ToDecimal(reader["Price"]),
                rawStock: Convert.ToDecimal(reader["Stock"]),
                categories: new List<Category>(), // Se llena luego en MapCategoryToProduct
                active: (bool)reader["IsActive"],
                createdAt: (DateTime)reader["CreatedAt"],
                isDeleted: (bool)reader["IsDeleted"],
                dvh: reader["DVH"] != DBNull.Value ? reader["DVH"].ToString() : string.Empty
            );
        }

        private void MapCategoryToProduct(SqlDataReader reader, Product product)
        {
            if (reader["Id"] != DBNull.Value)
            {
                var category = Category.Create
                (
                    name: reader["CategoryName"].ToString(),
                    description: reader["CategoryDesc"] != DBNull.Value ? reader["CategoryDesc"].ToString() : string.Empty
                );

                var list = (List<Category>)product.Categories;
                if (!list.Any(c => c.Id == category.Id))
                {
                    list.Add(category);
                }
            }
        }
    }
}