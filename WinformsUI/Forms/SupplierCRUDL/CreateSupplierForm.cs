using BLL.DTOs;
using BLL.LogicLayers;
using Presenter.ForSupplier;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Winforms.Theme;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls.Wizard;

namespace WinformsUI.Forms.SupplierCRUDL
{
    public partial class CreateSupplierForm : Form, ICreateSupplierView
    {
        private readonly IApplicationSettings _appSettings;
        private readonly ITranslatableControlsManager _transMgr;
        private readonly IWizardPanelNavigator _wizard;

        private readonly string _errorMsg;
        private readonly string _successMsg;


        public event EventHandler<SupplierDTO> CreateSupplierRequested;
        public event EventHandler CloseRequested;

        public CreateSupplierForm
        (
            ITranslatableControlsManager transMgr,
            IApplicationSettings appSettings,
            IWizardPanelNavigator wizard
        )
        {
            _transMgr = transMgr;
            _appSettings = appSettings;
            _wizard = wizard;

            _successMsg = _appSettings.SuccessOnOperation;
            _errorMsg = _appSettings.ErrorOnOperation;

            InitializeComponent();
            AddTranslatables();
            InitializeWizard();
            ApplyGlobalPalette();
            WireCommonEvents();

        }
        public void ApplyGlobalPalette() => DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());

        private void InitializeWizard()
        {
            _wizard.Initialize(new Panel[] { pnlAddSupplier, pnlAddAddress });
            WireFlowEvents();
        }

        private void WireFlowEvents()
        {
            btnNextPnl1.Click += WizardAdvance;
            btnBackPnl2.Click += WizardBack;
        }

        private void WireCommonEvents()
        {
            btnFinish.Click += ExecuteCreation;
        }

        private void ExecuteCreation(object sender, EventArgs e)
        {
            var newSupplierDto = new SupplierDTO()
            {
                CompanyName = txtCompanyName.Text,
                ContactName = txtContactName.Text,
                TaxId = txtTaxId.Text,
                Mail = txtMail.Text,
                Phone = txtPhone.Text,
                Address = txtAddress.Text,
                City = txtCity.Text,
                Observations = txtObservations.Text,                
            };

            CreateSupplierRequested?.Invoke(this, newSupplierDto);
        }

        private void WizardAdvance(object sender, EventArgs e)
        {
            _wizard.Advance();
            UpdateFormSize();
        }

        private void WizardBack(object sender, EventArgs e)
        {
            _wizard.Back();
            UpdateFormSize();
        }
        private void UpdateFormSize() => this.ClientSize = _wizard.GetPanelSize();

        private void AddTranslatables()
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<Button>(this.Controls, "Text");

            ApplyTranslation();

            _transMgr.AddFormNotify(this);
        }

        public void NotifiedByTranslationManager() => ApplyTranslation();
        public void ApplyTranslation() => _transMgr.Apply();

        public void ShowOperationResult(OperationResult<SupplierDTO> opRes)
        {
            MessageBox.Show(opRes.Success ? $"{_successMsg}" : $"{_errorMsg}. Errors: {string.Join(", ", opRes.Errors)}");
        }

        protected override void Dispose(bool disposing)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);

            if (disposing && (components != null))
            {
                components.Dispose();
            }

            btnNextPnl1.Click -= WizardAdvance;
            btnBackPnl2.Click -= WizardBack;
            _transMgr.RemoveFormNotify(this);

            base.Dispose(disposing);

            this.Close();
        }

    }
}
