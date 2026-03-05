using System;
using System.Linq;
using System.Windows.Forms;

namespace Winforms.Theme
{
    /// <summary>
    /// Aplica un formato consistente al DataGridView principal.
    /// </summary>
    public static class GiveMainDataGridViewFormat
    {
        public static void Execute(DataGridView dgv)
        {
            if (dgv == null) return;

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

                dgv.ScrollBars = ScrollBars.Both;                    // ← CLAVE: permite scrollbar horizontal
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dgv.AllowUserToResizeColumns = true;
                dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
                dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                dgv.ColumnHeadersHeight = 30;
                dgv.RowTemplate.Height = 40;

                // ====================== COLUMNA NATURAL + SCROLL HORIZONTAL ======================
                // 1. Calculamos el tamaño exacto según contenido (AllCells)
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                // 2. Forzamos el cálculo inmediato
                dgv.PerformLayout();

                // 3. ¡Congelamos los anchos! (esto es lo que hace aparecer la scrollbar horizontal)
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                // Ajustes por columna (mantenemos tu lógica original)
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    col.MinimumWidth = 100;   // tu valor original
                    col.Width = 300;

                    // Columna especial (Nombre/Descripción) ocupa más espacio natural
                    string nombreCol = col.Name.ToLowerInvariant();
                    if (nombreCol.Contains("descripcion") || nombreCol.Contains("nombre"))
                    {
                        col.MinimumWidth = 380;   // un poco más generoso
                    }

                    // Pequeño padding visual para que no quede pegado
                    col.Width += 12;
                }

                // Altura de filas existentes
                foreach (DataGridViewRow row in dgv.Rows)
                    row.Height = dgv.RowTemplate.Height;

                // Altura para filas que se agreguen después
                dgv.RowsAdded += (s, e) =>
                {
                    for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
                    {
                        if (i < dgv.Rows.Count)
                            dgv.Rows[i].Height = dgv.RowTemplate.Height;
                    }
                };
            }
            finally
            {
                dgv.ResumeLayout();
            }
        }

        private static void TryEnableDoubleBuffer(DataGridView dgv)
        {
            try
            {
                typeof(DataGridView).InvokeMember(
                    "DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                    null, dgv, new object[] { true });
            }
            catch { }
        }
    }
}