using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;

namespace Shared.Services.Searching
{
    /// <summary>
    /// Define el contrato para presenters encargados de coordinar búsquedas,
    /// filtrado, ordenamiento y notificación de cambios en la presentación de datos.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad manipulada por el presenter.</typeparam>
    public interface IListFilterSortProvider
    {

        /// <summary>
        /// Aplica un filtro textual sobre los datos disponibles, considerando la columna indicada.
        /// </summary>
        /// <param name="searchText">Término de búsqueda (puede estar vacío).</param>
        /// <param name="columnName">Nombre de columna sobre la cual filtrar ("General" aplica sobre todas).</param>
        List<T> Filter<T>(List<T> list, string searchText, string columnName, Type type = null) where T : IDto;

        /// <summary>
        /// Ordena los datos actuales según la clave proporcionada.
        /// </summary>
        /// <param name="list">Lista de entidades a ordenar.</param>
        /// <param name="columnName">Nombre de columna por la cual ordenar.</param>
        /// <param name="ascending">Dirección del ordenamiento.</param>
        List<T> Sort<T>(List<T> list, string columnName, bool ascending) where T : IDto;

        // Eventos opcionales para notificar cambios en la vista
        // event Action<IEnumerable<T>> DataChanged;
        // event Action<T> SelectionChanged;


    }
}