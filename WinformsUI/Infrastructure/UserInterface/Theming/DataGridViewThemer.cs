using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Winforms.Theme.DarkTheme;

namespace WinformsUI.Infrastructure.UserInterface.Theming
{
    internal static class DataGridViewThemer
    {


        // ============================
        // 8) DATAGRIDVIEW
        // ============================
        private sealed class HeaderGradientSpec
        {
            public Color Begin, End, Border;
            public Color Accent, LowAccent, HighAccent;
            public LinearGradientMode Direction;
        }
        private static readonly ConditionalWeakTable<DataGridView, HeaderGradientSpec> _headerGradients =
            new ConditionalWeakTable<DataGridView, HeaderGradientSpec>();

        internal static void StyleDgv(DataGridView dgv, Palette p, VisualDepth depth)
        {
            if (IsNonPaintable(dgv.Tag)) return;

            TryEnableDoubleBuffering(dgv);

            dgv.BorderStyle = BorderStyle.None;

            dgv.Layout -= Dgv_EnforceNoScrollBars; // Evitar duplicados si se llama varias veces
            dgv.Layout += Dgv_EnforceNoScrollBars;

            // ====================================================================
            // 1. Ocultamos las barras nativas feas
            // ====================================================================


            // ====================================================================
            // 2. Inyectamos nuestras barras modernas
            // ====================================================================
            AttachCustomScrollbars(dgv, p);



            dgv.EnableHeadersVisualStyles = false;
            dgv.BackgroundColor = p.SurfaceAlt;

            dgv.GridColor = p.GridLine;


            if (depth == VisualDepth.ThreeD)
            {
                if (IsDarkPalette(p))
                {
                    AttachGradientHeaderRenderer(dgv, Darken(p.Accent, 0.8), Darken(p.Accent, 0.8),    //Oscuro
                                                 LinearGradientMode.Vertical, p.Border, p);

                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Darken(ChooseReadableForeground(Darken(p.Accent, 0.8)), 0.5);
                }
                else
                {
                    AttachGradientHeaderRenderer(dgv, Darken(p.Accent, -0.8), Darken(p.Accent, -0.8),   //Claro
                                             LinearGradientMode.Vertical, p.Border, p);

                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Darken(ChooseReadableForeground(Darken(p.Accent, -0.8)), -0.5);
                }


            }
            else
            {
                DetachGradientHeaderRenderer(dgv);
                var headerBg = p.LowAccent;
                var headerFg = Color.Red; //CÓDIGO DEBUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUG
                dgv.ColumnHeadersDefaultCellStyle.BackColor = headerBg;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = headerFg;
                dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = headerBg;
                dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = headerFg;
            }

            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.DefaultCellStyle.BackColor = p.SurfaceAlt;
            dgv.DefaultCellStyle.ForeColor = p.TextPrimary;

            var selBg = p.AccentHover;
            dgv.DefaultCellStyle.SelectionBackColor = selBg;
            dgv.DefaultCellStyle.SelectionForeColor = ChooseReadableForeground(selBg);

            dgv.AlternatingRowsDefaultCellStyle.BackColor = p.Surface;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;

            if (HasTag(dgv.Tag, "AutoFill"))
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.Disposed -= DgvOnDisposed;
            dgv.Disposed += DgvOnDisposed;

            dgv.BorderStyle = BorderStyle.None;
        }

        private static void Dgv_EnforceNoScrollBars(object sender, LayoutEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv != null && dgv.ScrollBars != ScrollBars.None)
            {
                dgv.ScrollBars = ScrollBars.None;
            }
        }

        internal static void AccentDgvColumns(DataGridView dgv, Palette p)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                Color bg;
                if (IsLowAccented(col.Tag)) bg = p.LowAccent;
                else if (IsHighAccented(col.Tag)) bg = p.HighAccent;
                else if (IsAccentuable(col.Tag)) bg = p.Accent;
                else continue;

