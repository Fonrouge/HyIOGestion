using BLL.DTOs;
using Presenter.Presenters.ForSupplier;
using Shared;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Infrastructure.Translations;
using static Winforms.Theme.DarkTheme;

namespace WinformsUI.Forms.SupplierCRUDL
{
    public partial class UpdateSupplierForm : Form, IUpdateSupplierView
    {
        public event EventHandler<SupplierDTO> UpdateSupplierRequested;
        public event EventHandler MinimizeRequested;
        public event EventHandler CloseRequested;

        private List<Button> _editButtons;
        private Dictionary<TextBox, string> _cacheOriginalValues;
        private Palette _internalPalette;
        private SupplierDTO _supplier;

        private string _confirmChanges = "¿Está seguro de que desea confirmar los siguientes cambios?";
        private string _title = "Actualizar Proveedor";
        private string _confirmChangesTitle = "Confirmar Modificación";
        private string _successMsg = "";
        private string _errorMsg = "";


        private readonly ITranslatableControlsManager _transMgr;
        private readonly IApplicationSettings _appSettings;

        public UpdateSupplierForm
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
            ApplyGlobalPalette();
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
        private void ApplyGlobalPalette()
        {
            DarkTheme.RedrawBorders = true;
            DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());
            _internalPalette = DarkTheme.IsDarkPalette(DarkTheme.GetCurrentPalette()) ? DarkTheme.PalettesDark.Oceanic() : DarkTheme.PalettesLight.Solar();
            ApplyGradientBackground(tlpTitleBar, Darken(_internalPalette.LowAccent, 0.65), Darken(_internalPalette.Accent, 0.65), LinearGradientMode.Vertical, false);
            ApplyGradientBackground(pnlBackground, Darken(_internalPalette.Accent, 0.65), Darken(_internalPalette.LowAccent, 0.65), LinearGradientMode.Vertical, false);
        }

        private void AddTranslatables()
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<Button>(this.Controls, "Text");

            _transMgr.AddString("UpdateSupplierForm._confirmChanges", _confirmChanges);
            _transMgr.AddString("UpdateSupplierForm._title", _title);
            _transMgr.AddString("UpdateSupplierForm._confirmChangesTitle", _confirmChangesTitle);
            _transMgr.AddString("UpdateSupplierForm.AppSettings._successMsg", _successMsg);
            _transMgr.AddString("UpdateSupplierForm.AppSettings._errorMsg", _errorMsg);

            _transMgr.AddFormNotify(this);
        }

        public void NotifiedByTranslationManager() => ApplyTranslation();

        private void ApplyTranslation()
        {
            _confirmChanges = _transMgr.GetString("UpdateSupplierForm._confirmChanges");
            _confirmChangesTitle = _transMgr.GetString("UpdateSupplierForm._confirmChangesTitle");
            _title = _transMgr.GetString("UpdateSupplierForm._title");
            _successMsg = _transMgr.GetString("UpdateSupplierForm.AppSettings._successMsg");
            _errorMsg = _transMgr.GetString("UpdateSupplierForm.AppSettings._errorMsg");

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
                btnEditCompanyName,
                btnEditContactName,
                btnEditTaxId,
                btnEditTaxNumber,
                btnEditMail,
                btnEditPhone,
                btnEditAddress,
                btnEditCity
            };

            btnEditCompanyName.Tag = txtCompanyName;
            btnEditContactName.Tag = txtContactName;
            btnEditTaxId.Tag = txtTaxId;
            btnEditTaxNumber.Tag = txtTaxNumber;
            btnEditMail.Tag = txtMail;
            btnEditPhone.Tag = txtPhone;
            btnEditAddress.Tag = txtAddress;
            btnEditCity.Tag = txtCity;
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
        public void SetSupplierData(SupplierDTO dto)
        {
            _supplier = dto;

            txtCompanyName.Text = dto.CompanyName.ToString();
            txtContactName.Text = dto.ContactName.ToString();
            txtTaxId.Text = dto.TaxId.ToString();
            txtTaxNumber.Text = dto.TaxNumber.ToString();
            txtMail.Text = dto.Mail.ToString();
            txtPhone.Text = dto.Phone.ToString();
            txtAddress.Text = dto.Address.ToString();
            txtCity.Text = dto.City.ToString();
        }

        private void OnConfirmClick(object sender, EventArgs e)
        {
            if (ValidateChanges())
            {
                btnConfirm.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                UpdateSupplierRequested?.Invoke(this, MapNewData());
                this.DialogResult = DialogResult.OK;
                CloseRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private SupplierDTO MapNewData()
        {
            return new SupplierDTO()
            {
                Id = _supplier.Id,
                CompanyName = txtCompanyName.Text,
                ContactName = txtContactName.Text,
                TaxId = txtTaxId.Text,
                TaxNumber = txtTaxNumber.Text,
                Mail = txtMail.Text,
                Phone = txtPhone.Text,
                Address = txtAddress.Text,
                City = txtCity.Text
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
        public void ShowOperationResult(OperationResult<SupplierDTO> opRes)
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
                UpdateSupplierRequested = null;
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