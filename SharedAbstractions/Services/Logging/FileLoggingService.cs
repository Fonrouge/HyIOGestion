using System;
using System.IO;

namespace Shared.Services
{
    /// <summary>
    /// Servicio de registro que escribe eventos en archivos de texto plano agrupados por fecha.
    /// </summary>
    /// <remarks>
    /// Las entradas se guardan en la carpeta <c>Logs</c> del directorio base, o en la ruta personalizada si se especifica.
    /// Cada archivo de log se nombra como <c>log_yyyyMMdd.log</c>.
    /// Compatible con seguimiento en <c>DebugOutput</c> durante desarrollo.
    /// </remarks>
    public class FileLoggingService // : ILoggingService
    {
        private readonly string _logPath;

        /// <summary>
        /// Inicializa el servicio de logging con una ruta opcional para los archivos.
        /// Si no se especifica, se usa <c>AppDomain.CurrentDomain.BaseDirectory\Logs</c>.
        /// </summary>
        /// <param name="logDirectory">Ruta personalizada donde se almacenarán los archivos de log.</param>
        public FileLoggingService(string logDirectory = null)
        {
            if (logDirectory == null)
                logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            _logPath = Path.Combine(logDirectory, $"log_{DateTime.Now:yyyyMMdd}.log");
        }

        /// <inheritdoc />
        public void Info(string message) => Write("INFO", message);

        /// <inheritdoc />
        public void Warn(string message) => Write("WARN", message);

        /// <inheritdoc />
        public void Error(string message, Exception ex = null)
        {
            var msg = $"{message}{(ex != null ? $" | EX: {ex.Message}" : "")}";
            Write("ERROR", msg);
        }

        /// <summary>
        /// Escribe una entrada en el archivo de log con nivel y mensaje formateados.
        /// </summary>
        /// <param name="level">Nivel de severidad: INFO, WARN o ERROR.</param>
        /// <param name="message">Mensaje de log a registrar.</param>
        private void Write(string level, string message)
        {
            var entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
            File.AppendAllText(_logPath, entry + Environment.NewLine);

            #if DEBUG
            System.Diagnostics.Debug.WriteLine(entry);
            #endif
        }
    }
}