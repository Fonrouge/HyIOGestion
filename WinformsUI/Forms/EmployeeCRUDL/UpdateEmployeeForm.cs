using BLL.DTOs;
using Presenter.ForEmployee;
using Shared;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Infrastructure.Translations;
using static Winforms.Theme.DarkTheme;

namespace WinformsUI.Forms.EmployeeCRUDL
{
    public partial class UpdateEmployeeForm : Form, IUpdateEmployeeView
    {
        public event EventHandler<EmployeeDTO> UpdateEmployeeRequested;
        public event EventHandler MinimizeRequested;
        public event EventHandler CloseRequested;

        private List<Button> _editButtons;
        private Dictionary<TextBox, string> _cacheOriginalValues;
        private Palette _internalPalette;
        private EmployeeDTO _employee;

        private string _confirmChanges = "¿Está seguro de que desea confirmar los siguientes cambios?";
        private string _title = "Actualizar Empleado";
        private string _confirmChangesTitle = "Confirmar Modificación";
        private string _successMsg = "";
        private string _errorMsg = "";


        private readonly ITranslatableControlsManager _transMgr;
        private readonly IApplicationSettings _appSettings;

        public UpdateEmployeeForm
        (
            ITranslatableControlsManager transMgr,
            IApplicationSettings appSettings
        )
        {
            _transMgr = transMgr;
            _cacheOriginalValues = new Dictionary<TextBox, string>();
            _appSettings = appSettings;
            _successMsg = _appSettings.SuccessOnOperation;
            _errorMsg = _appSettings.ErrorOnOperation;

            InitializeComponent();
            ThemingNotifiedByConfigurationsModule();
            AddTranslatables();
            InitEditButtonsList();
            SetInitialControlsState();
            WireEvents();
        }

        // =================================================================
        // SUSCRIPCIÓN DE EVENTOS NO ANÓNIMOS (para desuscribir al cerrar)
        // =================================================================
        private void WireEvents()
        {
            _editButtons.ForEach(b => b.Click += SwitchButtonState);
            btnConfirm.Click += OnConfirmClick;
            btnClose.Click += CloseRequest;
            btnMinimize.Click += MinimizeRequest;
        }


        // =========================================================
        // TRADUCCIONES Y PALETA
        // =========================================================
        private void ThemingNotifiedByConfigurationsModule()
        {
            DarkTheme.RedrawBorders = true;
            DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());
            _internalPalette = DarkTheme.IsDarkPalette(DarkTheme.GetCurrentPalette()) ? DarkTheme.PalettesDark.Nordic() : DarkTheme.PalettesLight.Paper();
            ApplyGradientBackground(tlpTitleBar, Darken(_internalPalette.LowAccent, 0.65), Darken(_internalPalette.Accent, 0.65), LinearGradientMode.Vertical, false);
            ApplyGradientBackground(pnlBackground, Darken(_internalPalette.Accent, 0.65), Darken(_internalPalette.LowAccent, 0.65), LinearGradientMode.Vertical, false);
        }

        private void AddTranslatables()
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<Button>(this.Controls, "Text");

            _transMgr.AddString("UpdateEmployeeForm._confirmChanges", _confirmChanges);
            _transMgr.AddString("UpdateEmployeeForm._title", _title);
            _transMgr.AddString("UpdateEmployeeForm._confirmChangesTitle", _confirmChangesTitle);
            _transMgr.AddString("UpdateEmployeeForm.AppSettings._successMsg", _successMsg);
            _transMgr.AddString("UpdateEmployeeForm.AppSettings._errorMsg", _errorMsg);

