using BLL.DTOs;
using BLL.LogicLayers;
using Presenter.ForEmployee;
using Shared;
using Shared.Services.Searching;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls;
using WinformsUI.UserControls.SearchBar;
using WinformsUI.UserControls.Wizard;

namespace WinformsUI.Forms.PaymentCRUDL
{
    public partial class CreatePaymentForm : Form, ICreatePaymentView
    {
        private readonly IApplicationSettings _appSettings;
        private readonly IListFilterSortProvider _listFilterSortProvider;
        private readonly IWizardPanelNavigator _wizard;
        private readonly ITranslatableControlsManager _transMgr;

        private readonly string _errorMsg;
        private readonly string _successMsg;
        private SearchBehavior<SaleDTO> _searchBehavior;

        private BindingList<SaleDTO> _catchedAllSales;
        private SaleDTO _selectedSaleDTO;

        public event EventHandler<PaymentDTO> CreatePaymentRequested;
        public event EventHandler GetAllSalesRequested;
        public event EventHandler CloseRequested;

        public CreatePaymentForm
        (
            IApplicationSettings appSettings,
            ITranslatableControlsManager transMgr,
            IListFilterSortProvider listFilterSortProvider,
            IWizardPanelNavigator wizardPanelNavigator
        )
        {
            _appSettings = appSettings;
            _transMgr = transMgr;
            _listFilterSortProvider = listFilterSortProvider;
            _wizard = wizardPanelNavigator;

            _successMsg = _appSettings.SuccessOnOperation;
            _errorMsg = _appSettings.ErrorOnOperation;


            InitializeComponent();

            ApplyDarkTheme();
            InitializeWizard();
            WireFlowButtonsEvents();
            ApplyGlobalPalette();

            UpdateFormSize();


            this.Load += (sender, e) =>
            {
                InitializeClientsDGV();
            };

        }

        public void ApplyGlobalPalette() => DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());
        private void UpdateFormSize() => this.ClientSize = _wizard.GetPanelSize();

        private void InitializeClientsDGV()
        {
            GetAllSalesRequested?.Invoke(this, EventArgs.Empty);
        }

        public void InitializeGrid(List<SaleDTO> allSalesList)
        {
            if (allSalesList == null) return;

            _catchedAllSales = new BindingList<SaleDTO>();


            _searchBehavior = new SearchBehavior<SaleDTO>
            (
                dgv: selectClientDGV,
                listFilterSort: _listFilterSortProvider,
                appSettings: _appSettings,
                transMgr: _transMgr
            );

            _searchBehavior.AttachNewTextBoxAsSearchBar(tbSearchBar, _appSettings.SearchBarPlaceHolder);

            _catchedAllSales = allSalesList.ToBindingList<SaleDTO>();
            _searchBehavior.UpdateList(_catchedAllSales.ToBindingList());
            DgvFormat.Apply(selectClientDGV, isMiniDgv: true);
            AddTranslatables();
            WireDGVEvents();


        }
        public void FillPaymentMethods(IEnumerable<object> paymentMethods) => cbPaymentMethods.DataSource = paymentMethods;

        private void WireDGVEvents()
        {
            selectClientDGV.SelectionChanged += (sender, e) =>
            {
                if (selectClientDGV.SelectedRows.Count > 0)
                {
                    _selectedSaleDTO = selectClientDGV.SelectedRows[0].DataBoundItem as SaleDTO;
                }
            };
        }

        private void InitializeWizard() => _wizard.Initialize(new Panel[] { pnlSelectClient, tlpAddPayment });


        private void AddTranslatables()
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<Button>(this.Controls, "Text");

            foreach (DataGridViewColumn column in selectClientDGV.Columns)
            {
                _transMgr.AddString($"CustomDGVForm.DataGridView.Column.{column.Name}", column.HeaderText);
            }


            ApplyTranslation();

            _transMgr.AddFormNotify(this);
        }

        public void NotifiedByTranslationManager() => ApplyTranslation(); //Reflection, do not change the method's name.

        public void ApplyTranslation()
        {
            foreach (DataGridViewColumn column in selectClientDGV.Columns)
            {
                column.HeaderText = _transMgr.GetString($"CustomDGVForm.DataGridView.Column.{column.Name}");
            }

            _transMgr.Apply();
        }

        /// <summary>
        /// Limpia los recursos utilizados por el formulario.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CreatePaymentRequested = null;

                btnNextPnl1.Click -= WizardAdvance;
                btnBackContact.Click -= WizardBack;
                btnFinish.Click -= ExecuteCreation;
                _searchBehavior.Dispose();

                if (components != null)
                    components.Dispose();

            }

            base.Dispose(disposing);
        }

        public void ShowOperationResult(OperationResult<PaymentDTO> opRes)
        {
            MessageBox.Show(opRes.Success ? $"{_successMsg}" : $"{_errorMsg}. Errors: {string.Join(", ", opRes.Errors)}");
            this.Close();
        }
        public void ShowOperationResult(OperationResult<SaleDTO> opRes)
        {
            MessageBox.Show(opRes.Success ? $"{_successMsg}" : $"{_errorMsg}. Errors: {string.Join(", ", opRes.Errors)}");
            this.Close();
        }

        private void ApplyDarkTheme()
        {
            DarkTheme.RedrawBorders = true;
            DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());
        }


        private void WireFlowButtonsEvents()
        {
            btnNextPnl1.Click += WizardAdvance;
            btnBackContact.Click += WizardBack;
            btnFinish.Click += ExecuteCreation;
        }

        private void WizardAdvance(object sender, EventArgs e)
        {
            if (_selectedSaleDTO == null)
            {
                _selectedSaleDTO = selectClientDGV.SelectedRows[0].DataBoundItem as SaleDTO;
                txtSelectedClient.Text = _selectedSaleDTO.ToString();
            }

            _wizard.Advance();
            UpdateFormSize();
        }
        private void WizardBack(object sender, EventArgs e)
        {
            _wizard.Back();
            UpdateFormSize();
        }

        private void ExecuteCreation(object sender, EventArgs e) //DATOS DE MOCK PARA CREAR ASÍ NOMÁS
        {
            txtAmount.Text = "0.52";


            var dto = new PaymentDTO()
            {
                //Id = Automático
                Amount = decimal.Parse(txtAmount.Text),
                CreationDate = DateTime.Now,
                EffectiveDate = DateTime.Now, //DateTime.Parse(txtEffectiveDate.Text) ?
                SaleId = Guid.NewGuid(), //    ClientId = Guid.Parse(txtClient.Text),             TOCA CAPTUDAD LA ENTIDAD RECUPERADA DEL DATAGRIDVIEW PARA USARLA ACA.
                Method = cbPaymentMethods.Text,
                Reference = txtReference.Text,
                IsDeleted = false
            };


            CreatePaymentRequested?.Invoke(this, dto);

        }
    }
}
