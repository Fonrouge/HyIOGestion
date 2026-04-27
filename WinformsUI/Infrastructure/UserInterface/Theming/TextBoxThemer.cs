using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinformsUI.Infrastructure.UserInterface.Theming
{
    public class TextBoxThemer
    {
        public float CornerRadius { get; set; } = 8f;
        public Color BorderColor { get; set; } = Color.FromArgb(180, 180, 180);
        public Color FocusBorder { get; set; } = Color.FromArgb(0, 120, 212);
        public Color BackgroundColor { get; set; } = Color.White;
        public int BorderThickness { get; set; } = 1;
        public Padding InnerPadding { get; set; } = new Padding(6, 4, 6, 4);

        /// <summary>
        /// Aplica o actualiza el estilo redondeado. Seguro para llamar
        /// múltiples veces (hotswap de tema).
        /// </summary>
        public TextBox MakeRounded(TextBox tb, float radius = -1)
        {
            if (radius >= 0) CornerRadius = radius;

            // ── HOTSWAP: el TextBox ya vive dentro de un RoundedPanel ─────────
            if (tb.Parent is RoundedPanel existingPanel)
            {
                existingPanel.UpdateTheme(
                    CornerRadius, BorderColor, FocusBorder,
                    BorderThickness, BackgroundColor);

                tb.BackColor = BackgroundColor;
                tb.ForeColor = tb.ForeColor; // conservar forecolor externo

                // Reajustar posición interna por si cambió el padding/border
                tb.Location = new Point(
                    InnerPadding.Left + BorderThickness,
                    InnerPadding.Top + BorderThickness);
                tb.Width = existingPanel.Width
                           - InnerPadding.Horizontal
                           - BorderThickness * 2;

                return tb;
            }

            // ── PRIMERA VEZ: preparar el TextBox ─────────────────────────────
            tb.BorderStyle = BorderStyle.None;
            tb.BackColor = BackgroundColor;

            if (tb.Parent != null)
                WrapInRoundedPanel(tb);
            else
                tb.ParentChanged += OnParentChanged;

            return tb;
        }

        private void OnParentChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            tb.ParentChanged -= OnParentChanged;
            WrapInRoundedPanel(tb);
        }

        private void WrapInRoundedPanel(TextBox tb)
        {
            if (tb.Parent is RoundedPanel || tb.Parent == null) return;

            var parent = tb.Parent;
            var bounds = tb.Bounds;
            var anchor = tb.Anchor;
            var dock = tb.Dock;

            // 1. Calculamos la altura necesaria de forma precisa
            // Altura del texto + padding superior/inferior + bordes
            int preferredHeight = tb.PreferredHeight + InnerPadding.Vertical + (BorderThickness * 2);

            var panel = new RoundedPanel(
                CornerRadius, BorderColor, FocusBorder,
                BorderThickness, BackgroundColor)
            {
                // Usamos la posición original pero la altura calculada
                Bounds = new Rectangle(bounds.X, bounds.Y, bounds.Width, preferredHeight),
                Anchor = anchor,
                Dock = dock
            };

            // 2. Operación de intercambio atómica para evitar roturas
            parent.SuspendLayout(); // Evita parpadeos y errores de layout

            int index = parent.Controls.GetChildIndex(tb);
            parent.Controls.Remove(tb);

            tb.BorderStyle = BorderStyle.None;
            tb.Dock = DockStyle.None; // El TB interno NUNCA debe tener Dock

            // Posicionamos el TB dentro del panel
            tb.Location = new Point(InnerPadding.Left + BorderThickness, InnerPadding.Top + BorderThickness);
            tb.Width = panel.Width - InnerPadding.Horizontal - (BorderThickness * 2);

            panel.Controls.Add(tb);
            parent.Controls.Add(panel);
            parent.Controls.SetChildIndex(panel, index); // Mantenemos el orden visual original

            parent.ResumeLayout();

            // 3. Vincular el click del panel al foco del TB
            panel.Click += (s, e) => tb.Focus();
        }

        // ─────────────────────────────────────────────────────────────────────
        public class RoundedPanel : Panel
        {
            private float _radius;
            private Color _border;
            private Color _focusBorder;
            private int _thickness;
            private bool _focused;

            public RoundedPanel(float radius, Color border, Color focusBorder,
                                int thickness, Color back)
            {
                SetStyle(ControlStyles.UserPaint |
                         ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.OptimizedDoubleBuffer, true);

                Apply(radius, border, focusBorder, thickness, back);
            }

            // ── API de hotswap ────────────────────────────────────────────────
            public void UpdateTheme(float radius, Color border, Color focusBorder,
                                    int thickness, Color back)
            {
                Apply(radius, border, focusBorder, thickness, back);
                RecalcRegion();
                Invalidate();
            }

            private void Apply(float radius, Color border, Color focusBorder,
                               int thickness, Color back)
            {
                _radius = radius;
                _border = border;
                _focusBorder = focusBorder;
                _thickness = thickness;
                BackColor = back;
            }

            protected override void OnControlAdded(ControlEventArgs e)
            {
                base.OnControlAdded(e);
                e.Control.GotFocus += ChildFocus;
                e.Control.LostFocus += ChildFocus;

                // Sincronizar BackColor del hijo al montar
                e.Control.BackColor = BackColor;
            }

            private void ChildFocus(object sender, EventArgs e)
            {
                _focused = ((Control)sender).Focused;
                Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                var rect = new RectangleF(
                    _thickness / 2f, _thickness / 2f,
                    Width - _thickness,
                    Height - _thickness);

                using (var brush = new SolidBrush(BackColor))
                using (var path = RoundedRect(rect, _radius))
                    g.FillPath(brush, path);

                var penColor = _focused ? _focusBorder : _border;
                using (var pen = new Pen(penColor, _thickness))
                using (var path = RoundedRect(rect, _radius))
                    g.DrawPath(pen, path);
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                RecalcRegion();
            }

            private void RecalcRegion()
            {
                if (Width <= 0 || Height <= 0) return;
                var rect = new RectangleF(0, 0, Width, Height);
                var path = RoundedRect(rect, _radius + 1);
                Region = new Region(path);
                path.Dispose();
            }

            private static GraphicsPath RoundedRect(RectangleF r, float rad)
            {
                var path = new GraphicsPath();
                float d = rad * 2;
                path.AddArc(r.X, r.Y, d, d, 180, 90);
                path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                return path;
            }
        }
    }
}