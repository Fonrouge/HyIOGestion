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

            // Disparamos la carga inicial
            


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
            btnNextPnl1.Click += (_, __) =>
            {
                _wizard.Advance();
                ListAllCategoriesRequested?.Invoke(this, EventArgs.Empty);
                lbSelectCategories.DataSource = _availableCategories;
            };

            btnBackPnl2.Click += (_, __) => _wizard.Back();
            btnFinish.Click += (_, __) => CreateProduct();

            // Categorías
            btnAddCategory.Click += (_, __) => AddCategory();
            btnRemoveCategory.Click += (_, __) => RemoveCategory();

            lbSelectCategories.SelectedIndexChanged += (_, __) => UpdateButtonsState();
            lbCategoriesAdded.SelectedIndexChanged += (_, __) => UpdateButtonsState();
        }

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
            lbSelectCategories.ClearSelected();   // Mejor UX
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
            lbCategoriesAdded.ClearSelected();    // Mejor UX
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
            // ← Aquí iría tu lógica de feedback (MessageBox, label de error, cerrar form, etc.)
            // Ejemplo rápido:
            // if (opRes.IsSuccess) { MessageBox.Show("Producto creado correctamente"); Close(); }
            throw new NotImplementedException();
        }

        private void AddTranslatables()
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<Button>(this.Controls, "Text");
            ApplyTranslation();
            _transMgr.AddFormNotify(this);
        }

        public void NotifiedByTranslationManager() => ApplyTranslation();
        public void ApplyTranslation() => _transMgr.Apply();

        public void ApplyGlobalPalette() => DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());

        private void UpdateClientSize() => this.ClientSize = _wizard.GetPanelSize();
        private void InitializeWizard() => _wizard.Initialize(new Panel[] { pnlCreation, pnlCategories });
    }
}