using BLL.DTOs;
using Presenter.Presenters.ForClient;
using Shared;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Infrastructure.Shortcuts;
using WinformsUI.Infrastructure.Translations;
using static Winforms.Theme.DarkTheme;

namespace WinformsUI.Forms.ClientCRUDL
{  
    public partial class UpdateClientForm : Form, IUpdateClientView
    {
        public event EventHandler<ClientDTO> UpdateClientRequested;
        public event EventHandler MinimizeRequested;
        public event EventHandler CloseRequested;

        private List<Button> _editButtons;
        private Dictionary<TextBox, string> _cacheOriginalValues;
        private Palette _internalPalette;
        private ClientDTO _client;

        private string _confirmChanges = "¿Está seguro de que desea confirmar los siguientes cambios?";
        private string _title = "Actualizar Cliente";
        private string _confirmChangesTitle = "Confirmar Modificación";
        private string _successMsg = "";
        private string _errorMsg = "";
        
        private readonly ITranslatableControlsManager _transMgr;
        private readonly IApplicationSettings _appSettings;
        private ShortcutManager _shortcutMgr;

        public UpdateClientForm
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
            SetShortcuts();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Si el ShortcutManager logra manejar la tecla, retornamos true para "consumirla"
            if (_shortcutMgr.TryHandle(keyData))
            {
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void SetShortcuts() => _shortcutMgr = ShortcutManager.Attach(this)
                                        .Add("Esc", () => MessageBox.Show("I WAS LIVING FOR A DREAM"))
                                        .Add("Ctrl+W", () => MessageBox.Show("LOVING FOR A MOMENT"))
                                        .Add("Ctrl+H", () => MessageBox.Show("HAT AWS JUST MY STYLE"));

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
            _internalPalette = DarkTheme.IsDarkPalette(DarkTheme.GetCurrentPalette()) ? DarkTheme.PalettesDark.Obsidian() : DarkTheme.PalettesLight.Classic();
            ApplyGradientBackground(tlpTitleBar, Darken(_internalPalette.LowAccent, 0.65), Darken(_internalPalette.Accent, 0.65), LinearGradientMode.Vertical, false);
            ApplyGradientBackground(pnlBackground, Darken(_internalPalette.Accent, 0.65), Darken(_internalPalette.LowAccent, 0.65), LinearGradientMode.Vertical, false);
        }

        private void AddTranslatables()
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<Button>(this.Controls, "Text");

            _transMgr.AddString("UpdateClientForm._confirmChanges", _confirmChanges);
            _transMgr.AddString("UpdateClientForm._title", _title);
            _transMgr.AddString("UpdateClientForm._confirmChangesTitle", _confirmChangesTitle);
            _transMgr.AddString("UpdateClientForm.AppSettings._successMsg", _successMsg);
            _transMgr.AddString("UpdateClientForm.AppSettings._errorMsg", _errorMsg);

            _transMgr.AddFormNotify(this);
        }

        public void NotifiedByTranslationManager() => ApplyTranslation();

        private void ApplyTranslation()
        {
            _confirmChanges = _transMgr.GetString("UpdateClientForm._confirmChanges");
            _confirmChangesTitle = _transMgr.GetString("UpdateClientForm._confirmChangesTitle");
            _title = _transMgr.GetString("UpdateClientForm._title");
            _successMsg = _transMgr.GetString("UpdateClientForm.AppSettings._successMsg");
            _errorMsg = _transMgr.GetString("UpdateClientForm.AppSettings._errorMsg");

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
                btnEditName, btnEditLastName, btnEditDocType, btnEditDocNumber,
                btnEditShipAddress, btnEditShipCountry, btnEditShipState,
                btnEditZipCode, btnEditEmail, btnEditPhone, btnEditObservations
            };

            btnEditName.Tag = txtName;
            btnEditLastName.Tag = txtLastName;
            btnEditDocType.Tag = txtDocType;
            btnEditDocNumber.Tag = txtDocNumber;
            btnEditShipAddress.Tag = txtShipAddress;
            btnEditShipCountry.Tag = txtShipCountry;
            btnEditShipState.Tag = txtShipState;
            btnEditZipCode.Tag = txtZipCode;
            btnEditEmail.Tag = txtEmail;
            btnEditPhone.Tag = txtPhone;
            btnEditObservations.Tag = txtObservations;
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
        public void SetClientData(ClientDTO dto)
        {
            _client = dto;

            txtName.Text = dto.Name?.ToString();
            txtLastName.Text = dto.LastName?.ToString();
            txtDocType.Text = dto.TaxId?.ToString();
            txtDocNumber.Text = dto.DocNumber?.ToString();
            txtShipAddress.Text = dto.ShipAddress?.ToString();
            txtShipCountry.Text = dto.ShipCountry?.ToString();
            txtShipState.Text = dto.ShipState?.ToString();
            txtZipCode.Text = dto.ShipZipCode?.ToString();
            txtEmail.Text = dto.Email?.ToString();
            txtPhone.Text = dto.Phone?.ToString();
            txtObservations.Text = dto.Observations?.ToString();
        }

        private void OnConfirmClick(object sender, EventArgs e)
        {
            if (ValidateChanges())
            {
                btnConfirm.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                UpdateClientRequested?.Invoke(this, MapNewData());
                this.DialogResult = DialogResult.OK;
                CloseRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private ClientDTO MapNewData()
        {
            return new ClientDTO()
            {
                Id = _client.Id,
                Name = txtName.Text,
                LastName = txtLastName.Text,
                TaxId = txtDocType.Text,
                DocNumber = txtDocNumber.Text,
                ShipAddress = txtShipAddress.Text,
                ShipCountry = txtShipCountry.Text,
                ShipState = txtShipState.Text,
                ShipZipCode = txtZipCode.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                Observations = txtObservations.Text
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
        public void ShowOperationResult(OperationResult<ClientDTO> opRes)
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
                UpdateClientRequested = null;
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