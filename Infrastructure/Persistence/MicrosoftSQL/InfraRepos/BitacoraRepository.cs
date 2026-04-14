using Domain.Exceptions;
using Domain.Exceptions.Base;
using Domain.Infrastructure.Audit;
using Domain.Repositories;
using Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
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
            // Se agregaron SessionId, CorrelationId y HostName a la consulta
            string query = @"INSERT INTO [HSecurity].[dbo].[Bitacora] 
                        (Id, SessionId, CorrelationId, HostName, [Timestamp], [User], Message, ExceptionType, TableName, StackTrace, BitacoraType, Severity, Success, DVH) 
                    VALUES 
                        (@Id, @SessionId, @CorrelationId, @HostName, @Timestamp, @User, @Message, @ExceptionType, @TableName, @StackTrace, @BitacoraType, @Severity, @Success, @DVH)";

            return ExecuteNonQueryAsync(query, cmd => SetParameters(cmd, logEntry));
        }

        public async Task<IEnumerable<Bitacora>> GetAllIntegrityCheckableAsync()
        {
            var logs = new List<Bitacora>();

            // Filtramos por Severity >= 1 e incluimos los nuevos campos
            string query = @"SELECT Id, SessionId, CorrelationId, HostName, [Timestamp], [User], Message, ExceptionType, TableName, StackTrace, BitacoraType, Severity, Success, DVH 
                         FROM [HSecurity].[dbo].[Bitacora] 
                         WHERE Severity >= 1 
                         ORDER BY [Timestamp] DESC";

            await ExecuteReaderAsync(query, null, reader => logs.Add(Map(reader)));

            return logs;
        }

        public async Task<IEnumerable<Bitacora>> GetAllAsync()
        {
            var logs = new List<Bitacora>();

            string query = @"SELECT Id, SessionId, CorrelationId, HostName, [Timestamp], [User], Message, ExceptionType, TableName, StackTrace, BitacoraType, Severity, Success, DVH 
                             FROM [HSecurity].[dbo].[Bitacora] 
                             ORDER BY [Timestamp] DESC";

            await ExecuteReaderAsync(query, null, reader => logs.Add(Map(reader)));

            return logs;
        }

        // --- MÉTODOS PRIVADOS DE INFRAESTRUCTURA ---

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
            catch (Exception ex)
            {
                // Loguear error de infraestructura aquí si es necesario
                Debugger.Break();
                throw;
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
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = logEntry.Id });
            cmd.Parameters.Add(new SqlParameter("@SessionId", SqlDbType.UniqueIdentifier) { Value = logEntry.SessionId });
            cmd.Parameters.Add(new SqlParameter("@CorrelationId", SqlDbType.UniqueIdentifier) { Value = logEntry.CorrelationId });
            cmd.Parameters.Add(new SqlParameter("@HostName", SqlDbType.NVarChar, 256) { Value = (object)logEntry.HostName ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Timestamp", SqlDbType.DateTime2) { Value = logEntry.Timestamp });
            cmd.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar, 256) { Value = (object)logEntry.User ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar, -1) { Value = (object)logEntry.Message ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@ExceptionType", SqlDbType.NVarChar, 500) { Value = (object)logEntry.ExceptionType ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar, 128) { Value = (object)logEntry.TableName ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@StackTrace", SqlDbType.NVarChar, -1) { Value = (object)logEntry.StackTrace ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@BitacoraType", SqlDbType.Int) { Value = (int)logEntry.BitacoraType });
            cmd.Parameters.Add(new SqlParameter("@Severity", SqlDbType.Int) { Value = (int)logEntry.Severity });
            cmd.Parameters.Add(new SqlParameter("@Success", SqlDbType.Bit) { Value = logEntry.Success });
            cmd.Parameters.Add(new SqlParameter("@DVH", SqlDbType.Char, 64) { Value = (object)logEntry.DVH?.Value ?? DBNull.Value });
        }

        private Bitacora Map(SqlDataReader reader)
        {
            // IMPORTANTE: El orden de los argumentos debe coincidir con Bitacora.Reconstitute
            return Bitacora.Reconstitute(
                id: (Guid)reader["Id"],
                timestamp: (DateTime)reader["Timestamp"],
                user: reader["User"]?.ToString(),
                message: reader["Message"]?.ToString(),
                sessionId: (Guid)reader["SessionId"],
                correlationId: (Guid)reader["CorrelationId"],
                type: (int)reader["BitacoraType"],
                severity: (int)reader["Severity"],
                hostName: reader["HostName"]?.ToString(),
                success: (bool)reader["Success"],
                tableName: reader["TableName"]?.ToString(),
                exceptionType: reader["ExceptionType"]?.ToString(),
                stackTrace: reader["StackTrace"]?.ToString(),
                dvh: reader["DVH"]?.ToString()
            );
        }
    }
}