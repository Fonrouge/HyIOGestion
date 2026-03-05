using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Winforms.Theme.DarkTheme;   // tu Palette

namespace Winforms.Theme
{
    internal static class ComboBoxThemer
    {
        #region Win32 Ultra-Directo
        private const int WM_PAINT = 0x000F;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ERASEBKGND = 0x0014;
        private const int WM_SETFOCUS = 0x0007;
        private const int WM_KILLFOCUS = 0x0008;

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string appName, string idList);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        #endregion

        // ====================== ESTADO POR COMBOBOX ======================
        private sealed class CmbState
        {
            public CmbRenderer Renderer;
            public bool IsHooked;
            public Palette CurrentPalette;   // siempre tendrá valor después del primer ThemeComboBox
        }

        private static readonly ConditionalWeakTable<ComboBox, CmbState> _state = new ConditionalWeakTable<ComboBox, CmbState>();

        // ====================== API PÚBLICA ======================
        public static void ThemeComboBox(ComboBox cb, Palette palette)
        {
            if (cb == null || cb.IsDisposed ) return;

            var state = _state.GetValue(cb, _ => new CmbState());

            // 1. Matar tema nativo de Windows (solo la primera vez es crítico)
            if (!state.IsHooked)
                try { SetWindowTheme(cb.Handle, "", ""); } catch { }

            // Configuración base (siempre, por si alguien cambió algo)
            cb.DrawMode = DrawMode.OwnerDrawFixed;
            cb.FlatStyle = FlatStyle.Flat;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;

            // 2. Hook del DrawItem (solo una vez)
            if (!state.IsHooked)
            {
                cb.DrawItem += DrawItemHandler;   // ← ahora sin closure!
                state.IsHooked = true;
            }

            // 3. Crear o ACTUALIZAR el renderer
            if (state.Renderer == null)
                state.Renderer = new CmbRenderer(cb, state);

            // 4. ACTUALIZAMOS la paleta actual (¡esto es lo que faltaba!)
            state.CurrentPalette = palette;

            // Forzamos repaint completo (incluye dropdown si está abierto)
            cb.Invalidate();
            if (cb.DroppedDown)
                cb.Invalidate(true); // fuerza repaint del dropdown
        }

        // ====================== DRAWITEM (usa siempre la paleta ACTUAL) ======================
        private static void DrawItemHandler(object sender, DrawItemEventArgs e)
        {
            if (!(sender is ComboBox cb )|| e.Index < 0) return;

            var state = _state.TryGetValue(cb, out var s) ? s : null;
            var p = state?.CurrentPalette ?? throw new InvalidOperationException("Palette no inicializada");

            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            Color backColor = isSelected ? Color.FromArgb(50, p.Accent) : p.Surface;
            Color textColor = isSelected ? p.TextPrimary : p.TextSecondary;

            using (var brush = new SolidBrush(backColor))
                e.Graphics.FillRectangle(brush, e.Bounds);

            string text = cb.GetItemText(cb.Items[e.Index]);

            using (var font = new Font(cb.Font,
                       (e.State & DrawItemState.ComboBoxEdit) != 0 ? FontStyle.Italic : FontStyle.Regular))
            {
                TextRenderer.DrawText(e.Graphics, text, font, e.Bounds, textColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
            }
        }

        // ====================== RENDERER NATIVO (también usa paleta ACTUAL) ======================
        private class CmbRenderer : NativeWindow
        {
            private readonly ComboBox _cb;
            private readonly CmbState _state;

            public CmbRenderer(ComboBox cb, CmbState state)
            {
                _cb = cb;
                _state = state;
                AssignHandle(cb.Handle);
            }

            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case WM_NCPAINT:
                        return; // bloqueamos borde nativo

                    case WM_ERASEBKGND:
                        m.Result = (IntPtr)1;
                        return;

                    case WM_PAINT:
                        base.WndProc(ref m);
                        PaintCustomEverything();
                        return;

                    case WM_SETFOCUS:
                    case WM_KILLFOCUS:
                        base.WndProc(ref m);
                        _cb.Invalidate();
                        return;
                }
                base.WndProc(ref m);
            }

            private void PaintCustomEverything()
            {
                var p = _state.CurrentPalette; // ← siempre la última

                using (var g = Graphics.FromHwnd(_cb.Handle))
                {
                    var rect = _cb.ClientRectangle;
                    int btnWidth = SystemInformation.VerticalScrollBarWidth;

                    // Ajuste para tapar la línea divisoria blanca
                    int overlapFix = 3;
                    var eraserRect = new Rectangle(rect.Right - btnWidth - overlapFix, 0, btnWidth + overlapFix, rect.Height);

                    // 1. Borramos botón nativo + línea divisoria
                    using (var b = new SolidBrush(p.Surface))
                        g.FillRectangle(b, eraserRect);

                    // 2. Flecha custom
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    int cx = rect.Right - (btnWidth / 2);
                    int cy = rect.Height / 2;
                    Point[] arrow = { new Point(cx - 4, cy - 2), new Point(cx + 4, cy - 2), new Point(cx, cy + 2) };

                    using (var pArrow = new SolidBrush(p.TextSecondary))
                        g.FillPolygon(pArrow, arrow);

                    // 3. Borde exterior (cambia con focus)
                    g.SmoothingMode = SmoothingMode.Default;
                    Color borderColor = _cb.Focused ? p.Accent : p.Border;
                    using (var pen = new Pen(borderColor, 1))
                    {
                        g.DrawRectangle(pen, 0, 0, rect.Width - 1, rect.Height - 1);
                    }
                }
            }
        }
    }
}