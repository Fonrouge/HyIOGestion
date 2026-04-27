using BLL.DTOs;
using Presenter.LoginScreen;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Helpers;
using static Winforms.Theme.DarkTheme;

namespace WinformsUI.Forms.Login
{
    public partial class LoginFrm : Form, ILoginView, IDisposable
    {
        // A futuro de archivo de recursos
        private string _userPlaceholder = "Usuario...";
        private string _passwordPlaceholder = "Contraseña...";
        private string _buttonText = "Ingresar";
        private string _windowTitle = "Bienvenido";
        private string _charging = "Cargando...";

        private Palette _currentPalette;

        // Eventos: De la Vista hacia el Presenter          
        public event Func<object, (string username, string password), Task> LoginRequested;

        public event EventHandler CloseRequested;
        public event EventHandler MinimizeRequested;
        public event EventHandler ContractRequested;

        public LoginFrm()
        {
            InitializeComponent();

            ApplyTheme();
            SetControls();
            WireEvents();

            tbUser.Focus();
        }


        private void ApplyTheme()
        {
            _currentPalette = DarkTheme.GetCurrentPalette();
            DarkTheme.RedrawBorders = true;
            DarkTheme.Apply(this, _currentPalette);
        }

        private void SetControls()
        {
            tbUser.Text = _userPlaceholder;
            tbPass.Text = _passwordPlaceholder;
            btnEnter.Text = _buttonText;
            this.Text = _windowTitle;

            ApplyPlaceholder(tbUser);
            ApplyPlaceholder(tbPass);
        }

        private void ApplyPlaceholder(TextBox tb)
        {
            TextBoxPlaceholder.WirePlaceholderBehavior(tb, tb.Text, false, _currentPalette.TextPrimary, _currentPalette.TextSecondary);
            TextBoxPlaceholder.Apply(tb, true);
        }

        private void WireEvents()
        {
            btnEnter.Click += (s, e) =>
            {
                LoginRequested?.Invoke(this, (tbUser.Text, tbPass.Text));
            };

            // Aquí también deberías enlazar los botones de la barra de título a tus eventos OnCloseRequested, etc.
        }

        // Método: Del Presenter hacia la Vista
        public void SetLoadingState(bool isLoading)
        {
            btnEnter.Enabled = !isLoading;
            tbUser.Enabled = !isLoading;
            tbPass.Enabled = !isLoading;

            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
            
            btnEnter.Text = isLoading ? _charging : _buttonText;
        }

        bool ILoginView.ShowDialog()
        {
            // Corregido: Una sola llamada evaluada directamente
            return this.ShowDialog() == DialogResult.OK;
        }

        public void ShowOperationResult(OperationResult<UsuarioDTO> result)
        {
            if (!result.Success)
            {
                // Unimos todos los errores en un solo texto con saltos de línea
                string errorMessage = string.Join("\n\n", result.Errors.Select(v => $"Error: {v.InformativeMessage}\nAcción: {v.RecommendedAction}"));
                MessageBox.Show(errorMessage, "Error de autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Feedback opcional
                // MessageBox.Show("¡Bienvenido al sistema!", "Autenticación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // CRUCIAL: Si es exitoso, marcamos el form como OK para que se cierre y devuelva true al punto de entrada
                this.DialogResult = DialogResult.OK;
            }
        }

    }
}