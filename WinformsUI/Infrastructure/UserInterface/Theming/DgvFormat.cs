using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Winforms.Theme
{
    /// <summary>
    /// Aplica un formato consistente al DataGridView principal.
    /// </summary>
    public static class DgvFormat
    {
        public static void Apply(DataGridView dgv, bool isMiniDgv = false)
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

                dgv.ScrollBars = ScrollBars.Both;
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dgv.AllowUserToResizeColumns = true;
                dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
                dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                dgv.ColumnHeadersHeight = 30;
                dgv.RowTemplate.Height = 40;

               
                dgv.EnableHeadersVisualStyles = false;

                dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
                dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;


                dgv.Invalidate();

                // ====================== COLUMNA NATURAL + SCROLL HORIZONTAL ======================
                // 1. Calculamos el tamaño exacto según contenido (AllCells)
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                // 2. Forzamos el cálculo inmediato
                dgv.PerformLayout();

                // 3. ¡Congelamos los anchos! (esto es lo que hace aparecer la scrollbar horizontal)
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;


                int ColumnMinimumWidth = 0;
                int ColumnWidth = 0;

                if (!isMiniDgv)
                {
                    ColumnWidth = 200;
                    ColumnMinimumWidth = 80;
                }
                else
                {
                    ColumnWidth = 100;
                    ColumnMinimumWidth = 40;
                }

                // Ajustes por columna (mantenemos tu lógica original)
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                   // Pequeño padding visual para que no quede pegado
                    col.Width += ColumnWidth;
                    col.MinimumWidth += ColumnMinimumWidth;
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