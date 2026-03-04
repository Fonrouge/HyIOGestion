using System;
using System.Drawing;
using System.Windows.Forms;

namespace Presentation.WinForms.Services.Implementations.TextBoxes
{
    public static class TextBoxPlaceholder  //ESTE SE USA PARA EL LOGIN
    {
        #region Campos Privados

        // Nota: BaseFont sigue definiendo la FAMILIA por defecto (Microsoft Tai Le), 
        // pero el tamaño podrá ser sobrescrito por el parámetro.
        private static readonly Font BaseFont = null;// new Font("Microsoft Tai Le", 12f, FontStyle.Bold);
        private static readonly Color DefaultPlaceholderColor = SystemColors.GrayText;

        #endregion

        #region API Pública

        public static void WirePlaceholderBehavior(
            TextBox tb,
            string placeholderText,
            bool isPassword,
            Color? normalForeColor = null,
            Color? placeholderForeColor = null,
            float? fontSize = null) // <--- 1. Nuevo parámetro opcional
        {
            if (tb == null) return;

            void refreshState()
            {
                if (!tb.IsHandleCreated) return;

                bool isPh = string.IsNullOrEmpty(tb.Text) || tb.Text == placeholderText;

                if (isPh && !tb.Focused)
                {
                    tb.Text = placeholderText;
                    // Pasamos el fontSize aquí
                    Apply(tb, true, isPassword, normalForeColor, placeholderForeColor, fontSize);
                }
                else
                {
                    if (tb.Text == placeholderText) tb.Text = "";
                    // Y aquí también
                    Apply(tb, false, isPassword, normalForeColor, placeholderForeColor, fontSize);
                }
            }

            void safeRefresh(object sender, EventArgs e)
            {
                if (tb.IsDisposed || !tb.IsHandleCreated) return;

                tb.BeginInvoke(new Action(() =>
                {
                    if (!tb.IsDisposed) refreshState();
                }));
            }

            tb.GotFocus += safeRefresh;
            tb.LostFocus += safeRefresh;

            if (string.IsNullOrEmpty(tb.Text))
            {
                tb.Text = placeholderText;
            }
            refreshState();
        }

        public static void Apply(
            TextBox tb,
            bool isPlaceholder,
            bool isPassword = false,
            Color? normalForeColor = null,
            Color? placeholderForeColor = null,
            float? fontSize = null) // <--- 2. Nuevo parámetro opcional
        {
            if (tb == null) return;

            // 1. TIPOGRAFÍA
            var targetFontStyle = isPlaceholder ? FontStyle.Italic : FontStyle.Regular;

            // LÓGICA DE PRIORIDAD DE TAMAÑO:
            // 1. Si pasaron un fontSize explícito, úsalo.
            // 2. Si no, usa el de BaseFont (12f).
            // 3. Si BaseFont fuera nulo, usa el del TextBox actual.
            float finalSize = fontSize ?? BaseFont?.Size ?? tb.Font.Size;

            // Lógica de Familia (Mantiene "Microsoft Tai Le" si existe BaseFont, sino usa la del TB)
            var family = BaseFont?.FontFamily ?? tb.Font.FontFamily;

            // Solo aplicamos si cambió el estilo O el tamaño es diferente al actual
            // (La comparación de punto flotante debe tener un pequeño margen, pero != suele bastar en GDI+)
            if (tb.Font.Style != targetFontStyle || Math.Abs(tb.Font.Size - finalSize) > 0.01)
            {
                tb.Font = new Font(family, finalSize, targetFontStyle);
            }

            // 2. COLOR
            var targetColor = isPlaceholder
                ? (placeholderForeColor ?? DefaultPlaceholderColor)
                : (normalForeColor ?? SystemColors.WindowText);

            if (tb.ForeColor != targetColor)
            {
                tb.ForeColor = targetColor;
            }

            // 3. CONTRASEÑA
            if (isPassword)
            {
                char targetChar = isPlaceholder ? '\0' : '●';
                bool targetSystemChar = !isPlaceholder;

                if (tb.PasswordChar != targetChar)
                    tb.PasswordChar = targetChar;

                if (tb.UseSystemPasswordChar != targetSystemChar)
                    tb.UseSystemPasswordChar = targetSystemChar;
            }
            else
            {
                if (tb.UseSystemPasswordChar) tb.UseSystemPasswordChar = false;
                if (tb.PasswordChar != '\0') tb.PasswordChar = '\0';
            }

            // 4. NAVEGACIÓN
            if (tb.AcceptsTab) tb.AcceptsTab = false;
            if (!tb.ShortcutsEnabled) tb.ShortcutsEnabled = true;

            tb.KeyDown -= EnsureTabNavigates;
            tb.KeyDown += EnsureTabNavigates;
        }

        #endregion

        #region Helpers Privados

        private static void EnsureTabNavigates(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.Handled = false;
                e.SuppressKeyPress = false;
            }
        }

        #endregion
    }
}