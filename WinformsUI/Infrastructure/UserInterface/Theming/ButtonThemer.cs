using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinformsUI.Infrastructure.UserInterface.Theming
{
    public class ButtonThemer
    {
        // ── Estilo por defecto ────────────────────────────────────────────────
        public float CornerRadius { get; set; } = 10f;
        public int BorderThickness { get; set; } = 2;

        // ── API principal ─────────────────────────────────────────────────────
        public Button MakeRounded(Button b, float radius = -1)
        {
            if (radius >= 0) CornerRadius = radius;

            // ── HOTSWAP: ya es un RoundedButton ──────────────────────────────
            if (b is RoundedButton rb)
            {
                rb.Radius = CornerRadius;
                rb.BorderThickness = BorderThickness;
                rb.Invalidate();
                return b;
            }

            // ── PRIMERA VEZ: reemplazar por RoundedButton ─────────────────────
            
            
            return SwapForRounded(b);
        }

        private Button SwapForRounded(Button original)
        {
            var parent = original.Parent;
            if (parent == null) return original;

            var rb = new RoundedButton(CornerRadius, BorderThickness)
            {
                Bounds = original.Bounds,
                Anchor = original.Anchor,
                Dock = original.Dock,
                Margin = original.Margin,
                Padding = original.Padding,
                TabIndex = original.TabIndex,
                TabStop = original.TabStop,
                Text = original.Text,
                Font = original.Font,
                BackColor = original.BackColor,
                ForeColor = original.ForeColor,
                Image = original.Image,
                ImageAlign = original.ImageAlign,
                TextAlign = original.TextAlign,
                Tag = original.Tag,
                Name = original.Name,
                FlatStyle = original.FlatStyle,
            };

            rb.Click += (sender, e) => original.PerformClick();
            

            rb.FlatAppearance.BorderColor = original.FlatAppearance.BorderColor;
            rb.FlatAppearance.BorderSize = original.FlatAppearance.BorderSize;
            rb.FlatAppearance.MouseOverBackColor = original.FlatAppearance.MouseOverBackColor;
            rb.FlatAppearance.MouseDownBackColor = original.FlatAppearance.MouseDownBackColor;

            TransferEvents(original, rb);

            int idx = parent.Controls.GetChildIndex(original);
            parent.Controls.Remove(original);
            parent.Controls.Add(rb);
            parent.Controls.SetChildIndex(rb, idx);

            return rb;
        }

        private void MouseHoverCopy(object sender, EventArgs e)
        {
            
        }


        private static void TransferEvents(Button source, Button target)
        {
            foreach (EventHandler h in source.GetClickHandlers())
                target.Click += h;
        }

        // ─────────────────────────────────────────────────────────────────────
        public sealed class RoundedButton : Button
        {
            public float Radius { get; set; }
            public int BorderThickness { get; set; }

            public RoundedButton(float radius, int borderThickness)
            {
                Radius = radius;
                BorderThickness = borderThickness;

                SetStyle(ControlStyles.UserPaint |
                         ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.OptimizedDoubleBuffer, true);

                FlatStyle = FlatStyle.Flat;
                FlatAppearance.BorderSize = 0;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                bool isHover = ClientRectangle.Contains(PointToClient(Cursor.Position));
                bool isDown = MouseButtons == MouseButtons.Left && isHover;

                Color back = isDown ? FlatAppearance.MouseDownBackColor :
                             isHover ? FlatAppearance.MouseOverBackColor :
                                       BackColor;

                if (back == Color.Empty || back.A == 0)
                    back = BackColor;

                var rect = new RectangleF(
                    BorderThickness / 2f,
                    BorderThickness / 2f,
                    Width - BorderThickness,
                    Height - BorderThickness);

                using (var path = RoundedRect(rect, Radius))
                {
                    // Fondo
                    if (back.A > 0)
                    {
                        using (var brush = new SolidBrush(back))
                            g.FillPath(brush, path);
                    }

                    // Borde
                    if (BorderThickness > 0 && FlatAppearance.BorderColor != Color.Empty)
                    {
                        using (var pen = new Pen(FlatAppearance.BorderColor, BorderThickness))
                            g.DrawPath(pen, path);
                    }

                    // Imagen
                    if (Image != null)
                    {
                        var imgRect = CalcImageRect();
                        g.DrawImage(Image, imgRect);
                    }

                    // Texto
                    if (!string.IsNullOrEmpty(Text))
                    {
                        var flags = BuildTextFormat(TextAlign);
                        var textRect = new Rectangle(
                            Padding.Left, Padding.Top,
                            Width - Padding.Horizontal,
                            Height - Padding.Vertical);

                        TextRenderer.DrawText(g, Text, Font, textRect, ForeColor, flags);
                    }
                }

                // Región para transparencia en esquinas
                using (var regPath = RoundedRect(new RectangleF(0, 0, Width, Height), Radius + 1))
                {
                    this.Region = new Region(regPath);
                }
            }

            protected override void OnEnabledChanged(EventArgs e)
            {
                base.OnEnabledChanged(e);
                Invalidate();
            }

            protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); Invalidate(); }
            protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); Invalidate(); }
            protected override void OnMouseDown(MouseEventArgs e) { base.OnMouseDown(e); Invalidate(); }
            protected override void OnMouseUp(MouseEventArgs e) { base.OnMouseUp(e); Invalidate(); }

            private Rectangle CalcImageRect()
            {
                int x;
                switch (ImageAlign)
                {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.BottomLeft:
                        x = Padding.Left;
                        break;
                    case ContentAlignment.TopRight:
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.BottomRight:
                        x = Width - Padding.Right - Image.Width;
                        break;
                    default:
                        x = (Width - Image.Width) / 2;
                        break;
                }

                int y;
                switch (ImageAlign)
                {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.TopCenter:
                    case ContentAlignment.TopRight:
                        y = Padding.Top;
                        break;
                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomCenter:
                    case ContentAlignment.BottomRight:
                        y = Height - Padding.Bottom - Image.Height;
                        break;
                    default:
                        y = (Height - Image.Height) / 2;
                        break;
                }

                return new Rectangle(x, y, Image.Width, Image.Height);
            }

            private static TextFormatFlags BuildTextFormat(ContentAlignment align)
            {
                switch (align)
                {
                    case ContentAlignment.TopLeft:
                        return TextFormatFlags.Top | TextFormatFlags.Left;
                    case ContentAlignment.TopCenter:
                        return TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    case ContentAlignment.TopRight:
                        return TextFormatFlags.Top | TextFormatFlags.Right;
                    case ContentAlignment.MiddleLeft:
                        return TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    case ContentAlignment.MiddleRight:
                        return TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    case ContentAlignment.BottomLeft:
                        return TextFormatFlags.Bottom | TextFormatFlags.Left;
                    case ContentAlignment.BottomCenter:
                        return TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    case ContentAlignment.BottomRight:
                        return TextFormatFlags.Bottom | TextFormatFlags.Right;
                    default:
                        return TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                }
            }

            private static GraphicsPath RoundedRect(RectangleF r, float rad)
            {
                var path = new GraphicsPath();
                float d = rad * 2;
                if (d <= 0) d = 1; // Evitar error si el radio es 0

                path.AddArc(r.X, r.Y, d, d, 180, 90);
                path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                return path;
            }
        }
    }

    internal static class ButtonExtensions
    {
        public static IEnumerable<EventHandler> GetClickHandlers(this Button b)
        {
            var field = typeof(Component).GetField("events", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var list = field?.GetValue(b) as EventHandlerList;
            if (list == null) yield break;

            var key = typeof(Control).GetField("EventClick", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)?.GetValue(null);
            if (key == null) yield break;

            if (list[key] is EventHandler h) yield return h;
        }
    }
}