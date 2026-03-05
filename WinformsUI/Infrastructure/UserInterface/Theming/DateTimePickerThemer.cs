using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Winforms.Theme.DarkTheme;

namespace Winforms.Theme
{
    internal static class DateTimePickerThemer
    {
        #region Win32 Constants & Imports

        private const int GWL_STYLE = -16;
        private const int GWL_EXSTYLE = -20;
        private const int WS_BORDER = 0x00800000;
        private const int WS_EX_CLIENTEDGE = 0x00000200;

        private const int WM_SETFOCUS = 0x0007;
        private const int WM_KILLFOCUS = 0x0008;
        private const int WM_PAINT = 0x000F;
        private const int WM_ERASEBKGND = 0x0014;
        private const int WM_NCPAINT = 0x0085;

        private const int DTM_FIRST = 0x1000;
        private const int DTM_GETMONTHCAL = DTM_FIRST + 8;
        private const int MCM_FIRST = 0x1000;
        private const int MCM_GETMINREQRECT = MCM_FIRST + 9;
        private const int MCM_SETCOLOR = MCM_FIRST + 10;

        private const int MCSC_BACKGROUND = 0;
        private const int MCSC_TEXT = 1;
        private const int MCSC_TITLEBK = 2;
        private const int MCSC_TITLETEXT = 3;
        private const int MCSC_TRAILINGTEXT = 4;

        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_FRAMECHANGED = 0x0020;

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string appName, string idList);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        private static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        [DllImport("user32.dll")]
        private static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        // Soporte P/Invoke seguro para 32 y 64 bits
        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8) return GetWindowLongPtr64(hWnd, nIndex);
            return new IntPtr(GetWindowLong32(hWnd, nIndex));
        }

        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8) return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        private static RECT SendMessageGetRect(IntPtr hWnd, int msg)
        {
            IntPtr pRect = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RECT)));
            try
            {
                Marshal.StructureToPtr(new RECT(), pRect, false);
                SendMessage(hWnd, msg, IntPtr.Zero, pRect);
                return (RECT)Marshal.PtrToStructure(pRect, typeof(RECT));
            }
            finally
            {
                Marshal.FreeHGlobal(pRect);
            }
        }

        #endregion

        #region Estado y Clases Auxiliares

        private sealed class DtpState
        {
            public bool WindowThemeApplied;
            public bool DropDownHooked;
            public bool ApplyingColors;
            public DtpRenderer Renderer;
        }

        private static readonly ConditionalWeakTable<DateTimePicker, DtpState> _state = new ConditionalWeakTable<DateTimePicker, DtpState>();

        private static DtpState GetState(DateTimePicker dtp) => _state.GetValue(dtp, _ => new DtpState());

        private sealed class DtpColorScheme
        {
            public Color TextBoxBack { get; set; }
            public Color TextBoxFore { get; set; }
            public Color CalendarBack { get; set; }
            public Color CalendarText { get; set; }
            public Color AccentBack { get; set; }
            public Color AccentText { get; set; }
            public Color TrailingText { get; set; }
            public Color TextSecondary { get; set; }
            public Color BorderColor { get; set; }

            public static DtpColorScheme FromPalette(Palette p)
            {
                var baseBack = p.SurfaceAlt;
                // Asumo que ChooseReadableForeground existe en tu clase DarkTheme
                var baseText = ChooseReadableForeground(baseBack);
                var accentBack = p.Accent;
                var accentText = ChooseReadableForeground(accentBack);

                return new DtpColorScheme
                {
                    TextBoxBack = baseBack,
                    TextBoxFore = baseText,
                    CalendarBack = baseBack,
                    CalendarText = baseText,
                    AccentBack = accentBack,
                    AccentText = accentText,
                    TrailingText = p.SurfaceAlt,
                    BorderColor = p.Border,
                    TextSecondary = p.TextSecondary
                };
            }

            // Si no tenías el método, descomentá este:
            // private static Color ChooseReadableForeground(Color c) => c.R * 0.299 + c.G * 0.587 + c.B * 0.114 > 186 ? Color.Black : Color.White;
        }

        #endregion

        #region API Principal

        public static void ThemeDateTimePicker(DateTimePicker dtp, Palette palette)
        {
            if (dtp == null || dtp.IsDisposed) return;

            var state = GetState(dtp);
            var colors = DtpColorScheme.FromPalette(palette);

            EnsureClassicThemeApplied(dtp, state);
            ApplyDateTimePickerColors(dtp, colors, state);
            WireMonthCalendar(dtp, state);

            if (state.Renderer == null)
            {
                state.Renderer = new DtpRenderer(dtp, colors);
            }
            else
            {
                state.Renderer.UpdateColors(colors);
                dtp.Invalidate();
            }

            // Asignación de fuente y color general
            dtp.ForeColor = palette.TextSecondary;
        }

        #endregion

        #region Implementación del Renderer (Subclassing)

        private class DtpRenderer : NativeWindow
        {
            private readonly DateTimePicker _dtp;
            private DtpColorScheme _colors;

            public DtpRenderer(DateTimePicker dtp, DtpColorScheme colors)
            {
                _dtp = dtp;
                _colors = colors;
                AssignHandle(dtp.Handle);
                RemoveNativeBorder();
            }

            public void UpdateColors(DtpColorScheme colors)
            {
                _colors = colors;
            }

            private void RemoveNativeBorder()
            {
                IntPtr h = _dtp.Handle;
                long style = GetWindowLongPtr(h, GWL_STYLE).ToInt64();
                style &= ~WS_BORDER;
                SetWindowLongPtr(h, GWL_STYLE, new IntPtr(style));

                long exStyle = GetWindowLongPtr(h, GWL_EXSTYLE).ToInt64();
                exStyle &= ~WS_EX_CLIENTEDGE;
                SetWindowLongPtr(h, GWL_EXSTYLE, new IntPtr(exStyle));

                SetWindowPos(h, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
            }

            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case WM_NCPAINT:
                        // Bloquear el borde gris nativo
                        return;

                    case WM_ERASEBKGND:
                        // Evitar parpadeos al limpiar el fondo
                        m.Result = (IntPtr)1;
                        return;

                    case WM_PAINT:
                        // 1. Dejar que el OS procese primero (vital para estado interno)
                        base.WndProc(ref m);
                        // 2. Pintar nuestra UI plana por encima
                        PaintCustomLook();
                        return;

                    case WM_SETFOCUS:
                    case WM_KILLFOCUS:
                        base.WndProc(ref m);
                        // Forzamos repintado para actualizar el color del borde (estado activo/inactivo)
                        _dtp.Invalidate();
                        return;
                }

                base.WndProc(ref m);
            }

            private void PaintCustomLook()
            {
                using (var g = Graphics.FromHwnd(_dtp.Handle))
                {
                    var rect = _dtp.ClientRectangle;
                    int buttonWidth = SystemInformation.VerticalScrollBarWidth;

                    // 1. Limpiar todo el fondo de una vez para evitar parpadeos y solapamientos
                    using (var backBrush = new SolidBrush(_colors.TextBoxBack))
                    {
                        g.FillRectangle(backBrush, rect);
                    }

                    // 2. Dibujar el texto centrado y limpio (sin espacios agregados al texto)
                    var textRect = new Rectangle(rect.Left + 4, rect.Top, rect.Width - buttonWidth - 4, rect.Height);
                    TextRenderer.DrawText(g, _dtp.Text, _dtp.Font, textRect, _colors.TextBoxFore,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.PreserveGraphicsClipping);

                    // 3. Dibujar la flecha moderna (Polygon sólido)
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    int cx = rect.Right - (buttonWidth / 2);
                    int cy = rect.Height / 2;

                    // Ajuste sutil de las coordenadas del triángulo
                    Point[] arrow = new Point[] {
                        new Point(cx - 4, cy - 1),
                        new Point(cx + 4, cy - 1),
                        new Point(cx, cy + 3)
                    };

                    using (var arrowBrush = new SolidBrush(_colors.TextBoxFore))
                    {
                        g.FillPolygon(arrowBrush, arrow);
                    }
                    g.SmoothingMode = SmoothingMode.Default;

                    // 4. Dibujar Borde (Destacar si el control tiene el foco)
                    Color currentBorderColor = _dtp.Focused ? _colors.AccentBack : _colors.BorderColor;
                    using (var pen = new Pen(currentBorderColor))
                    {
                        g.DrawRectangle(pen, 0, 0, rect.Width - 1, rect.Height - 1);
                    }
                }
            }
        }

        #endregion

        #region Helpers Existentes

        private static void EnsureClassicThemeApplied(DateTimePicker dtp, DtpState state)
        {
            if (state.WindowThemeApplied) return;
            if (dtp.IsHandleCreated)
            {
                ApplyClassicTheme(dtp.Handle);
                state.WindowThemeApplied = true;
            }
            else
            {
                dtp.HandleCreated += (s, e) =>
                {
                    ApplyClassicTheme(dtp.Handle);
                    state.WindowThemeApplied = true;
                };
            }
        }

        private static void ApplyClassicTheme(IntPtr handle)
        {
            if (handle == IntPtr.Zero) return;
            try { SetWindowTheme(handle, "", ""); } catch { }
        }

        private static void ApplyDateTimePickerColors(DateTimePicker dtp, DtpColorScheme c, DtpState state)
        {
            if (state.ApplyingColors) return;
            state.ApplyingColors = true;
            try
            {
                if (dtp.Format != DateTimePickerFormat.Custom) dtp.Format = DateTimePickerFormat.Custom;
                if (string.IsNullOrEmpty(dtp.CustomFormat)) dtp.CustomFormat = "dd/MM/yyyy";
                dtp.BackColor = c.TextBoxBack;
                dtp.ForeColor = c.TextBoxFore;
            }
            finally { state.ApplyingColors = false; }
        }

        private static void WireMonthCalendar(DateTimePicker dtp, DtpState state)
        {
            if (state.DropDownHooked) return;
            dtp.DropDown += DtpOnDropDownApplyMonthCal;
            state.DropDownHooked = true;
        }

        private static void DtpOnDropDownApplyMonthCal(object sender, EventArgs e)
        {
            var dtp = sender as DateTimePicker;
            if (dtp == null || !dtp.IsHandleCreated) return;

            // IMPORTANTE: Asegurate de tener acceso a GetPaletteFor(dtp) en tu contexto.
            var palette = GetPaletteFor(dtp);
            var colors = DtpColorScheme.FromPalette(palette);

            dtp.BeginInvoke((Action)delegate
            {
                IntPtr hMc = SendMessage(dtp.Handle, DTM_GETMONTHCAL, IntPtr.Zero, IntPtr.Zero);
                if (hMc == IntPtr.Zero) return;
                ApplyClassicTheme(hMc);
                ApplyMonthCalColors(hMc, colors);
                FixMonthCalSize(hMc);
            });
        }

        private static void ApplyMonthCalColors(IntPtr hMc, DtpColorScheme c)
        {
            Func<Color, int> ToWin32 = color => ColorTranslator.ToWin32(color);
            SendMessage(hMc, MCM_SETCOLOR, (IntPtr)MCSC_BACKGROUND, (IntPtr)ToWin32(c.CalendarBack));
            SendMessage(hMc, MCM_SETCOLOR, (IntPtr)MCSC_TEXT, (IntPtr)ToWin32(c.CalendarText));
            SendMessage(hMc, MCM_SETCOLOR, (IntPtr)MCSC_TITLEBK, (IntPtr)ToWin32(c.AccentBack));
            SendMessage(hMc, MCM_SETCOLOR, (IntPtr)MCSC_TITLETEXT, (IntPtr)ToWin32(c.AccentText));
            SendMessage(hMc, MCM_SETCOLOR, (IntPtr)MCSC_TRAILINGTEXT, (IntPtr)ToWin32(c.TrailingText));
        }

        private static void FixMonthCalSize(IntPtr hMc)
        {
            if (hMc == IntPtr.Zero) return;
            RECT minRc = SendMessageGetRect(hMc, MCM_GETMINREQRECT);
            int wantedW = minRc.Right - minRc.Left + 4;
            int wantedH = minRc.Bottom - minRc.Top + 4;
            IntPtr hParent = GetParent(hMc);
            if (hParent == IntPtr.Zero) return;
            RECT wndRc;
            if (!GetWindowRect(hParent, out wndRc)) return;
            MoveWindow(hMc, 0, 0, wantedW, wantedH, true);
            MoveWindow(hParent, wndRc.Left, wndRc.Top, wantedW, wantedH, true);
            InvalidateRect(hMc, IntPtr.Zero, true);
            InvalidateRect(hParent, IntPtr.Zero, true);
            UpdateWindow(hMc);
            UpdateWindow(hParent);
        }

        #endregion
    }
}