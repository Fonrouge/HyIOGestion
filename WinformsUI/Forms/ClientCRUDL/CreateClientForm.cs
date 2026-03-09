using BLL.DTOs;
using Presenter.ForClient;
using Shared;
using System;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.UserControls.Wizard;
using WinformsUI.Infrastructure.Translations;

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

            ApplyDarkTheme();
            InitializeWizard();
            WireFlowButtonsEvents();
            UpdateFormSize();
            ApplyGlobalPalette();
            AddTranslatables();

            this.FormClosed += (s, e) =>
            {
                _transMgr.RemoveFormNotify(this);

            };
        }


        private void AddTranslatables()
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<Button>(this.Controls, "Text");
            _transMgr.AddFormNotify(this);
        }

        public void ApplyTranslation() => _transMgr.Apply();

        public void ApplyGlobalPalette() => DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());

        private void UpdateFormSize()
        {
            if (_wizard.CurrentPanel.Size != null)
                this.ClientSize = _wizard.CurrentPanel.Size;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CreateClientRequested = null;

                if (components != null)
                    components.Dispose();
            }

            base.Dispose(disposing);
        }

        public void ShowOperationResult(OperationResult<ClientDTO> opRes)
        {
            MessageBox.Show(opRes.Success ? $"{_successMsg}" : $"{_errorMsg}. Errors: {string.Join(", ", opRes.Errors)}");
            this.Close();
        }
        private void ApplyDarkTheme()
        {
            DarkTheme.RedrawBorders = true;
            DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());
        }

        private void InitializeWizard() => _wizard.Initialize(new Panel[] { pnlIdentification, pnlShipAddress, pnlContact });

        private void WireFlowButtonsEvents()
        {
            // --- NAVEGACIÓN HACIA ATRÁS ---
            btnBackShipAddress.Click += (s, e) =>
            {
                _wizard.Back();
                UpdateFormSize();
            };

            btnBackContact.Click += (s, e) =>
            {
                _wizard.Back();
                UpdateFormSize();
            };


            // --- NAVEGACIÓN HACIA ADELANTE ---
            btnNextId.Click += (s, e) =>
            {
                _wizard.Advance();
                UpdateFormSize();
            };

            btnNextShip.Click += (s, e) =>
            {
                _wizard.Advance();
                UpdateFormSize();
            };

            btnFinish.Click += (s, e) => ExecuteCreation();
        }

        private void ExecuteCreation()
        {
            var dto = new ClientDTO
            {
                Name = txtName.Text,
                LastName = txtLastName.Text,
                TaxId = cbTaxId.Text,
                DocNumber = txtDocNumber.Text,
                ShipCountry = cbCountrySelector.Text,
                ShipState = cbStateSelector.Text,
                ShipAddress = txtShupAddreess.Text,
                ShipZipCode = cbZipCode.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                Observations = txtObservations.Text
            };

            CreateClientRequested?.Invoke(this, dto);

        }

    }
}