using BLL.DTOs;
using BLL.LogicLayers;
using Presenter.ForSale;
using Shared;
using Shared.Services.Searching;
using Shared.Sessions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls;
using WinformsUI.UserControls.SearchBar;
using WinformsUI.UserControls.Wizard;

namespace WinformsUI.Forms.SaleCRUDL
{
    public partial class CreateSaleForm : Form, ICreateSaleView
    {
        private readonly ITranslatableControlsManager _transMgr;
        private readonly IApplicationSettings _appSettings;
        private readonly IListFilterSortProvider _listFilterSortProvider;
        private readonly IWizardPanelNavigator _wizard;
        private readonly ISessionProvider _sessionProv;

        private ClientDTO _selectedClient;
        private BindingList<ClientDTO> _catchedAllClients;
        private BindingList<ProductDTO> _catchedAllProducts;
        private BindingList<SaleDetailDTO> _selectedProdsAndQuantities;

        private SearchBehavior<ClientDTO> _searchBehaviorForClient;
        private SearchBehavior<ProductDTO> _searchBehaviorForProducts;

        // ==================== EVENTOS EXISTENTES ====================
        public event EventHandler OnLoadRequested;
        public event EventHandler CreateSaleRequested;
        public event EventHandler OnCloseRequested;
        public event EventHandler UpdateSubTotalRequested;

        // ==================== NUEVOS EVENTOS (para el Presenter) ====================
        public event EventHandler AddProductRequested;
        public event EventHandler RemoveProductRequested;

        public CreateSaleForm
        (
            ITranslatableControlsManager transMgr,
            IApplicationSettings appSettings,
            IListFilterSortProvider listFilterSortProvider,
            IWizardPanelNavigator wizard,
            ISessionProvider sessionProv
        )
        {
            _transMgr = transMgr;
            _appSettings = appSettings;
            _listFilterSortProvider = listFilterSortProvider;
            _wizard = wizard;
            _sessionProv = sessionProv;

            _selectedProdsAndQuantities = new BindingList<SaleDetailDTO>();

            InitializeComponent();
            InitializeWizard();
            WireCommonEvents();
            WireFlowEvents();
            ThemingNotifiedByConfigurationsModule();
            UpdateClientSize();
        }

        private void InitializeWizard() => _wizard.Initialize(new TableLayoutPanel[] { tlpSelectClient, tlpSelectProds });

