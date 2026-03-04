using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static Winforms.Theme.DarkTheme;

namespace Winforms.Theme
{
    internal static class ComboBoxThemer
    {
        #region State Management
        private sealed class CmbState
        {
            public CmbRenderer Renderer;
            public bool IsHooked; // Para no suscribir el evento DrawItem mil veces
        }

        private static readonly ConditionalWeakTable<ComboBox, CmbState> _state
            = new ConditionalWeakTable<ComboBox, CmbState>();

        private static CmbState GetState(ComboBox cb)
        {
            return _state.GetValue(cb, _ => new CmbState());
        }
        #endregion

        #region Color Scheme
        private sealed class CmbColorScheme
        {
            public Color BackColor { get; set; }
            public Color TextSecondary { get; set; }
            public Color BorderColor { get; set; }
            public Color SelectionBack { get; set; } // Color al pasar el mouse por items

            public static CmbColorScheme FromPalette(Palette p)
            {
                return new CmbColorScheme
                {
                    // Usamos Surface para que coincida con tu DatePicker
                    BackColor = p.Surface,
                    TextSecondary = p.TextSecondary,
                    BorderColor = p.Border,
                    SelectionBack = p.Accent // O un gris más claro si prefieres
                };
            }
        }
        #endregion

        #region API Principal

        public static void ThemeComboBox(ComboBox cb, Palette palette)
        {
            if (cb == null || cb.IsDisposed) return;

            var state = GetState(cb);
            var colors = CmbColorScheme.FromPalette(palette);

            // 1. EL CAMBIO CLAVE: OwnerDrawFixed
            // Esto obliga al control a dejar de usar el estilo blanco de Windows
            cb.DrawMode = DrawMode.OwnerDrawFixed;
            cb.FlatStyle = FlatStyle.Flat;

            cb.BackColor = colors.BackColor;
            cb.ForeColor = colors.TextSecondary;

            // 2. Suscribirnos al pintado de items (Texto y Fondo)
            if (!state.IsHooked)
            {
                cb.DrawItem += (s, e) => DrawComboBoxItem(s, e, colors);
                state.IsHooked = true;
            }

            // 3. Attachar el renderer (Solo para el borde)
            if (state.Renderer == null)
            {
                state.Renderer = new CmbRenderer(cb, colors);
            }
            else
            {
                state.Renderer.UpdateColors(colors);
                cb.Invalidate();
            }
        }

        #endregion

        #region Lógica de Pintado (DrawItem)

        private static void DrawComboBoxItem(object sender, DrawItemEventArgs e, CmbColorScheme colors)
        {
            var cb = sender as ComboBox;
            if (cb == null || e.Index < 0) return;

            // 1. OBTENER EL TEXTO CORRECTO (Respetando DisplayMember)
            // Esto soluciona el problema de "Presentation.WinForms..."
            string text = cb.GetItemText(cb.Items[e.Index]);

            // 2. DETERMINAR ESTADO (¿Es la lista desplegada o el cuadro cerrado?)
            // Si el rectángulo del ítem es igual al ancho del cliente (aprox), estamos en la lista desplegada.
            // O verificamos el estado ComboBoxEdit.
            bool isEditBox = (e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit;
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // 3. DEFINIR COLORES
            Color backColor;
            Color textColor;

            if (isSelected && !isEditBox)
            {
                // Item seleccionado en la lista desplegada (Hover)
                backColor = colors.SelectionBack;
                textColor = Color.White;
            }
            else
            {
                // Item normal o la caja cerrada (reposo)
                backColor = colors.BackColor;
                textColor = colors.TextSecondary;
            }

            // 4. PINTAR FONDO
            using (var brush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            //ESTO ANDA COMO LA CACA.
            //BORRAR Y EMPEZAR DE NUEVO.



            // 5. PINTAR TEXTO
            // Usamos Italic SOLO si es la caja de texto cerrada (para igualar al DatePicker)
            // Usamos Normal si es la lista desplegable (para que sea fácil de leer)
            FontStyle style = isEditBox ? FontStyle.Italic : FontStyle.Regular;

            using (var font = new Font(cb.Font, style))
            using (var brush = new SolidBrush(textColor))
            {
                var stringFormat = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Near,
                    Trimming = StringTrimming.EllipsisCharacter, // Cortar con "..." si es largo
                    FormatFlags = StringFormatFlags.NoWrap
                };

                // Pequeño margen a la izquierda
                var textRect = new Rectangle(e.Bounds.X + 2, e.Bounds.Y, e.Bounds.Width - 2, e.Bounds.Height);

                e.Graphics.DrawString(text, font, brush, textRect, stringFormat);
            }

            // Opcional: Si es la lista desplegable, dibujar foco para accesibilidad
            if (isSelected && !isEditBox)
            {
                e.DrawFocusRectangle();
            }
        }
        #endregion

        #region Renderer de Borde (NativeWindow)

        // Esta clase solo se encarga de dibujar el borde exterior
        private class CmbRenderer : NativeWindow
        {
            private readonly ComboBox _cb;
            private CmbColorScheme _colors;
            private const int WM_PAINT = 0x000F;

            public CmbRenderer(ComboBox cb, CmbColorScheme colors)
            {
                _cb = cb;
                _colors = colors;
                AssignHandle(cb.Handle);
                _cb.SizeChanged += (s, e) => _cb.Invalidate();
                _cb.LostFocus += (s, e) => _cb.Invalidate();
                _cb.GotFocus += (s, e) => _cb.Invalidate();
            }

            public void UpdateColors(CmbColorScheme colors)
            {
                _colors = colors;
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                if (m.Msg == WM_PAINT)
                {
                    using (var g = Graphics.FromHwnd(_cb.Handle))
                    using (var p = new Pen(_colors.BorderColor))
                    {
                        g.SmoothingMode = SmoothingMode.None;
                        // Dibujamos el borde encima de todo
                        var rect = new Rectangle(0, 0, _cb.Width - 1, _cb.Height - 1);
                        g.DrawRectangle(p, rect);
                    }
                }
            }
        }
        #endregion
    }
}