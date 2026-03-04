using Domain.Exceptions.Base;
using Domain.Repositories;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Local
{
    public class ErrorsRepository : IErrorsRepository
    {
        private SqlTransaction _currentTransaction;
        private readonly string _filePath;
        
        private static readonly SemaphoreSlim _fileLock = new SemaphoreSlim(1, 1);

        public ErrorsRepository()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs.json");
        }

        public void SetTransaction(object transaction)
        {
            // Se mantiene el método por contrato.
            _currentTransaction = (SqlTransaction)transaction;
        }

        /// <summary>
        /// Persiste una LogEntry en el archivo Logs.json en formato NDJSON (JSON Lines).
        /// </summary>
        public async Task CreateAsync(ErrorLog entry)
        {
            // Serializamos fuera del lock porque es una operación de CPU, no de I/O
            string json = JsonConvert.SerializeObject(entry);
            string logLine = json + Environment.NewLine;

            // Esperamos asíncronamente nuestro turno para acceder al archivo
            await _fileLock.WaitAsync();

            try
            {
                using (StreamWriter sw = new StreamWriter(_filePath, append: true))
                {
                    await sw.WriteAsync(logLine);
                }
            }
            
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Application",
                    $"Error crítico al escribir en Logs.json: {ex.Message}",
                    System.Diagnostics.EventLogEntryType.Error);
            }
            
            finally
            {
                _fileLock.Release();
            }
        }
    }
}