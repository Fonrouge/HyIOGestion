using Domain.Entities.Permisos.Concrete;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Shared;
using Domain.Repositories;
using System.Data;
using System.Threading.Tasks;

namespace DAL.Persistence.MicrosoftSQL
{
    public class UserRepository : IUserRepository
    {
        private SqlTransaction _currentTransaction;
        private readonly IApplicationSettings _appSettings;

        public UserRepository(IApplicationSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void SetTransaction(object transaction)
        {
            _currentTransaction = (SqlTransaction)transaction;
        }

        public Task Create(Usuario user)
        {
            string query = @"INSERT INTO [User] (Id_User, Username, [Password], DVH, [Language], Id_Employee) 
                             VALUES (@Id, @Username, @Password, @DVH, @Language, @Id_Employee)";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, user));
        }

        public Task Update(Usuario user)
        {
            string query = @"UPDATE [User] SET Username = @Username, [Password] = @Password, 
                             DVH = @DVH, [Language] = @Language, Id_Employee = @Id_Employee 
                             WHERE Id_User = @Id";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, user));
        }

        public Task Delete(Guid id)
        {
            string query = "DELETE FROM [User] WHERE Id_User = @Id";
            return ExecuteNonQueryAsync(query, cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }));
        }

        public async Task<Usuario> GetById(Guid id)
        {
            Usuario user = null;
            string query = "SELECT Id_User, Username, [Password], DVH, [Language], Id_Employee FROM [User] WHERE Id_User = @Id";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }),
                reader => user = Map(reader)); // Si encuentra uno, lo asigna

            return user;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            var users = new List<Usuario>();
            string query = "SELECT Id_User, Username, [Password], DVH, [Language], Id_Employee FROM [User]";

            // Como no hay parámetros, pasamos null. Por cada fila, añade a la lista.
            await ExecuteReaderAsync(query, null, reader => users.Add(Map(reader)));

            return users;
        }

        public async Task<IEnumerable<Usuario>> GetByUsernameAsync(string username)
        {
            var users = new List<Usuario>();
            string query = "SELECT Id_User, Username, [Password], DVH, [Language], Id_Employee FROM [User] WHERE Username = @Username";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.VarChar) { Value = username }),
                reader => users.Add(Map(reader)));

            return users;
        }

        // --- MÉTODOS PRIVADOS DE INFRAESTRUCTURA (EL MOTOR) ---

        private async Task ExecuteNonQueryAsync(string query, Action<SqlCommand> parameterSetter)
        {
            SqlConnection conn = _currentTransaction?.Connection;
            bool isExternalConn = conn != null;

            if (!isExternalConn) conn = new SqlConnection(_appSettings.SecurityConnection);

            try
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    if (isExternalConn) cmd.Transaction = _currentTransaction;

                    parameterSetter?.Invoke(cmd);

                    if (!isExternalConn) await conn.OpenAsync(); // Asíncrono

                    await cmd.ExecuteNonQueryAsync(); // Asíncrono
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

            if (!isExternalConn) conn = new SqlConnection(_appSettings.SecurityConnection);

            try
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    if (isExternalConn) cmd.Transaction = _currentTransaction;

                    parameterSetter?.Invoke(cmd);

                    if (!isExternalConn) await conn.OpenAsync(); // Asíncrono

                    using (var reader = await cmd.ExecuteReaderAsync()) // Asíncrono
                    {
                        while (await reader.ReadAsync()) // Asíncrono
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

        private void SetParameters(SqlCommand cmd, Usuario user)
        {
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = user.Id });
            cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.VarChar) { Value = user.Username });
            cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.VarChar) { Value = user.Password });
            cmd.Parameters.Add(new SqlParameter("@DVH", SqlDbType.VarChar) { Value = user.DVH });

            object languageValue = string.IsNullOrEmpty(user.Language) ? DBNull.Value : (object)user.Language;
            cmd.Parameters.Add(new SqlParameter("@Language", SqlDbType.VarChar) { Value = languageValue });

            cmd.Parameters.Add(new SqlParameter("@Id_Employee", SqlDbType.UniqueIdentifier) { Value = user.EmployeeId });
        }

        private Usuario Map(SqlDataReader reader)
        {
            return new Usuario
            {
                Id = (Guid)reader["Id_User"],
                Username = reader["Username"].ToString(),
                Password = reader["Password"].ToString(),
                DVH = reader["DVH"].ToString(),
                Language = reader["Language"] != DBNull.Value ? reader["Language"].ToString() : null,
                EmployeeId = (Guid)reader["Id_Employee"]
            };
        }
    }
}