                var fg = ChooseReadableForeground(bg);
                col.HeaderCell.Style.BackColor = bg;
                col.HeaderCell.Style.ForeColor = fg;
            }

            dgv.ColumnAdded -= DgvOnColumnAddedAccent;
            dgv.ColumnAdded += DgvOnColumnAddedAccent;

            dgv.Disposed -= DgvOnDisposed;
            dgv.Disposed += DgvOnDisposed;
        }

        private static void DgvOnColumnAddedAccent(object sender, DataGridViewColumnEventArgs e)
        {
            var dgv = (DataGridView)sender;
            var p = GetPaletteFor(dgv);
            AccentDgvColumns(dgv, p);
        }
        private static void DgvOnDisposed(object sender, EventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv != null) DetachGradientHeaderRenderer(dgv);
        }

        private static void AttachGradientHeaderRenderer(DataGridView dgv, Color begin, Color end,
                                                       LinearGradientMode direction, Color border, Palette p)
        {
            if (dgv == null) return;
            _headerGradients.Remove(dgv);

            _headerGradients.Add(dgv, new HeaderGradientSpec
            {
                Begin = begin,
                End = end,
                Direction = direction,
                Border = border,
                Accent = p.Accent,
                LowAccent = p.LowAccent,
                HighAccent = p.HighAccent
            });

            dgv.CellPainting -= DgvOnCellPainting_HeaderGradient;
            dgv.CellPainting += DgvOnCellPainting_HeaderGradient;
            dgv.Invalidate();
        }

        private static void DetachGradientHeaderRenderer(DataGridView dgv)
        {
            if (dgv == null) return;
            dgv.CellPainting -= DgvOnCellPainting_HeaderGradient;
            _headerGradients.Remove(dgv);
            dgv.Invalidate();
        }

        private static void DgvOnCellPainting_HeaderGradient(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1 || e.ColumnIndex < 0) return;
            var dgv = (DataGridView)sender;
            HeaderGradientSpec spec;
            if (!_headerGradients.TryGetValue(dgv, out spec)) return;

            e.Handled = true;
            var r = e.CellBounds;
            var col = dgv.Columns[e.ColumnIndex];

            Color accentColor = Color.Empty;
            bool isAccented = false;

            if (IsLowAccented(col.Tag)) { accentColor = spec.LowAccent; isAccented = true; }
            else if (IsHighAccented(col.Tag)) { accentColor = spec.HighAccent; isAccented = true; }
            else if (IsAccentuable(col.Tag)) { accentColor = spec.Accent; isAccented = true; }

            if (isAccented)
            {
                using (var br = new SolidBrush(accentColor)) e.Graphics.FillRectangle(br, r);
                e.CellStyle.ForeColor = ChooseReadableForeground(accentColor);
            }
            else
            {
                using (var br = new LinearGradientBrush(r, spec.Begin, spec.End, spec.Direction))
                    e.Graphics.FillRectangle(br, r);
                var mid = Blend(spec.Begin, spec.End, .5);

            }

            using (var pen = new Pen(spec.Border))
                e.Graphics.DrawRectangle(pen, r.X, r.Y, r.Width - 1, r.Height - 1);

            var oldFore = e.CellStyle.ForeColor;
            e.PaintContent(r);
            e.CellStyle.ForeColor = oldFore;
        }



        // ============================
        // 14) SCROLLBARS PERSONALIZADAS (DGV)
        // ============================

        private static readonly ConditionalWeakTable<DataGridView, DgvScrollHelper> _dgvScrollHelpers =
            new ConditionalWeakTable<DataGridView, DgvScrollHelper>();

        private static void AttachCustomScrollbars(DataGridView dgv, Palette p)
        {
            if (dgv == null || dgv.IsDisposed) return;

            // Si ya tiene el helper, solo actualizamos los colores
            if (_dgvScrollHelpers.TryGetValue(dgv, out var helper))
            {
                helper.UpdatePalette(p);
                return;
            }

            // Si no, creamos el sistema de scroll y lo adjuntamos
            var newHelper = new DgvScrollHelper(dgv, p);
            _dgvScrollHelpers.Add(dgv, newHelper);
        }

        /// <summary>
        /// Control de Scrollbar moderno dibujado con GDI+
        /// </summary>
        private class ModernScrollBar : Control
        {
            public event EventHandler Scroll;
            private int _value = 0;
            private int _maximum = 100;
            private int _largeChange = 10;
            private bool _isHovered = false;
            private bool _isDragging = false;
            private int _clickPoint = 0;
            private int _dragStartValue = 0;

            // Colores
            public Color TrackColor { get; set; } = Color.FromArgb(30, 30, 30);
            public Color ThumbColor { get; set; } = Color.FromArgb(60, 60, 60);
            public Color ThumbHoverColor { get; set; } = Color.FromArgb(90, 90, 90);
            public bool IsVertical { get; set; } = true;

            public ModernScrollBar()
            {
                SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            }

            public int Maximum { get { return _maximum; } set { _maximum = value; Invalidate(); } }
            public int LargeChange { get { return _largeChange; } set { _largeChange = value; Invalidate(); } }
            public int Value
            {
                get { return _value; }
                set
                {
                    int v = Math.Max(0, Math.Min(value, _maximum - (_largeChange > 0 ? _largeChange - 1 : 0)));
                    if (_value != v) { _value = v; Invalidate(); Scroll?.Invoke(this, EventArgs.Empty); }
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.None;
                using (var brush = new SolidBrush(TrackColor))
                    e.Graphics.FillRectangle(brush, ClientRectangle);

                int trackSize = IsVertical ? Height : Width;
                int thumbSize = Math.Max(20, (int)((float)_largeChange / _maximum * trackSize));

                // Cálculos de posición
                int usableTrack = trackSize - thumbSize;
                int maxVal = _maximum - _largeChange; // Rango lógico efectivo
                if (maxVal <= 0) maxVal = 1;

                float percent = (float)_value / maxVal;
                int pixelPos = (int)(percent * usableTrack);

                // Evitar desbordes gráficos
                if (pixelPos < 0) pixelPos = 0;
                if (pixelPos > usableTrack) pixelPos = usableTrack;

                Rectangle thumbRect = IsVertical
                    ? new Rectangle(2, pixelPos, Width - 4, thumbSize)
                    : new Rectangle(pixelPos, 2, thumbSize, Height - 4);

                Color c = _isDragging ? Darken(ThumbHoverColor, 0.1) : (_isHovered ? ThumbHoverColor : ThumbColor);

                using (var brush = new SolidBrush(c))
                {
                    // Dibujamos un rectángulo redondeado suave
                    e.Graphics.FillRectangle(brush, thumbRect);
                }
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                base.OnMouseDown(e);
                int trackSize = IsVertical ? Height : Width;
                int thumbSize = Math.Max(20, (int)((float)_largeChange / _maximum * trackSize));
                int usableTrack = trackSize - thumbSize;
                int maxVal = _maximum - _largeChange;
                if (maxVal <= 0) maxVal = 1;

                float percent = (float)_value / maxVal;
                int pixelPos = (int)(percent * usableTrack);

                Rectangle thumbRect = IsVertical
                    ? new Rectangle(0, pixelPos, Width, thumbSize)
                    : new Rectangle(pixelPos, 0, thumbSize, Height);

                if (thumbRect.Contains(e.Location))
                {
                    _isDragging = true;
                    _clickPoint = IsVertical ? e.Y : e.X;
                    _dragStartValue = _value;
                }
                else
                {
                    // Click en el track (paging)
                    int clickPos = IsVertical ? e.Y : e.X;
                    if (clickPos < pixelPos) Value -= _largeChange;
                    else Value += _largeChange;
                }
            }

            protected override void OnMouseMove(MouseEventArgs e)
            {
                base.OnMouseMove(e);
                if (!_isHovered) { _isHovered = true; Invalidate(); }

                if (_isDragging)
                {
                    int delta = (IsVertical ? e.Y : e.X) - _clickPoint;
                    int trackSize = IsVertical ? Height : Width;
                    int thumbSize = Math.Max(20, (int)((float)_largeChange / _maximum * trackSize));
                    int usableTrack = trackSize - thumbSize;

                    if (usableTrack > 0)
                    {
                        float valPerPixel = (float)(_maximum - _largeChange) / usableTrack;
                        Value = _dragStartValue + (int)(delta * valPerPixel);
                    }
                }
            }

            protected override void OnMouseUp(MouseEventArgs e)
            {
                base.OnMouseUp(e);
                _isDragging = false;
                Invalidate();
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                base.OnMouseLeave(e);
                _isHovered = false;
                Invalidate();
            }
        }

        /// <summary>
        /// Clase "pegamento" que conecta el DataGridView con las Scrollbars falsas.
        /// Maneja el posicionamiento flotante y la sincronización de eventos.
        /// </summary>
        private class DgvScrollHelper : IDisposable
        {
            private readonly DataGridView _dgv;
            private readonly ModernScrollBar _vScroll;
            private readonly ModernScrollBar _hScroll;
            private bool _isUpdating = false;

            public DgvScrollHelper(DataGridView dgv, Palette p)
            {
                _dgv = dgv;

                // Crear Scrollbars
                _vScroll = new ModernScrollBar { IsVertical = true, Width = 12, Visible = false, Cursor = Cursors.Default };
                _hScroll = new ModernScrollBar { IsVertical = false, Height = 12, Visible = false, Cursor = Cursors.Default };

                UpdatePalette(p);

                // Agregar al PADRE del DGV para que floten encima
                // IMPORTANTE: El DGV debe estar en un contenedor (Form, Panel, etc).
                if (_dgv.Parent != null)
                {
                    _dgv.Parent.Controls.Add(_vScroll);
                    _dgv.Parent.Controls.Add(_hScroll);
                    _vScroll.BringToFront();
                    _hScroll.BringToFront();
                }
                else
                {
                    _dgv.ParentChanged += (s, e) =>
                    {
                        if (_dgv.Parent != null)
                        {
                            _dgv.Parent.Controls.Add(_vScroll);
                            _dgv.Parent.Controls.Add(_hScroll);
                            _vScroll.BringToFront();
                            _hScroll.BringToFront();
                            UpdateScrollBounds();
                        }
                    };
                }

                // Eventos
                _dgv.Resize += (s, e) => UpdateScrollBounds();
                _dgv.Move += (s, e) => UpdateScrollBounds();

                _dgv.RowsAdded += (s, e) => UpdateScrollValues();
                _dgv.RowsRemoved += (s, e) => UpdateScrollValues();
                _dgv.ColumnAdded += (s, e) => UpdateScrollValues();
                _dgv.ColumnRemoved += (s, e) => UpdateScrollValues();
                _dgv.Scroll += (s, e) => SyncScrollFromDgv();
                _dgv.ColumnWidthChanged += (s, e) => UpdateScrollValues();
                _dgv.RowHeadersWidthChanged += (s, e) => UpdateScrollValues();
                _dgv.ClientSizeChanged += (s, e) => UpdateScrollBounds();
                _dgv.MouseWheel += Dgv_MouseWheel; // DGV con ScrollBars.None pierde rueda, hay que reimplementarla

                _vScroll.Scroll += (s, e) =>
                {
                    if (_isUpdating) return;
                    try
                    {
                        if (_dgv.RowCount > 0)
                            _dgv.FirstDisplayedScrollingRowIndex = _vScroll.Value;
                    }
                    catch { }
                };

                _hScroll.Scroll += (s, e) =>
                {
                    if (_isUpdating) return;
                    try
                    {
                        _dgv.HorizontalScrollingOffset = _hScroll.Value;
                    }
                    catch { }
                };

                UpdateScrollBounds();
            }

            public void UpdatePalette(Palette p)
            {
                // Estética: Track sutil (Surface), Thumb visible (Border o Accent)
                _vScroll.TrackColor = p.Surface; // Fondo del canal
                _vScroll.ThumbColor = p.Border;  // Barra en reposo
                _vScroll.ThumbHoverColor = p.Accent; // Barra al pasar mouse

                _hScroll.TrackColor = p.Surface;
                _hScroll.ThumbColor = p.Border;
                _hScroll.ThumbHoverColor = p.Accent;

                _vScroll.Invalidate();
                _hScroll.Invalidate();
            }

            private void Dgv_MouseWheel(object sender, MouseEventArgs e)
            {
                // Simulación manual del scroll vertical con rueda
                if (!_vScroll.Visible) return;

                int scrollLines = SystemInformation.MouseWheelScrollLines;
                int delta = e.Delta > 0 ? -scrollLines : scrollLines;

                int newValue = _vScroll.Value + delta;
                _vScroll.Value = newValue; // Esto dispara el evento Scroll del control, que actualiza el DGV
            }

            private void UpdateScrollBounds()
            {
                if (_dgv.Parent == null) return;

                // Posicionar las barras SOBRE el borde derecho/inferior del DGV
                // Ajustamos -1 o -2 pixels para que queden dentro del borde visual

                var r = _dgv.Bounds;
                int barW = _vScroll.Width;
                int barH = _hScroll.Height;

                // Vertical: Lado derecho
                _vScroll.Location = new Point(r.Right - barW, r.Top + _dgv.ColumnHeadersHeight); // Dejar libre el Header
                _vScroll.Size = new Size(barW, r.Height - barH - _dgv.ColumnHeadersHeight);

                // Horizontal: Lado inferior
                _hScroll.Location = new Point(r.Left, r.Bottom - barH);
                _hScroll.Size = new Size(r.Width - barW, barH);

                UpdateScrollValues();
            }

            private void UpdateScrollValues()
            {
                if (_isUpdating) return;
                _isUpdating = true;

                // --- Vertical ---
                int totalRows = _dgv.RowCount;
                int visibleRows = _dgv.DisplayedRowCount(false);

                if (totalRows > visibleRows && totalRows > 0)
                {
                    _vScroll.Visible = true;
                    _vScroll.Maximum = totalRows;
                    _vScroll.LargeChange = visibleRows;
                    // Sincronizar valor actual
                    try { _vScroll.Value = _dgv.FirstDisplayedScrollingRowIndex; } catch { }
                }
                else
                {
                    _vScroll.Visible = false;
                    _vScroll.Value = 0;
                }

                // --- Horizontal ---
                // Calculamos el ancho total de columnas vs el ancho visible
                int totalWidth = 0;
                foreach (DataGridViewColumn col in _dgv.Columns) totalWidth += col.Visible ? col.Width : 0;

                int visibleWidth = _dgv.ClientSize.Width - (_vScroll.Visible ? _vScroll.Width : 0);

                if (totalWidth > visibleWidth)
                {
                    _hScroll.Visible = true;
                    _hScroll.Maximum = totalWidth;
                    _hScroll.LargeChange = visibleWidth;
                    try { _hScroll.Value = _dgv.HorizontalScrollingOffset; } catch { }
                }
                else
                {
                    _hScroll.Visible = false;
                    _hScroll.Value = 0;
                }

                _isUpdating = false;
            }

            private void SyncScrollFromDgv()
            {
                if (_isUpdating) return;
                _isUpdating = true;
                try
                {
                    if (_vScroll.Visible) _vScroll.Value = _dgv.FirstDisplayedScrollingRowIndex;
                    if (_hScroll.Visible) _hScroll.Value = _dgv.HorizontalScrollingOffset;
                }
                catch { }
                _isUpdating = false;
            }

            public void Dispose()
            {
                if (_vScroll != null && !_vScroll.IsDisposed) _vScroll.Dispose();
                if (_hScroll != null && !_hScroll.IsDisposed) _hScroll.Dispose();
            }
        }




    }
}
