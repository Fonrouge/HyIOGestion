using System;
using System.Linq;
using System.Windows.Forms;

namespace Winforms.Theme
{
    /// <summary>
    /// Aplica un formato consistente al <see cref="DataGridView"/> principal:
    /// bordes, tamaños de encabezado/filas, ocultación de row headers y
    /// ajuste automático del ancho de columnas para evitar “gris” sobrante
    /// cuando el contenido no completa el ancho disponible.
    /// </summary>
    public static class GiveMainDataGridViewFormat
    {
        /// <summary>
        /// Configura el <paramref name="dgv"/> con el estilo estándar de la UI.
        /// Esta operación está pensada para formularios/columnas fijas (one-shot).
        /// </summary>
        /// <param name="dgv">Instancia del grid a formatear.</param>
        public static void Execute(DataGridView dgv)
        {
            if (dgv == null) return;

            // Reduce parpadeos en scroll/redraw (prop interna)
            TryEnableDoubleBuffer(dgv);

            dgv.SuspendLayout();

            try
            {
                dgv.AutoGenerateColumns = true;
                dgv.AllowUserToAddRows = false;
                dgv.RowHeadersVisible = false;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.MultiSelect = true;
                dgv.ReadOnly = true;
                dgv.ScrollBars = ScrollBars.None;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells; // o None
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dgv.AllowUserToResizeColumns = true;
                dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
                dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                // --- Estilo visual base ---
                dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

                // dgv.EnableHeadersVisualStyles = false; // respeta colores/tema propios
                dgv.RowHeadersVisible = false;

                // --- Alturas estándar ---
                dgv.ColumnHeadersHeight = 30;
                dgv.RowTemplate.Height = 40;

                // Ajuste inicial para filas existentes (si las hubiera)
                foreach (DataGridViewRow row in dgv.Rows)
                    row.Height = dgv.RowTemplate.Height;

                // Asegura altura consistente para filas que se agreguen en runtime
                dgv.RowsAdded += (s, e) =>
                {
                    for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
                        dgv.Rows[i].Height = dgv.RowTemplate.Height;
                };

                // Desactivar la adición de filas por el usuario (la fila “*” al final)
                dgv.AllowUserToAddRows = false;


            }
            finally
            {
                dgv.ResumeLayout();
            }
        }


        /// <summary>
        /// Activa el doble buffer del grid para minimizar flicker al dibujar.
        /// (Propiedad interna; se accede por reflexión).
        /// </summary>
        private static void TryEnableDoubleBuffer(DataGridView dgv)
        {
            try
            {
                typeof(DataGridView).InvokeMember(
                    "DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic
                    | System.Reflection.BindingFlags.Instance
                    | System.Reflection.BindingFlags.SetProperty,
                    null, dgv, new object[] { true });
            }
            catch
            {
                // Si falla (p. ej., cambios de framework), no es crítico.
            }
        }
    }
}
