using BLL.DTOs;
using Presenter.ForClient;
using Shared;
using SharedAbstractions.Enums;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;
using Winforms.Theme;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls.Wizard;

namespace WinformsUI.Forms.ClientCRUDL
{
    public partial class CreateClientForm : Form, ICreateClientView
    {
        private readonly IApplicationSettings _appSettings;
        private readonly IWizardPanelNavigator _wizard;
        private readonly ITranslatableControlsManager _transMgr;
        private readonly string _errorMsg;
        private readonly string _successMsg;

        public event EventHandler<ClientDTO> CreateClientRequested;



        public CreateClientForm
        (
            IWizardPanelNavigator wizard,
            IApplicationSettings appSettings,
            ITranslatableControlsManager transMgr
        )
        {
            _wizard = wizard;
            _appSettings = appSettings;

            _successMsg = _appSettings.SuccessOnOperation;
            _errorMsg = _appSettings.ErrorOnOperation;
            _transMgr = transMgr;

            InitializeComponent();

            InitializeWizard();
            WireFlowButtonsEvents();
            UpdateFormSize();
            ApplyGlobalPalette();
            AddTranslatables();


        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                btnBackShipAddress.Click -= BackWizard;
                btnBackContact.Click -= BackWizard;
                btnNextId.Click -= AdvanceWizard;
                btnNextShip.Click -= AdvanceWizard;
                btnFinish.Click -= ExecuteCreation;

                _transMgr.RemoveFormNotify(this);
                CreateClientRequested = null;

                if (components != null)
                    components.Dispose();
            }

            base.Dispose(disposing);
        }

        public void FillClientDocTypes(IEnumerable<object> docTypes)
        {
            cbTaxId.ValueMember = "Id";
            cbTaxId.DisplayMember = "Display";

            cbTaxId.DataSource = docTypes;
        }
        public void FillCountries(IEnumerable<object> countries)
        {
            cbCountrySelector.ValueMember = "Id";
            cbCountrySelector.DisplayMember = "Display";

            cbCountrySelector.DataSource = countries;
        }



        private void AddTranslatables()
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<Button>(this.Controls, "Text");
            _transMgr.AddFormNotify(this);
        }

        public void ApplyTranslation() => _transMgr.Apply();

        public void ApplyGlobalPalette()
        {
            DarkTheme.RedrawBorders = false;
            DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());
        }
        private void UpdateFormSize()
        {
            if (_wizard.CurrentPanel.Size != null)
                this.ClientSize = _wizard.CurrentPanel.Size;
        }

        public void ShowOperationResult(OperationResult<ClientDTO> opRes)
        {
            this.Cursor = Cursors.Default;
            btnFinish.Enabled = !opRes.Success;

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

        private void InitializeWizard() => _wizard.Initialize(new Panel[] { pnlIdentification, pnlShipAddress, pnlContact });

        private void WireFlowButtonsEvents()
        {
            // --- NAVEGACIÓN HACIA ATRÁS ---
            btnBackShipAddress.Click += BackWizard;
            btnBackContact.Click += BackWizard;

            // --- NAVEGACIÓN HACIA ADELANTE ---
            btnNextId.Click += AdvanceWizard;
            btnNextShip.Click += AdvanceWizard;

            // --------- PASO FINAL ---------
            btnFinish.Click += ExecuteCreation;
        }

        private void AdvanceWizard(object sender, EventArgs e)
        {
            _wizard.Advance();
            UpdateFormSize();
        }

        private void BackWizard(object sender, EventArgs e)
        {
            _wizard.Back();
            UpdateFormSize();
        }

        private void ExecuteCreation(object sender, EventArgs e)
        {
            var dto = new ClientDTO
            {
                Name = txtName.Text,
                LastName = txtLastName.Text,
                TaxId = cbTaxId.Text,
                DocNumber = txtDocNumber.Text,
                ShipCountry = cbCountrySelector.Text,
                ShipState = tbState.Text,
                ShipAddress = txtShipAddreess.Text,
                ShipZipCode = cbZipCode.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                Observations = txtObservations.Text
            };

            btnFinish.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            CreateClientRequested?.Invoke(this, dto);
        }

    }
}