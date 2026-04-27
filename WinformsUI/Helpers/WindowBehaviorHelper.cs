using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinformsUI.Helpers
{
    public static class WindowBehaviorHelper
    {
        #region === Constantes Windows API ===
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;

        // Redimensionamiento
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        #endregion

        /// <summary>
        /// Aplica comportamiento completo de ventana (Arrastre, Doble Clic, y Botones).
        /// </summary>
        public static void EnableWindowBehaviors
        (
            Form form,
            Control titleBar,
            Button btnClose = null,
            Button btnMaximize = null,
            Button btnMinimize = null,
            Action onExpandContract = null, // Inyectamos la lógica custom si la hay
            Action onMinimize = null,
            Action onClose = null)
        {
            // 1. Habilitar Arrastre y Doble Clic
            if (titleBar != null)
            {
                EnableDragOnly(form, titleBar);

                titleBar.MouseDoubleClick += (s, e) =>
                {
                    if (e.Button == MouseButtons.Left)
                        ExecuteExpandContract(form, onExpandContract);
                };
            }

            // 2. Eventos de Botones
            if (btnClose != null)
                btnClose.Click += (s, e) => { if (onClose != null) onClose(); else form.Close(); };

            if (btnMaximize != null)
                btnMaximize.Click += (s, e) => ExecuteExpandContract(form, onExpandContract);

            if (btnMinimize != null)
                btnMinimize.Click += (s, e) => { if (onMinimize != null) onMinimize(); else form.WindowState = FormWindowState.Minimized; };
        }

        /// <summary>
        /// Sobrecarga ligera: Solo habilita arrastrar la ventana usando un control.
        /// </summary>
        public static void EnableDragOnly(Form form, Control dragArea)
        {
            dragArea.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    // Evita problemas si el formulario está custom-maximizado en tu contenedor
                    ReleaseCapture();
                    SendMessage(form.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };
        }

        /// <summary>
        /// Helper para limpiar el WndProc del formulario. Calcula los bordes para redimensionar.
        /// Retorna true si procesó el mensaje, false si debe continuar el flujo normal.
        /// </summary>
        public static bool TryHandleResize(Form form, ref Message m, int resizeBorder = 10, bool isCustomMaximized = false)
        {
            if (m.Msg == WM_NCHITTEST)
            {
                // Si está maximizado en tu contenedor, no permitimos redimensionar
                if (isCustomMaximized) return false;

                Point screenPoint = new Point(m.LParam.ToInt32());
                Point clientPoint = form.PointToClient(screenPoint);

                if (clientPoint.Y <= resizeBorder)
                {
                    if (clientPoint.X <= resizeBorder) m.Result = (IntPtr)HTTOPLEFT;
                    else if (clientPoint.X >= (form.Size.Width - resizeBorder)) m.Result = (IntPtr)HTTOPRIGHT;
                    else m.Result = (IntPtr)HTTOP;
                    return true;
                }
                else if (clientPoint.Y >= (form.Size.Height - resizeBorder))
                {
                    if (clientPoint.X <= resizeBorder) m.Result = (IntPtr)HTBOTTOMLEFT;
                    else if (clientPoint.X >= (form.Size.Width - resizeBorder)) m.Result = (IntPtr)HTBOTTOMRIGHT;
                    else m.Result = (IntPtr)HTBOTTOM;
                    return true;
                }
                else if (clientPoint.X <= resizeBorder)
                {
                    m.Result = (IntPtr)HTLEFT;
                    return true;
                }
                else if (clientPoint.X >= (form.Size.Width - resizeBorder))
                {
                    m.Result = (IntPtr)HTRIGHT;
                    return true;
                }
            }
            return false;
        }

        private static void ExecuteExpandContract(Form form, Action customAction)
        {
            if (customAction != null)
            {
                customAction(); // Usa tu lógica de "llenar el panel contenedor"
            }
            else
            {
                form.WindowState = form.WindowState == FormWindowState.Normal
                    ? FormWindowState.Maximized
                    : FormWindowState.Normal;
            }
        }
    }
}

