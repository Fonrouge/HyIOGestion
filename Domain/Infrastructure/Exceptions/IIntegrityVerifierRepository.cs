using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    /// <summary>
    /// Define las operaciones de persistencia asíncronas para el sistema de control de integridad vertical (DVV).
    /// </summary>
    public interface IIntegrityVerifierRepository
    {
        /// <summary>
        /// Obtiene la lista completa de Dígitos Verificadores Horizontales (DVH) de una tabla específica.
        /// </summary>
        Task<List<string>> GetVerticalHashesAsync(string tableName, string connectionString, bool hasId = true);

        /// <summary>
        /// Actualiza o inserta el Dígito Verificador Vertical (DVV) para una tabla en la tabla de control.
        /// </summary>
        Task UpdateDVVAsync(string tableName, string dvv, string connectionString);

        /// <summary>
        /// Recupera el Dígito Verificador Vertical (DVV) almacenado actualmente en la base de datos.
        /// </summary>
        Task<string> GetStoredDVVAsync(string tableName, string connectionString);

        void SetTransaction(object transaction);
    }
}