            _transMgr.AddFormNotify(this);
        }

        public void NotifiedByTranslationManager() => ApplyTranslation();

        private void ApplyTranslation()
        {
            _confirmChanges = _transMgr.GetString("UpdateEmployeeForm._confirmChanges");
            _confirmChangesTitle = _transMgr.GetString("UpdateEmployeeForm._confirmChangesTitle");
            _title = _transMgr.GetString("UpdateEmployeeForm._title");
            _successMsg = _transMgr.GetString("UpdateEmployeeForm.AppSettings._successMsg");
            _errorMsg = _transMgr.GetString("UpdateEmployeeForm.AppSettings._errorMsg");

            lblTitle.Text = _title;

            _transMgr.Apply();
        }

        // =========================================================
        // LÓGICA ESPECÍFICA DEL FORM Y CONTROLES
        // =========================================================
        private void InitEditButtonsList()
        {
            _editButtons = new List<Button>
            {
                btnEditFirstName,
                btnEditLastName,
                btnEditNationalId,
                btnEditFileNumber,
                btnEditEmail,
                btnPhoneNumber,
                btnEditHomeAddress
            };

            btnEditFirstName.Tag = txtFirstName;
            btnEditLastName.Tag = txtLastName;
            btnEditNationalId.Tag = txtNationalId;
            btnEditFileNumber.Tag = txtFileNumber;
            btnEditEmail.Tag = txtEmail;
            btnPhoneNumber.Tag = txtPhoneNumber;
            btnEditHomeAddress.Tag = txtHomeAddress;
        }

        private void SetInitialControlsState()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox tb)
                {
                    tb.Enabled = false;
                    tb.ReadOnly = true;
                }
            }
            lblTitle.Text = $"{_title}";
        }

        private void SwitchButtonState(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is TextBox tb)
            {
                if (tb.ReadOnly && !_cacheOriginalValues.ContainsKey(tb))
                {
                    _cacheOriginalValues.Add(tb, tb.Text);
                }

                tb.Enabled = !tb.Enabled;
                tb.ReadOnly = !tb.ReadOnly;
                if (!tb.ReadOnly) tb.Focus();
            }
        }


        // =========================================================
        // LÓGICA ESPECÍFICA DE VENTANA DIALOG
        // =========================================================
        private void CloseRequest(object sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
        private void MinimizeRequest(object sender, EventArgs e) => MinimizeRequested?.Invoke(this, EventArgs.Empty);

        public void MinimizeView() => this.WindowState = FormWindowState.Minimized;
        public void CloseView() => this.Close();



        // =========================================================
        // DATOS DE ENTRADA + DATOS DE SALIDA
        // =========================================================
        public void SetEmployeeData(EmployeeDTO dto)
        {
            _employee = dto;

            txtFirstName.Text = dto.FirstName?.ToString();
            txtLastName.Text = dto.LastName?.ToString();
            txtNationalId.Text = dto.NationalId?.ToString();
            txtFileNumber.Text = dto.FileNumber?.ToString();
            txtEmail.Text = dto.Email?.ToString();
            txtPhoneNumber.Text = dto.PhoneNumber?.ToString();
            txtHomeAddress.Text = dto.HomeAddress?.ToString();
        }

        private void OnConfirmClick(object sender, EventArgs e)
        {
            if (ValidateChanges())
            {
                btnConfirm.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                UpdateEmployeeRequested?.Invoke(this, MapNewData());
                this.DialogResult = DialogResult.OK;
                CloseRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private EmployeeDTO MapNewData()
        {
            return new EmployeeDTO()
            {
                Id          = _employee.Id,
                FirstName   = txtFirstName  .Text,
                LastName    = txtLastName   .Text,
                NationalId  = txtNationalId .Text,
                FileNumber  = txtFileNumber .Text,
                Email       = txtEmail      .Text,
                PhoneNumber = txtPhoneNumber.Text, 
                HomeAddress = txtHomeAddress.Text
            };
        }

        private bool ValidateChanges()
        {
            StringBuilder sb = new StringBuilder();
            bool hasChanges = false;

            foreach (var item in _cacheOriginalValues)
            {
                TextBox tb = item.Key;
                string valorOriginal = item.Value;
                string valorNuevo = tb.Text;

                if (valorOriginal != valorNuevo)
                {
                    hasChanges = true;
                    string fieldName = tb.Name.Replace("txt", "");
                    sb.AppendLine($"{fieldName}: '{valorOriginal}' -> '{valorNuevo}'");
                }
            }

            if (!hasChanges) return true;

            string mensaje = $"{_confirmChanges}\n\n" + sb.ToString();
            var result = MessageBox.Show(mensaje, $"{_confirmChangesTitle}", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            return result == DialogResult.Yes;
        }


        // =========================================================
        // INFORMACIÓN AL USUARIO
        // =========================================================
        public void ShowOperationResult(OperationResult<EmployeeDTO> opRes)
        {
            this.Cursor = Cursors.Default;
            btnConfirm.Enabled = !opRes.Success;

            if (opRes.Success)
            {
                MessageBox.Show(_successMsg, "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                string errors = string.Join(Environment.NewLine, opRes.Errors);
                MessageBox.Show($"{_errorMsg}{Environment.NewLine}{errors}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // =========================================================
        // CUIDADO DE MEMORIA RAM
        // =========================================================
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_editButtons != null)
                {
                    _editButtons.ForEach(b => b.Click -= SwitchButtonState);
                    _editButtons.Clear();
                    _editButtons = null;
                }

                if (_cacheOriginalValues != null)
                {
                    _cacheOriginalValues.Clear();
                    _cacheOriginalValues = null;
                }

                btnConfirm.Click -= OnConfirmClick;
                btnClose.Click -= CloseRequest;
                btnMinimize.Click -= MinimizeRequest;
                UpdateEmployeeRequested = null;
                MinimizeRequested = null;
                CloseRequested = null;
                _transMgr.RemoveFormNotify(this);

                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}