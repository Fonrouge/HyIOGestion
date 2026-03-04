using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shared.Services.Searching
{
    /// <summary>
    /// Presenter genérico encargado de coordinar operaciones de búsqueda, filtrado, ordenamiento y selección
    /// de entidades del tipo <typeparamref name="T"/> en entornos de presentación desacoplados.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad visualizable y manipulable desde la interfaz.</typeparam>
    public class ListFilterSortProvider : IListFilterSortProvider
    {

        /// <summary>
        /// Aplica un filtro textual sobre los datos disponibles, considerando la columna indicada.
        /// </summary>
        public List<T> Filter<T>(List<T> listToFilter, string searchText, string columnKey, Type t = null) where T : IDto
        {
            if (listToFilter == null) return new List<T>();

            searchText = (searchText ?? "").Trim();
            if (searchText.Length == 0) return listToFilter;

            if (t != null) listToFilter = listToFilter.Where(d => t.IsAssignableFrom(d.GetType())).ToList();

            bool filterAll = string.IsNullOrWhiteSpace(columnKey) || columnKey == "0";
            string needle = searchText.ToLowerInvariant();

            if (filterAll)
            {
                var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                return listToFilter.Where(item =>
                    props.Any(p => (GetStringSafe(p.GetValue(item))).IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();
            }
            else
            {
                // Soporte para rutas anidadas: "Cliente.Nombre"
                var getter = BuildGetter(typeof(T), columnKey);

                if (getter == null)
                {
                    // fallback a "todas"
                    var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    return listToFilter.Where(item =>
                        props.Any(p => (GetStringSafe(p.GetValue(item))).IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0)
                    ).ToList();
                }

                return listToFilter.Where(item =>
                    (GetStringSafe(getter(item))).IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();
            }
        }

        public List<T> Sort<T>(List<T> list, string columnKey, bool ascending) where T : IDto
        {
            if (list == null || list.Count == 0 || string.IsNullOrWhiteSpace(columnKey))
                return list;

            var getter = BuildGetter(typeof(T), columnKey); // Func<object, object>
            if (getter == null) return list;

            // Envolvemos getter para que sea Func<T, object>
            Func<T, object> keySelector = item => getter(item);

            return (ascending
                ? list.OrderBy(keySelector)
                : list.OrderByDescending(keySelector))
                .ToList();
        }


        private static Func<object, object> BuildGetter(Type type, string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;
            var parts = path.Split('.');

            return (obj) =>
            {
                object cur = obj;
                Type curType = type;
                foreach (var part in parts)
                {
                    if (cur == null) return null;
                    var prop = curType.GetProperty(part,
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (prop == null) return null;
                    cur = prop.GetValue(cur);
                    curType = prop.PropertyType;
                }
                return cur;
            };
        }

        private static string GetStringSafe(object v) => v?.ToString() ?? string.Empty;


    }
}