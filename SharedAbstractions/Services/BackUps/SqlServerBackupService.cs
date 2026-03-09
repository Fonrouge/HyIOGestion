using System;
using System.Data.SqlClient;
using System.IO;
using Shared;


namespace Shared.Services
{
    /// <summary>
    /// Servicio responsable de realizar operaciones de respaldo y restauración sobre una base de datos SQL Server.
    /// Utiliza cifrado simétrico (AES-256-CBC) para proteger la integridad de los archivos generados.
    /// </summary>
    /// <remarks>
    /// Se apoya en comandos T-SQL <c>BACKUP DATABASE</c> y <c>RESTORE DATABASE</c>.
    /// La ruta del archivo temporal se genera automáticamente y el nombre de la base se obtiene desde ApplicationSettings.
    /// </remarks>
    public class SqlBackupService// : IBackupService
    {
        private readonly ApplicationSettings _settings;
        private readonly AesEncryptionService _cipher;
  
  
        /// <summary>
        /// Inicializa el servicio con las configuraciones centralizadas y el sistema de cifrado AES.
        /// </summary>
        /// <param name="settings">Instancia de configuración global (ApplicationSettings).</param>
        /// <param name="cipher">Servicio de cifrado simétrico.</param>
        public SqlBackupService(ApplicationSettings settings, AesEncryptionService cipher)
        {
            _settings = settings;
            _cipher = cipher;
        }
  
        /// <summary>
        /// Realiza un respaldo cifrado de la base de datos y lo guarda en la ruta destino indicada.
        /// </summary>
        /// <param name="targetPath">Ruta del archivo resultante (.bak encriptado).</param>
        public void Backup(string targetPath)
        {
            //   var dbName = _settings.DatabaseNameMainDB;
            var dbName = "asdasd";
            var bakPath = Path.Combine(Path.GetTempPath(), $"{dbName}_{DateTime.Now:yyyyMMddHHmmss}.bak");


            //using (var conn = new SqlConnection(_settings.ConnectionStringMainDB))


            using (var conn = new SqlConnection("asdasd"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"BACKUP DATABASE [{dbName}] TO DISK = N'{bakPath}' WITH INIT";
                    cmd.ExecuteNonQuery();
                }
            }
  
            var bytes = File.ReadAllBytes(bakPath);
            var encrypted = _cipher.Encrypt(bytes);
  
            File.WriteAllBytes(targetPath, encrypted);
            File.Delete(bakPath);
        }
  
        /// <summary>
        /// Restaura la base de datos a partir de un archivo previamente respaldado y cifrado.
        /// </summary>
        /// <param name="sourcePath">Ruta del archivo de backup cifrado (.bak encriptado).</param>
        public void Restore(string sourcePath)
        {
            var dbName = "asd";//_settings.DatabaseNameMainDB;
  
            var encrypted = File.ReadAllBytes(sourcePath);
            var decrypted = _cipher.Decrypt(encrypted);
  
            var bakPath = Path.Combine(Path.GetTempPath(), $"restore_{Guid.NewGuid():N}.bak");
            File.WriteAllBytes(bakPath, decrypted);


            //using (var conn = new SqlConnection(_settings.ConnectionStringMainDB))

            using (var conn = new SqlConnection("asdasd"))
            {
                conn.Open();
  
                var setSingleUser = $"ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                var restore = $"RESTORE DATABASE [{dbName}] FROM DISK = N'{bakPath}' WITH REPLACE";
                var setMultiUser = $"ALTER DATABASE [{dbName}] SET MULTI_USER";
  
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"{setSingleUser}; {restore}; {setMultiUser};";
                    cmd.ExecuteNonQuery();
                }
            }
  
            File.Delete(bakPath);
        }
    }
}