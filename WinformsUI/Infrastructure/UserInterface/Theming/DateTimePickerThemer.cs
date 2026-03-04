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

        // Constantes para estilos de ventana
        private const int GWL_STYLE = -16;
        private const int GWL_EXSTYLE = -20;
        private const int WS_BORDER = 0x00800000;
        private const int WS_EX_CLIENTEDGE = 0x00000200;

        // Mensajes
        private const int WM_PAINT = 0x000F;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ERASEBKGND = 0x0014;

        // MonthCalendar messages
        private const int DTM_FIRST = 0x1000;
        private const int DTM_GETMONTHCAL = DTM_FIRST + 8;
        private const int MCM_FIRST = 0x1000;
        private const int MCM_SETCOLOR = MCM_FIRST + 10;
        private const int MCM_GETMINREQRECT = MCM_FIRST + 9;

        // MonthCalendar color constants
        private const int MCSC_BACKGROUND = 0;
        private const int MCSC_TEXT = 1;
        private const int MCSC_TITLEBK = 2;
        private const int MCSC_TITLETEXT = 3;
        private const int MCSC_TRAILINGTEXT = 4;

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

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_FRAMECHANGED = 0x0020;

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

        #region Estado y Renderer

        private sealed class DtpState
        {
            public bool WindowThemeApplied;
            public bool DropDownHooked;
            public bool ApplyingColors;
            // Guardamos referencia al renderer para que no lo recolecte el GC
            public DtpRenderer Renderer;
        }

        private static readonly ConditionalWeakTable<DateTimePicker, DtpState> _state
            = new ConditionalWeakTable<DateTimePicker, DtpState>();

        private static DtpState GetState(DateTimePicker dtp)
        {
            return _state.GetValue(dtp, _ => new DtpState());
        }

        #endregion

        #region Esquema de colores

        // (Tu clase DtpColorScheme original se mantiene igual)
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

            // Color del borde del DTP (Nuevo)
            public Color BorderColor { get; set; }

            public static DtpColorScheme FromPalette(Palette p)
            {
                var baseBack = p.SurfaceAlt;
                var baseText = ChooseReadableForeground(baseBack);
                var accentBack = p.Accent;
                var accentText = ChooseReadableForeground(accentBack);
                var trailingText = p.SurfaceAlt;

                return new DtpColorScheme
                {
                    TextBoxBack = baseBack,
                    TextBoxFore = baseText,
                    CalendarBack = baseBack,
                    CalendarText = baseText,
                    AccentBack = accentBack,
                    AccentText = accentText,
                    TrailingText = trailingText,
                    // Usamos el acento o un gris suave para el borde
                    BorderColor = p.Border,
                    TextSecondary = p.TextSecondary
                };
            }
        }

        // Simulación de helpers externos
        // private static Color ChooseReadableForeground(Color c) => c.R + c.G + c.B > 382 ? Color.Black : Color.White;

        #endregion

        #region API Principal

        public static void ThemeDateTimePicker(DateTimePicker dtp, Palette palette)
        {
            if (dtp == null || dtp.IsDisposed) return;

            var state = GetState(dtp);
            var colors = DtpColorScheme.FromPalette(palette);

            // 1. Quitar tema visual de Windows
            EnsureClassicThemeApplied(dtp, state);

            // 2. Aplicar colores base .NET
            ApplyDateTimePickerColors(dtp, colors, state);

            // 3. Hook para el dropdown (calendario antiguo)
            WireMonthCalendar(dtp, state);

            // 4. Attachar el Renderer para pintar bordes y flecha
            if (state.Renderer == null)
            {
                state.Renderer = new DtpRenderer(dtp, colors);
            }
            else
            {
                state.Renderer.UpdateColors(colors);
                dtp.Invalidate(); // Forzar repintado
            }
            dtp.ForeColor = palette.TextSecondary;
        }

        #endregion

        #region Implementación del Renderer (Subclassing)

        /// <summary>
        /// Esta clase se encarga de interceptar el pintado del control para quitar
        /// los bordes 3D nativos y dibujar una flecha moderna.
        /// </summary>
        private class DtpRenderer : NativeWindow
        {
            private readonly DateTimePicker _dtp;
            private DtpColorScheme _colors;

            public DtpRenderer(DateTimePicker dtp, DtpColorScheme colors)
            {
                _dtp = dtp;
                _colors = colors;
                AssignHandle(dtp.Handle);

                // Al iniciar, forzamos quitar los bordes 3D de Windows
                RemoveNativeBorder();
            }

            public void UpdateColors(DtpColorScheme colors)
            {
                _colors = colors;
            }

            private void RemoveNativeBorder()
            {
                IntPtr h = _dtp.Handle;
                // Quitar WS_BORDER
                int style = GetWindowLong(h, GWL_STYLE);
                style &= ~WS_BORDER;
                SetWindowLong(h, GWL_STYLE, style);

                // Quitar WS_EX_CLIENTEDGE (el efecto hundido 3D)
                int exStyle = GetWindowLong(h, GWL_EXSTYLE);
                exStyle &= ~WS_EX_CLIENTEDGE;
                SetWindowLong(h, GWL_EXSTYLE, exStyle);

                // Forzar a Windows a recalcular el área cliente
                SetWindowPos(h, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
            }

            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case WM_NCPAINT:
                        // Bloqueamos el pintado del borde nativo (devolvemos 0)
                        // Esto evita que Windows intente dibujar el borde gris clásico.
                        return;

                    case WM_PAINT:
                        // 1. Dejar que el control pinte el texto y fondo base
                        base.WndProc(ref m);

                        // 2. Pintar nuestra decoración encima
                        PaintCustomLook();
                        return;

                    case WM_ERASEBKGND:
                        // Evita parpadeo
                        m.Result = (IntPtr)1;
                        return;
                }

                base.WndProc(ref m);
            }

            // Dentro de DateTimePickerThemer.cs -> clase privada DtpRenderer

            private void PaintCustomLook()
            {
                // Usamos Graphics desde el handle para pintar sobre todo
                using (var g = Graphics.FromHwnd(_dtp.Handle))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    var rect = _dtp.ClientRectangle;

                    // -- A. PINTAR SOBRE EL BOTÓN NATIVO --
                    int nativeButtonWidth = SystemInformation.VerticalScrollBarWidth;
                    int overlapFix = 3;

                    var eraserRect = new Rectangle(
                        rect.Right - nativeButtonWidth - overlapFix,
                        0,
                        nativeButtonWidth + overlapFix,
                        rect.Height
                    );

                    using (var brush = new SolidBrush(_colors.TextBoxBack))
                    {
                        g.FillRectangle(brush, eraserRect);
                    }

                    // -- B. DIBUJAR FLECHA MODERNA --
                    var cx = rect.Right - (nativeButtonWidth / 2);
                    var cy = rect.Height / 2;

                    int arrowSize = 3;
                    var arrowPoints = new Point[]
                    {
            new Point(cx - arrowSize, cy - 1),
            new Point(cx, cy + 2),
            new Point(cx + arrowSize, cy - 1)
                    };

                    using (var pen = new Pen(_colors.TextBoxFore, 1.5f))
                    {
                        g.DrawLines(pen, arrowPoints);
                    }

                    // -- C. PINTAR EL TEXTO MANUALMENTE (NUEVO) --
                    // Aquí es donde "tapamos" el texto nativo y dibujamos el nuestro con el color deseado.

                    // 1. Definimos el área donde va el texto (todo el control menos el botón)
                    var textArea = new Rectangle(0, 0, rect.Right - nativeButtonWidth - overlapFix, rect.Height);

                    // 2. Limpiamos el fondo del área de texto (opcional, pero recomendado para evitar "fantasmas")
                    using (var backBrush = new SolidBrush(_colors.TextBoxBack))
                    {
                        g.FillRectangle(backBrush, textArea);
                    }

                    // 3. Dibujamos el texto
                    // Usamos TextRenderer para mejor calidad de texto en WinForms, o g.DrawString.
                    // TextRenderer suele alinearse mejor con el renderizado nativo de GDI.
                    TextRenderer.DrawText(g,
                        "  " + _dtp.Text, // Agregamos espacios para margen izquierdo
                        _dtp.Font,
                        textArea,
                        _colors.TextBoxFore, // ¡Aquí usas tu color personalizado!
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.SingleLine);

                    // Definimos el área de texto

                    // 1. Limpiamos el fondo
                    using (var backBrush = new SolidBrush(_colors.TextBoxBack))
                    {
                        g.FillRectangle(backBrush, textArea);
                    }

                    // 2. Creamos la fuente en Italic basada en la original
                    // Usamos 'using' porque las fuentes son recursos GDI que deben liberarse
                    using (var italicFont = new Font(_dtp.Font, FontStyle.Italic))
                    {
                        // 3. Dibujamos usando TextSecondary
                        TextRenderer.DrawText(g,
                            " " + _dtp.Text, // Pequeño espacio extra a la izq
                            italicFont,      // <--- Usamos la fuente Italic
                            textArea,
                            _colors.TextSecondary, // <--- Usamos el color secundario de la paleta
                            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.PreserveGraphicsClipping);
                    }

                    // -- D. DIBUJAR BORDE PLANO --
                    using (var pen = new Pen(_colors.BorderColor))
                    {
                        var borderRect = new Rectangle(0, 0, rect.Width - 1, rect.Height - 1);
                        g.DrawRectangle(pen, borderRect);
                    }
                }
            }
        }

        #endregion

        #region Helpers Existentes (Sin cambios)

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