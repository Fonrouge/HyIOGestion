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
        private SqlTransaction _currentTransaction;
        private readonly IApplicationSettings _appSettings;

        public UserRepository(IApplicationSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void SetTransaction(object transaction) => _currentTransaction = (SqlTransaction)transaction;

        // --- MÉTODOS PÚBLICOS ---

        public async Task CreateAsync(Usuario user)
        {
            string query = @"INSERT INTO [User] 
                (Id, Username, [Password], DVH, [Language], Id_Employee, IsDeleted) 
                VALUES 
                (@Id, @Username, @Password, @DVH, @Language, @Id_Employee, @IsDeleted)";

            await ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, user));
        }

        public async Task UpdateAsync(Usuario user)
        {
            string query = @"UPDATE [User] 
                SET Username = @Username, [Password] = @Password, DVH = @DVH, 
                    [Language] = @Language, Id_Employee = @Id_Employee, 
                    IsDeleted = @IsDeleted
                WHERE Id = @Id";

            await ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, user));
        }

        public async Task DeleteAsync(Guid id)
        {
            // Borrado FÍSICO. La BLL usará Update para el borrado lógico.
            string query = "DELETE FROM [User] WHERE Id = @Id";
            await ExecuteNonQueryAsync(query, cmd =>
                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }));
        }

        public async Task<Usuario> GetByIdAsync(Guid id)
        {
            Usuario user = null;
            // Quitamos el filtro IsDeleted = 0 para permitir que la BLL gestione la papelera
            string query = @"SELECT Id, Username, [Password], DVH, [Language], Id_Employee, IsDeleted 
                             FROM [User] WHERE Id = @Id";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }),
                reader => user = Map(reader));

            return user;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            var users = new List<Usuario>();
            string query = "SELECT Id, Username, [Password], DVH, [Language], Id_Employee, IsDeleted FROM [User]";

            await ExecuteReaderAsync(query, null, reader => users.Add(Map(reader)));
            return users;
        }

        public async Task<IEnumerable<Usuario>> GetByUsernameAsync(string username)
        {
            var users = new List<Usuario>();
            string query = @"SELECT Id, Username, [Password], DVH, [Language], Id_Employee, IsDeleted 
                             FROM [User] WHERE Username = @Username";

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.VarChar) { Value = username }),
                reader => users.Add(Map(reader)));

            return users;
        }

        // --- MAPEO Y PARÁMETROS ---

        private void SetParameters(SqlCommand cmd, Usuario user)
        {
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = user.Id });
            cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.VarChar) { Value = user.Username });
            cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.VarChar) { Value = user.Password });

            // Accedemos al .Value del Value Object DVH
            cmd.Parameters.Add(new SqlParameter("@DVH", SqlDbType.VarChar) { Value = (object)user.DVH?.Value ?? DBNull.Value });

            cmd.Parameters.Add(new SqlParameter("@Language", SqlDbType.VarChar)
            {
                Value = string.IsNullOrEmpty(user.Language) ? DBNull.Value : (object)user.Language
            });

            cmd.Parameters.Add(new SqlParameter("@Id_Employee", SqlDbType.UniqueIdentifier) { Value = user.EmployeeId });

            // Nuevos campos de estado
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = user.IsDeleted });
        }

        private Usuario Map(SqlDataReader reader)
        {
            // Usamos Reconstitute para respetar los setters privados y la lógica de dominio
            return Usuario.Reconstitute(
                id: (Guid)reader["Id"],
                username: reader["Username"].ToString(),
                password: reader["Password"].ToString(),
                language: reader["Language"] != DBNull.Value ? reader["Language"].ToString() : null,
                dvh: reader["DVH"]?.ToString(),
                employeeId: (Guid)reader["Id_Employee"],
                isDeleted: (bool)reader["IsDeleted"]
            );
        }

        // --- MOTOR ASÍNCRONO ---

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
    }
}












