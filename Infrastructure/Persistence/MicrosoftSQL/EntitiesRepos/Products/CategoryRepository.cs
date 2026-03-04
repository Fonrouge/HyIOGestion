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

        public Task Create(Category entity)
        {
            string query = @"INSERT INTO Categories (Id_Category, Name, Description) 
                             VALUES (@Id, @Name, @Description)";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public Task Update(Category entity)
        {
            string query = @"UPDATE Categories 
                             SET Name = @Name, Description = @Description 
                             WHERE Id_Category = @Id";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public Task Delete(Guid entityId)
        {
            string query = "DELETE FROM Categories WHERE Id_Category = @Id";

            return ExecuteNonQueryAsync(query, cmd =>
                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entityId }));
        }

        public async Task<Category> GetById(Guid id)
        {
            Category category = null;
            string query = "SELECT Id_Category, Name, Description FROM Categories WHERE Id_Category = @Id";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }),
                reader => category = Map(reader));

            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categories = new List<Category>();
            string query = "SELECT Id_Category, Name, Description FROM Categories";

            await ExecuteReaderAsync(query, null, reader => categories.Add(Map(reader)));

            return categories;
        }

        // --- MÉTODOS PRIVADOS DE INFRAESTRUCTURA (EL MOTOR) ---

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
                        {
                            mapAction(reader);
                        }
                    }
                }
            }
            finally
            {
                if (!isExternalConn) conn.Dispose();
            }
        }

        // --- MAPEO Y PARÁMETROS ---

        private void SetParameters(SqlCommand cmd, Category entity)
        {
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entity.Id });

            // Asumimos VarChar para los textos
            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar) { Value = (object)entity.Name ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar) { Value = (object)entity.Description ?? DBNull.Value });
        }

        private Category Map(SqlDataReader reader)
        {
            return new Category
            {
                Id = (Guid)reader["Id_Category"],
                Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : string.Empty,
                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty
            };
        }
    }
}