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
    public class CategoryRepository : ICategoryRepository
    {
        private SqlTransaction _currentTransaction;
        private readonly IApplicationSettings _appSettings;

        public CategoryRepository(IApplicationSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void SetTransaction(object transaction)
        {
            _currentTransaction = (SqlTransaction)transaction;
        }

        // --- MÉTODOS PÚBLICOS ---

        public async Task CreateAsync(Category entity)
        {
            // Agregamos el campo DVH a la persistencia
            string query = @"INSERT INTO Categories (Id_Category, Name, Description, DVH) 
                             VALUES (@Id, @Name, @Description, @DVH)";

            await ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public async Task UpdateAsync(Category entity)
        {
            // Agregamos el campo DVH al update
            string query = @"UPDATE Categories 
                             SET Name = @Name, Description = @Description, DVH = @DVH
                             WHERE Id_Category = @Id";

            await ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public async Task DeleteAsync(Guid entityId)
        {
            // Borrado Físico (La BLL decide si llama a este o al Update)
            string deleteRelations = "DELETE FROM ProductsCategories WHERE Id_Category = @Id";
            string deleteCategory = "DELETE FROM Categories WHERE Id_Category = @Id";

            await ExecuteNonQueryAsync(deleteRelations, cmd =>
                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entityId }));

            await ExecuteNonQueryAsync(deleteCategory, cmd =>
                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entityId }));
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            Category category = null;
            string query = "SELECT Id_Category, Name, Description, DVH FROM Categories WHERE Id_Category = @Id";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }),
                reader => category = Map(reader));

            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categories = new List<Category>();
            string query = "SELECT Id_Category, Name, Description, DVH FROM Categories";

            await ExecuteReaderAsync(query, null, reader => categories.Add(Map(reader)));

            return categories;
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            Category category = null;
            string query = "SELECT Id_Category, Name, Description, DVH FROM Categories WHERE Name = @Name";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar) { Value = (object)name ?? DBNull.Value }),
                reader => category = Map(reader));

            return category;
        }

        // --- MOTOR ASÍNCRONO (INFRAESTRUCTURA) ---

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
                // Solo cerramos la conexión si nosotros la creamos (no es una transacción externa)
                if (!isExternalConn)
                {
                    conn.Close();
                    conn.Dispose();
                }
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
                        {
                            mapAction(reader);
                        }
                    }
                }
            }
            finally
            {
                if (!isExternalConn)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        // --- HELPERS ---

        private void SetParameters(SqlCommand cmd, Category entity)
        {
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entity.Id });
            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar) { Value = (object)entity.Name ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar) { Value = (object)entity.Description ?? DBNull.Value });

            // Acceso al Value Object DVH
            cmd.Parameters.Add(new SqlParameter("@DVH", SqlDbType.VarChar) { Value = (object)entity.DVH?.Value ?? DBNull.Value });
        }

        private Category Map(SqlDataReader reader)
        {
            // Usamos el Factory Method Reconstitute para respetar los setters privados
            return Category.Reconstitute(
                id: (Guid)reader["Id_Category"],
                name: reader["Name"]?.ToString() ?? string.Empty,
                description: reader["Description"]?.ToString() ?? string.Empty,
                dvh: reader["DVH"]?.ToString() ?? string.Empty
            );
        }
    }
}