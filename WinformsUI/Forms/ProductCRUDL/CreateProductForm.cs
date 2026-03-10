using BLL.DTOs;
using BLL.LogicLayers;
using Presenter.ForProducts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls.Wizard;

namespace WinformsUI.Forms.ProductCRUDL
{
    public partial class CreateProductForm : Form, ICreateProductView
    {
        public event EventHandler<ProductDTO> CreateProductRequested;
        public event EventHandler ListAllCategoriesRequested;
        public event EventHandler CloseRequested;

        private readonly ITranslatableControlsManager _transMgr;
        private readonly IWizardPanelNavigator _wizard;

        private readonly BindingList<CategoryDTO> _availableCategories = new BindingList<CategoryDTO>();
        private readonly BindingList<CategoryDTO> _selectedCategories = new BindingList<CategoryDTO>();

        public CreateProductForm
        (
            ITranslatableControlsManager transMgr,
            IWizardPanelNavigator wizard
        )
        {
            _transMgr = transMgr;
            _wizard = wizard;

            InitializeComponent();

            ConfigureListBoxes();
            WireEvents();
            InitializeWizard();
            UpdateClientSize();

            AddTranslatables();
        }

        private void ConfigureListBoxes()
        {
            lbSelectCategories.SelectionMode = SelectionMode.MultiExtended;
            lbCategoriesAdded.SelectionMode = SelectionMode.MultiExtended;

            lbSelectCategories.DisplayMember = "Name";
            lbCategoriesAdded.DisplayMember = "Name";

            lbSelectCategories.DataSource = _availableCategories;
            lbCategoriesAdded.DataSource = _selectedCategories;
        }

        private void WireEvents()
        {
            // Flow del wizard
            btnNextPnl1.Click += BtnNextPnl1_Click;
            btnBackPnl2.Click += BtnBackPnl2_Click;
            btnFinish.Click += BtnFinish_Click;

            // Categorías
            btnAddCategory.Click += BtnAddCategory_Click;
            btnRemoveCategory.Click += BtnRemoveCategory_Click;

            lbSelectCategories.SelectedIndexChanged += LbCategories_SelectedIndexChanged;
            lbCategoriesAdded.SelectedIndexChanged += LbCategories_SelectedIndexChanged;
        }

        // ===================================================================
        // Manejadores de Eventos (Event Handlers)
        // ===================================================================
        private void BtnNextPnl1_Click(object sender, EventArgs e)
        {
            _wizard.Advance();
            ListAllCategoriesRequested?.Invoke(this, EventArgs.Empty);
            lbSelectCategories.DataSource = _availableCategories;
            UpdateClientSize();
        }

        private void BtnBackPnl2_Click(object sender, EventArgs e)
        {
            _wizard.Back();
            UpdateClientSize();
        }

        private void BtnFinish_Click(object sender, EventArgs e) => CreateProduct();

        private void BtnAddCategory_Click(object sender, EventArgs e) => AddCategory();

        private void BtnRemoveCategory_Click(object sender, EventArgs e) => RemoveCategory();

        private void LbCategories_SelectedIndexChanged(object sender, EventArgs e) => UpdateButtonsState();

        // ===================================================================
        // Lógica de UI
        // ===================================================================
        private void UpdateButtonsState()
        {
            btnAddCategory.Enabled = lbSelectCategories.SelectedItems.Count > 0;
            btnRemoveCategory.Enabled = lbCategoriesAdded.SelectedItems.Count > 0;
        }

        private void AddCategory()
        {
            var toAdd = lbSelectCategories.SelectedItems.Cast<CategoryDTO>().ToList();

            if (toAdd.Count == 0) return;

            foreach (var category in toAdd)
            {
                _availableCategories.Remove(category);
                _selectedCategories.Add(category);
            }

            SortAvailableList();
            UpdateButtonsState();
            lbSelectCategories.ClearSelected();
        }

        private void RemoveCategory()
        {
            var toRemove = lbCategoriesAdded.SelectedItems.Cast<CategoryDTO>().ToList();
            if (toRemove.Count == 0) return;

            foreach (var category in toRemove)
            {
                _selectedCategories.Remove(category);
                _availableCategories.Add(category);
            }

            SortAvailableList();
            UpdateButtonsState();
            lbCategoriesAdded.ClearSelected();
        }

        private void SortAvailableList()
        {
            if (_availableCategories.Count <= 1) return;

            var sorted = _availableCategories.OrderBy(c => c.Name).ToList();
            _availableCategories.Clear();
            foreach (var item in sorted)
                _availableCategories.Add(item);
        }

        private void CreateProduct()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("El nombre del producto es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            var newProduct = new ProductDTO
            {
                Name = txtName.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                Price = decimal.TryParse(txtPrice.Text, out var price) ? price : 0m,
                Stock = int.TryParse(txtStock.Text, out var stock) ? stock : 0,
                Categories = _selectedCategories.ToList()
            };

            CreateProductRequested?.Invoke(this, newProduct);
        }

        // ===================================================================
        // Métodos de la interfaz ICreateProductView
        // ===================================================================
        public void CachingCategoriesList(IEnumerable<CategoryDTO> categoriesList)
        {
            if (categoriesList == null)
                throw new ArgumentNullException(nameof(categoriesList));

            _availableCategories.Clear();
            _selectedCategories.Clear();

            foreach (var cat in categoriesList.OrderBy(c => c.Name))
                _availableCategories.Add(cat);

            UpdateButtonsState();
        }

        public void ShowOperationResult(OperationResult<ProductDTO> opRes)
        {
            if (opRes.Errors != null && opRes.Errors.Count > 0)
            {
                // Concatenamos todos los errores en un solo string
                var errorMessages = opRes.Errors.Select(e => $"{e.Message} - {e.InformativeMessage}");
                var finalMessage = string.Join(Environment.NewLine, errorMessages);

                MessageBox.Show(finalMessage, "Error en la operación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("El producto se creó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void AddTranslatables()
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<Button>(this.Controls, "Text");
            ApplyTranslation();
            _transMgr.AddFormNotify(this);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _transMgr.RemoveFormNotify(this);
            CloseRequested?.Invoke(this, EventArgs.Empty);
            base.OnFormClosed(e);
        }

        public void NotifiedByTranslationManager() => ApplyTranslation();
        public void ApplyTranslation() => _transMgr.Apply();
        public void ApplyGlobalPalette() => DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());
        private void UpdateClientSize() => this.ClientSize = _wizard.GetPanelSize();
        private void InitializeWizard() => _wizard.Initialize(new Panel[] { pnlCreation, pnlCategories });
    }
}