        private void WireCommonEvents()
        {
            this.Load += (sender, e) => OnLoadRequested?.Invoke(this, EventArgs.Empty);
            this.FormClosed += (sender, e) => OnCloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void WireFlowEvents()
        {
            btnNextPnl1.Click += WizardAdvance;
            btnBackPnl2.Click += WizardBack;
            btnFinish.Click += ExecuteSaleCreation;

            btnAddProd.Click += (s, e) => AddProductRequested?.Invoke(this, EventArgs.Empty);
            btnRemoveProd.Click += (s, e) => RemoveProductRequested?.Invoke(this, EventArgs.Empty);
        }

        private void WizardAdvance(object sender, EventArgs e) { _wizard.Advance(); UpdateClientSize(); }
        private void WizardBack(object sender, EventArgs e) { _wizard.Back(); UpdateClientSize(); }
        private void UpdateClientSize() => this.ClientSize = _wizard.GetPanelSize();


        public ProductDTO GetSelectedProduct()
        {
            if (dgvSelectProduct.SelectedRows.Count == 0)
                return null;

            return dgvSelectProduct.SelectedRows[0].DataBoundItem as ProductDTO;
        }

        public decimal GetSelectedQuantity()
        {
            return decimal.TryParse(tbQuantity.Text.Trim(), out decimal qty) && qty > 0 ? qty : 1m;
        }

        public void AddSaleDetailToList(SaleDetailDTO detail)
        {
            _selectedProdsAndQuantities.Add(detail);
            RefreshListBox();
        }

        public void RemoveSelectedSaleDetailFromList()
        {
            if (lbAddedProds.SelectedItem is SaleDetailDTO itemToRemove)
            {
                _selectedProdsAndQuantities.Remove(itemToRemove);
                RefreshListBox();
            }
        }

        public List<SaleDetailDTO> GetCurrentSaleDetails()
        {
            return _selectedProdsAndQuantities.ToList();
        }

        public void ClearSaleDetailsList()
        {
            _selectedProdsAndQuantities.Clear();
            lbAddedProds.DataSource = null;
        }

        public void RefreshSubTotal(decimal total) => tbSubTotal.Text = total.ToString("C2");

        public SaleDTO GetMappedSaleDTO()
        {
            if (_selectedClient == null)
            {
                EnsureDGVSelection();
            }

            return new SaleDTO()
            {
                ClientId = _selectedClient?.Id ?? Guid.Empty,
                EmployeeId = _sessionProv.Current.Id,
                Date = DateTime.UtcNow,
                TotalAmount = _selectedProdsAndQuantities.Sum(d => d.Subtotal),
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Items = _selectedProdsAndQuantities.ToList(),
                IsDeleted = false,
                DVH = string.Empty
            };
        }

        private void RefreshListBox()
        {
            lbAddedProds.DataSource = null;
            lbAddedProds.DataSource = _selectedProdsAndQuantities;
            // Usa automáticamente .ToString() de SaleDetailDTO
        }

        private void ExecuteSaleCreation(object sender, EventArgs e)
        {
            if (_selectedProdsAndQuantities.Count < 1)
            {
                MessageBox.Show("Debe agregar al menos un producto a la venta.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            CreateSaleRequested?.Invoke(this, EventArgs.Empty);
        }

       
        public void AddViewTranslatables() => AddTranslatables();

        private void AddTranslatables()
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<Button>(this.Controls, "Text");

            foreach (DataGridViewColumn column in dgvSelectClient.Columns)
                _transMgr.AddString($"dgvSelectClient.DataGridView.Column.{column.Name}", column.HeaderText);

            foreach (DataGridViewColumn column in dgvSelectProduct.Columns)
                _transMgr.AddString($"dgvSelectProduct.DataGridView.Column.{column.Name}", column.HeaderText);

            ApplyTranslation();
            _transMgr.AddFormNotify(this);
        }

        public void InitializeClientGrid(List<ClientDTO> allClientsList)
        {
            _searchBehaviorForClient = new SearchBehavior<ClientDTO>
            (
                dgv: dgvSelectClient,
                listFilterSort: _listFilterSortProvider,
                appSettings: _appSettings,
                transMgr: _transMgr
            );
            _searchBehaviorForClient.AttachNewTextBoxAsSearchBar(tbSearchBarClient, _appSettings.SearchBarPlaceHolder);

            _catchedAllClients = allClientsList.ToBindingList<ClientDTO>();
            _searchBehaviorForClient.UpdateList(_catchedAllClients);

            DgvFormat.Apply(dgvSelectClient, isMiniDgv: true);



            if (dgvSelectClient.Rows.Count > 0)
                dgvSelectClient.Rows[0].Selected = true;

            dgvSelectClient.SelectionChanged += (sender, e) =>
            {
                if (dgvSelectClient.SelectedRows.Count > 0)
                {
                    EnsureDGVSelection();
                }
            };
        }

        private void EnsureDGVSelection()
        {
            _selectedClient = dgvSelectClient.SelectedRows[0].DataBoundItem as ClientDTO;
            txtClientsummary.Text = _selectedClient.ToString();
        }

        public void InitializeProductsGrid(List<ProductDTO> allProductsList)
        {
            _searchBehaviorForProducts = new SearchBehavior<ProductDTO>
            (
                dgv: dgvSelectProduct,
                listFilterSort: _listFilterSortProvider,
                appSettings: _appSettings,
                transMgr: _transMgr
            );
            _searchBehaviorForProducts.AttachNewTextBoxAsSearchBar(tbSearchBarProducts, _appSettings.SearchBarPlaceHolder);

            _catchedAllProducts = allProductsList.ToBindingList<ProductDTO>();
            _searchBehaviorForProducts.UpdateList(_catchedAllProducts);

            DgvFormat.Apply(dgvSelectProduct, isMiniDgv: true);
        }

        public void ShowOperationResultClients(OperationResult<ClientDTO> opRes)
        {
            if (opRes.Errors.Count > 0)
            {
                foreach (var error in opRes.Errors)
                    MessageBox.Show(error.Message + " - " + error.InformativeMessage);
            }
        }

        public void ShowOperationResultProducts(OperationResult<ProductDTO> opRes)
        {
            if (opRes.Errors.Count > 0)
            {
                foreach (var error in opRes.Errors)
                    MessageBox.Show(error.Message + " - " + error.InformativeMessage);
            }
        }

        public void ShowOperationResultSales(OperationResult<SaleDTO> opRes)
        {
            if (opRes.Errors.Count > 0)
            {
                foreach (var error in opRes.Errors)
                    MessageBox.Show(error.Message + " - " + error.InformativeMessage);
            }
            else
            {
                MessageBox.Show("Venta generada correctamente", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void ShowWarning(string message)
        {
            MessageBox.Show(message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void NotifiedByTranslationManager() => ApplyTranslation();
        public void ApplyTranslation() => _transMgr.Apply();

        private void ThemingNotifiedByConfigurationsModule()
        {
            DarkTheme.RedrawBorders = true;
            DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());
        }
        public void DisposeForm() => this.Dispose(true);

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                btnNextPnl1.Click -= WizardAdvance;
                btnBackPnl2.Click -= WizardBack;
                btnFinish.Click -= ExecuteSaleCreation;

                OnLoadRequested = null;
                CreateSaleRequested = null;
                AddProductRequested = null;
                RemoveProductRequested = null;

                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}