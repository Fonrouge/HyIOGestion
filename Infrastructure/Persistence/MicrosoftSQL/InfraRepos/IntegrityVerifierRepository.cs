using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL.Persistence.MicrosoftSQL
{
    /// <summary>
    /// Repositorio encargado de la persistencia de los sellos de integridad vertical (DVV).
    /// </summary>
    public class IntegrityVerifierRepository : IIntegrityVerifierRepository
    {
        private SqlTransaction _currentTransaction;

        /// <summary>
        /// Asigna una transacción activa proveniente de la Unit of Work.
        /// </summary>
        public void SetTransaction(object transaction)
        {
            _currentTransaction = (SqlTransaction)transaction;
        }

        /// <summary>
        /// Obtiene todos los DVH de una tabla específica para recalcular el DVV de forma asíncrona.
        /// </summary>
        public async Task<List<string>> GetVerticalHashesAsync(string tableName, string connectionString)
        {
            List<string> hashes = new List<string>();

            SqlConnection conn = _currentTransaction?.Connection;
            bool isExternalConn = conn != null;

            if (!isExternalConn)
                conn = new SqlConnection(connectionString);

            try
            {
                // NOTA: Como el nombre de la tabla no puede parametrizarse con @, se inyecta directamente.
                // Asegúrate de que tableName provenga de tus appSettings y NUNCA de un input de usuario 
                // para evitar SQL Injection.
                string query = $"SELECT DVH FROM [{tableName}] ORDER BY Id_{tableName}";

                using (var cmd = new SqlCommand(query, conn))
                {
                    if (isExternalConn) cmd.Transaction = _currentTransaction;

                    if (!isExternalConn) await conn.OpenAsync(); // Asíncrono

                    using (var reader = await cmd.ExecuteReaderAsync()) // Asíncrono
                    {
                        while (await reader.ReadAsync()) // Asíncrono
                        {
                            hashes.Add(reader["DVH"].ToString());
                        }
                    }
                }
            }
            finally
            {
                if (!isExternalConn) conn.Dispose();
            }
            return hashes;
        }

        /// <summary>
        /// Actualiza el Dígito Verificador Vertical (DVV) para una tabla.
        /// </summary>
        public async Task UpdateDVVAsync(string tableName, string dvv, string connectionString)
        {
            SqlConnection conn = _currentTransaction?.Connection;
            bool isExternalConn = conn != null;

            if (!isExternalConn)
                conn = new SqlConnection(connectionString);

            try
            {
                string query = "UPDATE Control_Integridad SET DVV = @dvv WHERE Tabla = @tabla";

                using (var cmd = new SqlCommand(query, conn))
                {
                    if (isExternalConn) cmd.Transaction = _currentTransaction;

                    // Reemplazamos AddWithValue por tipos explícitos
                    cmd.Parameters.Add(new SqlParameter("@dvv", SqlDbType.VarChar) { Value = dvv });
                    cmd.Parameters.Add(new SqlParameter("@tabla", SqlDbType.VarChar) { Value = tableName });

                    if (!isExternalConn) await conn.OpenAsync(); // Asíncrono

                    await cmd.ExecuteNonQueryAsync(); // Asíncrono
                }
            }
            finally
            {
                if (!isExternalConn) conn.Dispose();
            }
        }

        /// <summary>
        /// Recupera el sello DVV almacenado en la tabla de control.
        /// </summary>
        public async Task<string> GetStoredDVVAsync(string tableName, string connectionString)
        {
            SqlConnection conn = _currentTransaction?.Connection;
            bool isExternalConn = conn != null;

            if (!isExternalConn)
                conn = new SqlConnection(connectionString);

            try
            {
                string query = "SELECT DVV FROM Control_Integridad WHERE Tabla = @tabla";

                using (var cmd = new SqlCommand(query, conn))
                {
                    if (isExternalConn) cmd.Transaction = _currentTransaction;

                    // Reemplazamos AddWithValue por tipos explícitos
                    cmd.Parameters.Add(new SqlParameter("@tabla", SqlDbType.VarChar) { Value = tableName });

                    if (!isExternalConn) await conn.OpenAsync(); // Asíncrono

                    var result = await cmd.ExecuteScalarAsync(); // Asíncrono

                    return result != null && result != DBNull.Value ? result.ToString() : null;
                }
            }
            finally
            {
                if (!isExternalConn) conn.Dispose();
            }
        }
    }
}