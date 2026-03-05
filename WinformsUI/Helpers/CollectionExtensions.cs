using BLL.LogicLayers;
using SharedAbstractions.ArchitecturalMarkers;
using System.Collections.Generic;
using System.ComponentModel;

namespace WinformsUI.UserControls
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Convierte un IEnumerable en una BindingList para soporte nativo de WinForms.
        /// </summary>
        public static BindingList<T> ToBindingList<T>(this IEnumerable<T> source)
        {
            if (source == null) return new BindingList<T>();

            if (source is IList<T> list)
            {
                return new BindingList<T>(list);
            }

            return new BindingList<T>(new List<T>(source));
        }

    

    }

}

