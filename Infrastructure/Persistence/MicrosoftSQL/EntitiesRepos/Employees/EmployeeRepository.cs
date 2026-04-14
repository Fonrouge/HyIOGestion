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
    public class EmployeeRepository : IEmployeeRepository
    {
        private SqlTransaction _currentTransaction;
        private readonly IApplicationSettings _appSettings;

        // Queries optimizadas y sincronizadas con los nombres de columna
        private const string SQL_SELECT_BY_ID = "SELECT Id, FileNumber, FirstName, LastName, NationalId, Email, PhoneNumber, HomeAddress, Active, IsDeleted, DVH FROM {0} WHERE Id = @Id";
        private const string SQL_SELECT_ALL = "SELECT Id, FileNumber, FirstName, LastName, NationalId, Email, PhoneNumber, HomeAddress, Active, IsDeleted, DVH FROM {0} WHERE IsDeleted = 0";
        private const string SQL_SELECT_ALL_DELETED = "SELECT Id, FileNumber, FirstName, LastName, NationalId, Email, PhoneNumber, HomeAddress, Active, IsDeleted, DVH FROM {0} WHERE IsDeleted = 1";
        private const string SQL_SELECT_BY_FILE_NUMBER = "SELECT Id, FileNumber, FirstName, LastName, NationalId, Email, PhoneNumber, HomeAddress, Active, IsDeleted, DVH FROM {0} WHERE FileNumber = @FileNumber";
        private const string SQL_SELECT_BY_NATIONALID = "SELECT Id, FileNumber, FirstName, LastName, NationalId, Email, PhoneNumber, HomeAddress, Active, IsDeleted, DVH FROM {0} WHERE NationalId = @NationalId";

        public EmployeeRepository(IApplicationSettings appSettings) => _appSettings = appSettings;

        public void SetTransaction(object transaction) => _currentTransaction = (SqlTransaction)transaction;

        public Task CreateAsync(Employee entity)
        {
            string query = string.Format(@"INSERT INTO {0} 
                (Id, FileNumber, FirstName, LastName, NationalId, Email, PhoneNumber, HomeAddress, Active, IsDeleted, DVH) 
                VALUES 
                (@Id, @FileNumber, @FirstName, @LastName, @NationalId, @Email, @PhoneNumber, @HomeAddress, @Active, @IsDeleted, @DVH)",
                _appSettings.EmployeeTableName);

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public Task UpdateAsync(Employee entity)
        {
            string query = string.Format(@"UPDATE {0} SET 
                FileNumber = @FileNumber, FirstName = @FirstName, LastName = @LastName, 
                NationalId = @NationalId, Email = @Email, PhoneNumber = @PhoneNumber, 
                HomeAddress = @HomeAddress, Active = @Active, IsDeleted = @IsDeleted, DVH = @DVH 
                WHERE Id = @Id",
                _appSettings.EmployeeTableName);

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, entity));
        }

        public Task DeleteAsync(Guid id)
        {
            // HARD DELETE: Eliminación física del registro
            string query = string.Format("DELETE FROM {0} WHERE Id = @Id", _appSettings.EmployeeTableName);
            return ExecuteNonQueryAsync(query, cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }));
        }

        public async Task<Employee> GetByIdAsync(Guid id)
        {
            Employee employee = null;
            string query = string.Format(SQL_SELECT_BY_ID, _appSettings.EmployeeTableName);

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id }),
                reader => employee = Map(reader));

            return employee;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var employees = new List<Employee>();
            string query = string.Format(SQL_SELECT_ALL, _appSettings.EmployeeTableName);

            await ExecuteReaderAsync(query, null, reader => employees.Add(Map(reader)));
            return employees;
        }

        public async Task<IEnumerable<Employee>> GetAllDeletedAsync()
        {
            var employees = new List<Employee>();
            string query = string.Format(SQL_SELECT_ALL_DELETED, _appSettings.EmployeeTableName);

            await ExecuteReaderAsync(query, null, reader => employees.Add(Map(reader)));
            return employees;
        }

        public async Task<Employee> GetByFileNumberAsync(string fileNumber)
        {
            Employee employee = null;
            string query = string.Format(SQL_SELECT_BY_FILE_NUMBER, _appSettings.EmployeeTableName);

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@FileNumber", SqlDbType.NVarChar) { Value = fileNumber ?? string.Empty }),
                reader => employee = Map(reader));

            return employee;
        }

        public async Task<Employee> GetByNationalIdAsync(string nationalId)
        {
            Employee employee = null;
            string query = string.Format(SQL_SELECT_BY_NATIONALID, _appSettings.EmployeeTableName);

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@NationalId", SqlDbType.NVarChar) { Value = nationalId ?? string.Empty }),
                reader => employee = Map(reader));

            return employee;
        }

        // --- MÉTODOS PRIVADOS ---

        private void SetParameters(SqlCommand cmd, Employee entity)
        {
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entity.Id });
            cmd.Parameters.Add(new SqlParameter("@FileNumber", SqlDbType.NVarChar) { Value = entity.FileNumber?.Value ?? string.Empty });
            cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar) { Value = entity.FirstName?.Value ?? string.Empty });
            cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar) { Value = entity.LastName?.Value ?? string.Empty });
            cmd.Parameters.Add(new SqlParameter("@NationalId", SqlDbType.NVarChar) { Value = entity.NationalId?.Value ?? string.Empty });
            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = entity.Email?.Value ?? string.Empty });
            cmd.Parameters.Add(new SqlParameter("@PhoneNumber", SqlDbType.NVarChar) { Value = entity.PhoneNumber?.Value ?? string.Empty });
            cmd.Parameters.Add(new SqlParameter("@HomeAddress", SqlDbType.NVarChar) { Value = entity.HomeAddress?.Value ?? string.Empty });
            cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit) { Value = entity.Active });
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = entity.IsDeleted });
            cmd.Parameters.Add(new SqlParameter("@DVH", SqlDbType.VarChar)
            {
                Value = (object)entity.DVH?.Value ?? string.Empty
            });
        }
        

        private Employee Map(SqlDataReader reader)
        {
            return Employee.Reconstitute(
                (Guid)reader["Id"],
                reader["FileNumber"].ToString(),
                reader["FirstName"].ToString(),
                reader["LastName"].ToString(),
                reader["NationalId"].ToString(),
                reader["Email"].ToString(),
                reader["PhoneNumber"].ToString(),
                reader["HomeAddress"].ToString(),
                reader["DVH"].ToString(),
                (bool)reader["Active"],
                (bool)reader["IsDeleted"]
            );
        }

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
    }
}