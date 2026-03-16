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
            // Agregamos el campo DVH a la query de inserción
            string query = @"INSERT INTO [HSecurity].[dbo].[Bitacora] 
                                ([Timestamp], [User], Message, ExceptionType, TableName, StackTrace, BitacoraType, Severity, Success, DVH) 
                            VALUES 
                                (@Timestamp, @User, @Message, @ExceptionType, @TableName, @StackTrace, @BitacoraType, @Severity, @Success, @DVH)";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, logEntry));
        }

        public async Task<IEnumerable<Bitacora>> GetAllAsync()
        {
            var logs = new List<Bitacora>();
            // Incluimos Id y DVH en la lectura
            string query = @"SELECT Id, [Timestamp], [User], Message, ExceptionType, TableName, StackTrace, BitacoraType, Severity, Success, DVH 
                             FROM [HSecurity].[dbo].[Bitacora] 
                             ORDER BY [Timestamp] DESC";

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
            cmd.Parameters.Add(new SqlParameter("@Timestamp", SqlDbType.DateTime2) { Value = logEntry.Timestamp });
            cmd.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar, 256) { Value = (object)logEntry.User ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar, -1) { Value = (object)logEntry.Message ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@ExceptionType", SqlDbType.NVarChar, 500) { Value = (object)logEntry.ExceptionType ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar, 128) { Value = (object)logEntry.TableName ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@StackTrace", SqlDbType.NVarChar, -1) { Value = (object)logEntry.StackTrace ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@BitacoraType", SqlDbType.Int) { Value = (int)logEntry.BitacoraType });
            cmd.Parameters.Add(new SqlParameter("@Severity", SqlDbType.Int) { Value = (int)logEntry.Severity });
            cmd.Parameters.Add(new SqlParameter("@Success", SqlDbType.Bit) { Value = logEntry.Success });

            // CORREGIDO: Acceso al .Value del Value Object DVH
            cmd.Parameters.Add(new SqlParameter("@DVH", SqlDbType.Char, 64) { Value = (object)logEntry.DVH?.Value ?? DBNull.Value });
        }

        private Bitacora Map(SqlDataReader reader)
        {
            // Usamos Reconstitute para instanciar la entidad con sus propiedades privadas
            return Bitacora.Reconstitute(
                id: (int)reader["Id"],
                timestamp: (DateTime)reader["Timestamp"],
                user: reader["User"]?.ToString(),
                message: reader["Message"]?.ToString(),
                type: (int)reader["BitacoraType"],
                severity: (int)reader["Severity"],
                success: (bool)reader["Success"],
                tableName: reader["TableName"]?.ToString(),
                exceptionType: reader["ExceptionType"]?.ToString(),
                stackTrace: reader["StackTrace"]?.ToString(),
                dvh: reader["DVH"]?.ToString()
            );
        }
    }
}