using BLL.DTOs;
using Presenter.ForClient;
using Shared;
using System;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.UserControls.Wizard;
using WinformsUI.Infrastructure.Translations;

namespace WinformsUI.Features.ClientCRUDL
{
    public partial class CreateClientForm : Form, ICreateClientView
    {
        private readonly IApplicationSettings _appSettings;
        private readonly IWizardPanelNavigator _wizard;
        private readonly ITranslatableControlsManager _transMgr;
        private readonly string _errorMsg;
        private readonly string _successMsg;

        public event EventHandler<ClientDTO> CreateClientRequested;

        private enum AddressOption
        {
            OnlyShipping = 0,
            OnlyPickUp = 1,
            Both = 2
        }
        private AddressOption _chosenAddressOption;

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


            //Para después hacer el OnClose(); ---> _transMgr.RemoveFormNotify(this);

         
            _transMgr.AddFormNotify(this);
        }

        public void ApplyTranslation()
        {
            throw new NotImplementedException();
        }

        public void ApplyGlobalPalette() => DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());

        private void UpdateFormSize()
        {
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
        private void InitializeWizard() => _wizard.Initialize(new Panel[] { pnlIdentification, pnlChooseDirections, pnlAddPickAddress, pnlShipAddress, pnlContact });

        private void WireFlowButtonsEvents()
        {
            // --- NAVEGACIÓN HACIA ATRÁS ---

            btnBackChooseAddress.Click += (s, e) =>
            {
                _wizard.GoTo(0);
                UpdateFormSize();
            };

            btnBackPickAddress.Click += (s, e) =>
            {
                _wizard.GoTo(1);
                UpdateFormSize();
            };

            btnBackShipAddress.Click += (s, e) =>
            {
                if (_chosenAddressOption == AddressOption.OnlyShipping)
                {
                    _wizard.GoTo(1);
                    UpdateFormSize();
                }
                else
                {
                    _wizard.Back();
                    UpdateFormSize();
                }
            };

            btnBackContact.Click += (s, e) =>
            {
                if (_chosenAddressOption == AddressOption.OnlyPickUp)
                {
                    _wizard.GoTo(2);
                    UpdateFormSize();
                }
                else
                {
                    _wizard.Back();
                    UpdateFormSize();
                }
            };

            // --- NAVEGACIÓN HACIA ADELANTE ---
            btnNextId.Click += (s, e) =>
            {
                _wizard.Advance();
                UpdateFormSize();
            };

            btnNextOnlyShip.Click += (s, e) =>
            {
                _chosenAddressOption = AddressOption.OnlyShipping;
                _wizard.GoTo(3);
                UpdateFormSize();
            };

            btnNextOnlyPick.Click += (s, e) =>
            {
                _chosenAddressOption = AddressOption.OnlyPickUp;
                _wizard.GoTo(2);
                UpdateFormSize();
            };

            btnBothAddresses.Click += (s, e) =>
            {
                _chosenAddressOption = AddressOption.Both;
                _wizard.Advance();
                UpdateFormSize();
            };

            btnNextPick.Click += (s, e) =>
            {
                if (_chosenAddressOption == AddressOption.OnlyPickUp)
                {
                    _wizard.GoTo(4);
                    UpdateFormSize();
                }
                else
                {
                    _wizard.Advance();
                    UpdateFormSize();
                }
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
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                TaxId = cbTaxId.Text,
                // TODO: Mapear direcciones aquí
            };

            CreateClientRequested?.Invoke(this, dto);

        }

    
    }
}