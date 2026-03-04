using Domain.Exceptions;
using Domain.Exceptions.Base;
using Domain.Infrastructure.Audit;
using Domain.Repositories;
using Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL.Persistence.MicrosoftSQL
{
    public class BitacoraRepository : IBitacoraRepository
    {
        private SqlTransaction _currentTransaction;
        private readonly IApplicationSettings _appSettings;

        public BitacoraRepository(IApplicationSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void SetTransaction(object transaction)
        {
            _currentTransaction = (SqlTransaction)transaction;
        }

        public Task CreateAsync(Bitacora logEntry)
        {
            // Usamos corchetes [] para User y Timestamp por si el motor de SQL los toma como palabras reservadas
            string query = @"INSERT INTO Bitacora 
                             ([Timestamp], [User], Message, ExceptionType, TableName, StackTrace, BitacoraType, Severity, Success) 
                             VALUES 
                             (@Timestamp, @User, @Message, @ExceptionType, @TableName, @StackTrace, @BitacoraType, @Severity, @Success)";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, logEntry));
        }

        public async Task<IEnumerable<Bitacora>> GetAllAsync()
        {
            var logs = new List<Bitacora>();
            string query = @"SELECT [Timestamp], [User], Message, ExceptionType, TableName, StackTrace, BitacoraType, Severity, Success 
                             FROM Bitacora";

            await ExecuteReaderAsync(query, null, reader => logs.Add(Map(reader)));

            return logs;
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

            if (!isExternalConn) conn = new SqlConnection(_appSettings.SecurityConnection);

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

        private void SetParameters(SqlCommand cmd, Bitacora logEntry)
        {
            // Mapeo explícito de tipos para optimizar el rendimiento en SQL Server
            cmd.Parameters.Add(new SqlParameter("@Timestamp", SqlDbType.DateTime2) { Value = logEntry.Timestamp });
            cmd.Parameters.Add(new SqlParameter("@User", SqlDbType.VarChar) { Value = (object)logEntry.User ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.VarChar) { Value = (object)logEntry.Message ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@ExceptionType", SqlDbType.VarChar) { Value = (object)logEntry.ExceptionType ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@TableName", SqlDbType.VarChar) { Value = (object)logEntry.TableName ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@StackTrace", SqlDbType.VarChar) { Value = (object)logEntry.StackTrace ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@BitacoraType", SqlDbType.Int) { Value = (int)logEntry.BitacoraType });
            cmd.Parameters.Add(new SqlParameter("@Severity", SqlDbType.Int) { Value = (int)logEntry.Severity });
            cmd.Parameters.Add(new SqlParameter("@Success", SqlDbType.Bit) { Value = logEntry.Success });
        }

        private Bitacora Map(SqlDataReader reader)
        {
            return new Bitacora
            {
                Timestamp = (DateTime)reader["Timestamp"],
                User = reader["User"] != DBNull.Value ? reader["User"].ToString() : null,
                Message = reader["Message"] != DBNull.Value ? reader["Message"].ToString() : null,
                ExceptionType = reader["ExceptionType"] != DBNull.Value ? reader["ExceptionType"].ToString() : null,
                TableName = reader["TableName"] != DBNull.Value ? reader["TableName"].ToString() : null,
                StackTrace = reader["StackTrace"] != DBNull.Value ? reader["StackTrace"].ToString() : null,

                // Mapeo seguro de enteros a Enums
                BitacoraType = (BitacoraTypeEnum)Convert.ToInt32(reader["BitacoraType"]),
                Severity = (SeverityEnum)Convert.ToInt32(reader["Severity"]),

                Success = (bool)reader["Success"]
            };
        }
    }
}