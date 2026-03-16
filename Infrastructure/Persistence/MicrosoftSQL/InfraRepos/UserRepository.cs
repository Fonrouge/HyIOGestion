using Domain.Entities.Permisos.Concrete;
using Domain.Repositories;
using Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL.Persistence.MicrosoftSQL
{
    public class UserRepository : IUserRepository
    {
        private readonly IApplicationSettings _appSettings;

        public UserRepository(IApplicationSettings appSettings)
        {
            _appSettings = appSettings;
        }

        // Ignoramos la transacción del UnitOfWork (User vive en HSecurity)
        public void SetTransaction(object transaction)
        {
            // No se usa nunca
        }

        public async Task CreateAsync(Usuario user)
        {
            string query = @"
                INSERT INTO [dbo].[User]
                    (Id, Username, [Password], DVH, [Language], Id_Employee, IsDeleted)
                VALUES
                    (@Id, @Username, @Password, @DVH, @Language, @Id_Employee, @IsDeleted)";

            await ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, user));
        }

        public async Task UpdateAsync(Usuario user)
        {
            string query = @"
                UPDATE [dbo].[User]
                SET Username = @Username,
                    [Password] = @Password,
                    DVH = @DVH,
                    [Language] = @Language,
                    Id_Employee = @Id_Employee,
                    IsDeleted = @IsDeleted
                WHERE Id = @Id";

            await ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, user));
        }

        public async Task DeleteAsync(Guid id)
        {
            string query = "DELETE FROM [dbo].[User] WHERE Id = @Id";
            await ExecuteNonQueryAsync(query, cmd =>
                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }));
        }

        public async Task<Usuario> GetByIdAsync(Guid id)
        {
            Usuario user = null;
            string query = @"
                SELECT Id, Username, [Password], DVH, [Language], Id_Employee, IsDeleted
                FROM [dbo].[User]
                WHERE Id = @Id";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }),
                reader => user = Map(reader));

            return user;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            var users = new List<Usuario>();
            string query = @"
                SELECT Id, Username, [Password], DVH, [Language], Id_Employee, IsDeleted
                FROM [dbo].[User]";

            await ExecuteReaderAsync(query, null, reader => users.Add(Map(reader)));
            return users;
        }

        public async Task<IEnumerable<Usuario>> GetByUsernameAsync(string username)
        {
            var users = new List<Usuario>();
            string query = @"
                SELECT Id, Username, [Password], DVH, [Language], Id_Employee, IsDeleted
                FROM [dbo].[User]
                WHERE Username = @Username";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar, 128) { Value = username }),
                reader => users.Add(Map(reader)));

            return users;
        }

        // ====================== MÉTODOS PRIVADOS ======================

        private async Task ExecuteNonQueryAsync(string query, Action<SqlCommand> parameterSetter)
        {
            using (var conn = new SqlConnection(_appSettings.SecurityConnection))
            {
                await conn.OpenAsync();

                using (var cmd = new SqlCommand(query, conn))
                {
                    parameterSetter?.Invoke(cmd);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task ExecuteReaderAsync(string query, Action<SqlCommand> parameterSetter, Action<SqlDataReader> mapAction)
        {
            using (var conn = new SqlConnection(_appSettings.SecurityConnection))
            {
                await conn.OpenAsync();

                using (var cmd = new SqlCommand(query, conn))
                {
                    parameterSetter?.Invoke(cmd);


                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                            mapAction(reader);
                    }
                }
            }
        }

        private void SetParameters(SqlCommand cmd, Usuario user)
        {
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = user.Id });
            cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar, 128) { Value = user.Username });
            cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar, 256) { Value = user.Password });
            cmd.Parameters.Add(new SqlParameter("@DVH", SqlDbType.Char, 64) { Value = (object)user.DVH ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Language", SqlDbType.NVarChar, 10) { Value = (object)user.Language ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Id_Employee", SqlDbType.UniqueIdentifier) { Value = user.EmployeeId });
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = user.IsDeleted });
        }

        private Usuario Map(SqlDataReader reader)
        {
            return Usuario.Reconstitute
            (
                id: (Guid)reader["Id"],
                username: reader["Username"].ToString(),
                password: reader["Password"].ToString(),
                language: reader["Language"] != DBNull.Value ? reader["Language"].ToString() : null,
                employeeId: (Guid)reader["Id_Employee"],
                dvh: reader["DVH"] != DBNull.Value ? reader["DVH"].ToString() : string.Empty,
                isDeleted: (bool)reader["IsDeleted"]
            );
        }
    }
}