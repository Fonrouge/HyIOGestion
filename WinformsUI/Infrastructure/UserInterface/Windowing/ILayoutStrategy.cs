using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformsUI.Infrastructure.UserInterface.Windowing
{
    public interface ILayoutStrategy
    {
        /// <summary>
        /// Ordena una colección de formularios dentro de un área específica.
        /// </summary>
        /// <param name="clientArea">El rectángulo disponible para dibujar (ej: pnlContenedor.DisplayRectangle)</param>
        /// <param name="forms">Lista de formularios a ordenar</param>
        void Arrange(Rectangle clientArea, IEnumerable<Form> forms);
    }
}
