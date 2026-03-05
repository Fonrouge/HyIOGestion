using Domain.BaseContracts;
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

        private const string SQL_INSERT = @"INSERT INTO {0} (Id_Employee, DNI, Nombre, Apellido, Email, Telefono, Activo, DVH, IsDeleted, FileNumber) 
                                            VALUES (@Id_Employee, @DNI, @Nombre, @Apellido, @Email, @Telefono, @Activo, @DVH, @IsDeleted, @FileNumber)";
      
        private const string SQL_SOFT_DELETE = "UPDATE {0} SET IsDeleted = 1 WHERE Id_Employee = @Id";

        private const string SQL_HARD_DELETE = "DELETE FROM {0} WHERE Id_Employee = @Id";

        private const string SQL_SELECT_BY_ID = "SELECT * FROM {0} WHERE Id_Employee = @Id AND IsDeleted = 0";

        private const string SQL_SELECT_ALL = "SELECT * FROM {0} WHERE IsDeleted = 0";

        private const string SQL_SELECT_BY_FILE_NUMBER = "SELECT * FROM {0} WHERE FileNumber = @FileNumber AND IsDeleted = 0";
        
        private const string SQL_UPDATE = @"UPDATE {0} SET DNI = @DNI, Nombre = @Nombre, Apellido = @Apellido, 
                                            Email = @Email, Telefono = @Telefono, Activo = @Activo, DVH = @DVH, 
                                            IsDeleted = @IsDeleted, FileNumber = @FileNumber 
                                            WHERE Id_Employee = @Id_Employee";
        
        private const string SQL_SELECT_BY_TAXID = "SELECT * FROM {0} WHERE DNI = @DNI AND IsDeleted = 0";


        public EmployeeRepository(IApplicationSettings appSettings) => _appSettings = appSettings;

        public void SetTransaction(object transaction) => _currentTransaction = (SqlTransaction)transaction;

        public Task CreateAsync(Employee employee)
        {
            string query = string.Format(SQL_INSERT, _appSettings.EmployeeTableName);
            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, employee));
        }

        public Task UpdateAsync(Employee employee)
        {
            string query = string.Format(SQL_UPDATE, _appSettings.EmployeeTableName);
            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, employee));
        }

        public async Task DeleteAsync(Guid entityId)
        {
            var entity = await GetByIdAsync(entityId);
            if (entity == null) return;

            string query;
            if (entity is ISoftDeletable)
                query = string.Format(SQL_SOFT_DELETE, _appSettings.EmployeeTableName);
            else
                query = string.Format(SQL_HARD_DELETE, _appSettings.EmployeeTableName);

            await ExecuteNonQueryAsync(query, cmd => cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = entityId }));
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

        public async Task<Employee> GetByFileNumberAsync(string fileNumber)
        {
            Employee employee = null;
            string query = string.Format(SQL_SELECT_BY_FILE_NUMBER, _appSettings.EmployeeTableName);

            await ExecuteReaderAsync(query,
                cmd => cmd.Parameters.Add(new SqlParameter("@FileNumber", SqlDbType.VarChar) { Value = fileNumber }),
                reader => employee = Map(reader));

            return employee;
        }

        public async Task<Employee> GetByTaxIdAsync(string taxId)
        {
            Employee employee = null;

            // Recordá tener declarada esta constante arriba en tu clase, algo como:
            // private const string SQL_SELECT_BY_TAXID = "SELECT * FROM {0} WHERE NationalId = @TaxId AND IsDeleted = 0";
            string query = string.Format(SQL_SELECT_BY_TAXID, _appSettings.EmployeeTableName);

            await ExecuteReaderAsync(query,
                // Nota: Usé VarChar para mantener el patrón de tu GetByFileNumberAsync, 
                // pero si en tu DB es NVARCHAR, cambialo a SqlDbType.NVarChar
                cmd => cmd.Parameters.Add(new SqlParameter("@TaxId", SqlDbType.VarChar) { Value = taxId }),
                reader => employee = Map(reader));

            return employee;
        }

        // --- MÉTODOS PRIVADOS DE INFRAESTRUCTURA ---

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
            finally { if (!isExternalConn) conn.Dispose(); }
        }

        private async Task ExecuteReaderAsync(string query, Action<SqlCommand> parameterSetter, Action<SqlDataReader> mapAction)
        {
            SqlConnection conn;
            SqlTransaction trans = _currentTransaction;

            if (trans != null && trans.Connection.ConnectionString == _appSettings.EntitiesConnection)
            {
                conn = trans.Connection;
            }
            else
            {
                conn = new SqlConnection(_appSettings.EntitiesConnection);
                trans = null;
            }

            bool managedExternally = (trans != null);

            try
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    if (managedExternally) cmd.Transaction = trans;
                    parameterSetter?.Invoke(cmd);

                    if (conn.State != ConnectionState.Open) await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync()) mapAction(reader);
                    }
                }
            }
            finally
            {
                if (!managedExternally) conn.Dispose();
            }
        }

        private void SetParameters(SqlCommand cmd, Employee employee)
        {
            cmd.Parameters.Add(new SqlParameter("@Id_Employee", SqlDbType.UniqueIdentifier) { Value = employee.Id });
            cmd.Parameters.Add(new SqlParameter("@DNI", SqlDbType.VarChar) { Value = (object)employee.NationalId ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.VarChar) { Value = (object)employee.FirstName ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Apellido", SqlDbType.VarChar) { Value = (object)employee.LastName ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar) { Value = (object)employee.Email ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Telefono", SqlDbType.VarChar) { Value = (object)employee.PhoneNumber ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Activo", SqlDbType.Bit) { Value = employee.Active });
            cmd.Parameters.Add(new SqlParameter("@DVH", SqlDbType.VarChar) { Value = (object)employee.DVH ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = employee.IsDeleted });
            cmd.Parameters.Add(new SqlParameter("@FileNumber", SqlDbType.VarChar) { Value = (object)employee.FileNumber ?? DBNull.Value });
        }

        private Employee Map(SqlDataReader reader)
        {
            bool HasColumn(SqlDataReader r, string columnName)
            {
                for (int i = 0; i < r.FieldCount; i++)
                {
                    if (r.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase)) return true;
                }
                return false;
            }

            Guid id = (Guid)reader["Id_Employee"];
            string nationalId = reader["DNI"] != DBNull.Value ? reader["DNI"].ToString() : null;
            string firstName = reader["Nombre"] != DBNull.Value ? reader["Nombre"].ToString() : null;
            string lastName = reader["Apellido"] != DBNull.Value ? reader["Apellido"].ToString() : null;
            string email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null;
            string phone = reader["Telefono"] != DBNull.Value ? reader["Telefono"].ToString() : null;
            bool active = (bool)reader["Activo"];
            bool isDeleted = (bool)reader["IsDeleted"];

            string dvh = HasColumn(reader, "DVH") && reader["DVH"] != DBNull.Value ? reader["DVH"].ToString() : null;
            string fileNumber = HasColumn(reader, "NroLegajo") && reader["NroLegajo"] != DBNull.Value ? reader["NroLegajo"].ToString() : null;
            string homeAddress = HasColumn(reader, "Direccion") && reader["Direccion"] != DBNull.Value ? reader["Direccion"].ToString() : null;

            return Employee.Reconstitute(id, fileNumber, firstName, lastName, nationalId, email, phone, homeAddress, dvh, active, isDeleted);
        }
    }
}