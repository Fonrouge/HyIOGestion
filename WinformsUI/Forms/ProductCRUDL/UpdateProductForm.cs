using BLL.DTOs;
using BLL.LogicLayers;
using Presenter.Presenters.ForProducts;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Infrastructure.Translations;
using static Winforms.Theme.DarkTheme;

namespace WinformsUI.Forms.ProductCRUDL
{
    public partial class UpdateProductForm : Form, IUpdateProductView
    {
        public event EventHandler<ProductDTO> UpdateProductRequested;
        public event EventHandler ListAllCategoriesRequested;
        public event EventHandler MinimizeRequested;
        public event EventHandler CloseRequested;

        private List<Button> _editButtons;
        private Dictionary<TextBox, string> _cacheOriginalValues;
        private Palette _internalPalette;
        private ProductDTO _product;

        // Categorías (misma lógica que CreateProductForm)
        private readonly BindingList<CategoryDTO> _availableCategories = new BindingList<CategoryDTO>();
        private readonly BindingList<CategoryDTO> _selectedCategories = new BindingList<CategoryDTO>();
        private List<CategoryDTO> _originalCategories = new List<CategoryDTO>();

        private string _confirmChanges = "¿Está seguro de que desea confirmar los siguientes cambios?";
        private string _title = "Actualizar Producto";
        private string _confirmChangesTitle = "Confirmar Modificación";
        private string _successMsg = "";
        private string _errorMsg = "";

        private readonly ITranslatableControlsManager _transMgr;
        private readonly IApplicationSettings _appSettings;

        public UpdateProductForm
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
            UpdateButtonsState();
        }

        // =================================================================
        // CONFIGURACIÓN DE LISTBOX DE CATEGORÍAS
        // =================================================================
        private void ConfigureListBoxes()
        {
            lbSelectCategories.SelectionMode = SelectionMode.MultiExtended;
            lbCategoriesAdded.SelectionMode = SelectionMode.MultiExtended;

                  lbSelectCategories.DisplayMember = "Id";
                  lbSelectCategories.ValueMember = "Id";
                  lbCategoriesAdded.DisplayMember = "Id";
                  lbCategoriesAdded.ValueMember = "Id";

                    lbSelectCategories.DataSource = _availableCategories;
              lbCategoriesAdded.DataSource = _selectedCategories;
        }

        // =================================================================
        // SUSCRIPCIÓN DE EVENTOS
        // =================================================================
        private void WireEvents()
        {
            _editButtons.ForEach(b => b.Click += SwitchButtonState);

            // Categorías
            btnAddCategory.Click += BtnAddCategory_Click;
            btnRemoveCategory.Click += BtnRemoveCategory_Click;
            lbSelectCategories.SelectedIndexChanged += LbCategories_SelectedIndexChanged;
            lbCategoriesAdded.SelectedIndexChanged += LbCategories_SelectedIndexChanged;

            btnConfirm.Click += OnConfirmClick;
            btnClose.Click += CloseRequest;
            btnMinimize.Click += MinimizeRequest;
        }

        // =========================================================
        // TRADUCCIONES Y PALETA (sin cambios)
        // =========================================================
        private void ThemingNotifiedByConfigurationsModule()
        {
            DarkTheme.RedrawBorders = true;
            DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());
            _internalPalette = DarkTheme.IsDarkPalette(DarkTheme.GetCurrentPalette()) ? DarkTheme.PalettesDark.Aubergine() : DarkTheme.PalettesLight.Grape();
            ApplyGradientBackground(tlpTitleBar, Darken(_internalPalette.LowAccent, 0.65), Darken(_internalPalette.Accent, 0.65), LinearGradientMode.Vertical, false);
            ApplyGradientBackground(pnlBackground, Darken(_internalPalette.Accent, 0.65), Darken(_internalPalette.LowAccent, 0.65), LinearGradientMode.Vertical, false);
        }

        private void AddTranslatables()
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<Button>(this.Controls, "Text");

            _transMgr.AddString("UpdateProductForm._confirmChanges", _confirmChanges);
            _transMgr.AddString("UpdateProductForm._title", _title);
            _transMgr.AddString("UpdateProductForm._confirmChangesTitle", _confirmChangesTitle);
            _transMgr.AddString("UpdateProductForm.AppSettings._successMsg", _successMsg);
            _transMgr.AddString("UpdateProductForm.AppSettings._errorMsg", _errorMsg);

            _transMgr.AddFormNotify(this);
        }

        public void NotifiedByTranslationManager() => ApplyTranslation();

        private void ApplyTranslation()
        {
            _confirmChanges = _transMgr.GetString("UpdateProductForm._confirmChanges");
            _confirmChangesTitle = _transMgr.GetString("UpdateProductForm._confirmChangesTitle");
            _title = _transMgr.GetString("UpdateProductForm._title");
            _successMsg = _transMgr.GetString("UpdateProductForm.AppSettings._successMsg");
            _errorMsg = _transMgr.GetString("UpdateProductForm.AppSettings._errorMsg");

            lblTitle.Text = _title;
            _transMgr.Apply();
        }

        // ===========================================================
        // BOTONES DE EDICIÓN DE CAMPOS "NORMALES" (tipos primitivos)
        // ===========================================================
        private void InitEditButtonsList()
        {
            _editButtons = new List<Button>
            {
                btnEditName,
                btnEditDescription,
                btnEditPrice,
                btnEditStock
            };

            btnEditName.Tag = txtName;
            btnEditDescription.Tag = txtDescription;
            btnEditPrice.Tag = txtPrice;
            btnEditStock.Tag = txtStock;
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
            lblTitle.Text = _title;
        }

        private void SwitchButtonState(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is TextBox tb)
            {
                if (tb.ReadOnly && !_cacheOriginalValues.ContainsKey(tb))
                    _cacheOriginalValues.Add(tb, tb.Text);

                tb.Enabled = !tb.Enabled;
                tb.ReadOnly = !tb.ReadOnly;
                if (!tb.ReadOnly) tb.Focus();
            }
        }

        // ===================================================================
        // LÓGICA DE CATEGORÍAS
        // ===================================================================
        private void UpdateButtonsState()
        {
            btnAddCategory.Enabled = lbSelectCategories.SelectedItems.Count > 0;
            btnRemoveCategory.Enabled = lbCategoriesAdded.SelectedItems.Count > 0;
        }

        private void BtnAddCategory_Click(object sender, EventArgs e) => AddCategory();
        private void BtnRemoveCategory_Click(object sender, EventArgs e) => RemoveCategory();
        private void LbCategories_SelectedIndexChanged(object sender, EventArgs e) => UpdateButtonsState();

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

        // ===================================================================
        // CARGA DE CATEGORÍAS (aquí se hace la magia)
        // ===================================================================
        public void CachingCategoriesList(IEnumerable<CategoryDTO> categoriesList)
        {
            if (categoriesList == null)
                throw new ArgumentNullException(nameof(categoriesList));

            _availableCategories.Clear();
            _selectedCategories.Clear();

            // Todas las categorías disponibles
            foreach (var cat in categoriesList.OrderBy(c => c.Name))
                _availableCategories.Add(cat);

            // Mover automáticamente las que ya tiene el producto a "seleccionadas"
            if (_product?.Categories != null)
            {
                foreach (var selectedCat in _product.Categories.ToList())
                {
                    var catInAvailable = _availableCategories.FirstOrDefault(c => c.Id == selectedCat.Id);

                    if (catInAvailable != null)
                    {
                        _availableCategories.Remove(catInAvailable);
                        _selectedCategories.Add(catInAvailable);
                    }
                }
            }

            ConfigureListBoxes();
            UpdateButtonsState();
        }

        // =========================================================
        // DATOS DE ENTRADA
        // =========================================================
        public void SetProductData(ProductDTO dto)
        {
            _product = dto ?? throw new ArgumentNullException(nameof(dto));

            txtName.Text = dto.Name ?? string.Empty;
            txtDescription.Text = dto.Description ?? string.Empty;
            txtPrice.Text = dto.Price.ToString(System.Globalization.CultureInfo.CurrentCulture);
            txtStock.Text = dto.Stock.ToString(System.Globalization.CultureInfo.CurrentCulture);

            ListAllCategoriesRequested?.Invoke(this, EventArgs.Empty);


    
            _originalCategories = _selectedCategories.ToList(); // para detectar cambios
        }

        // =========================================================
        // DATOS DE SALIDA + CONFIRMACIÓN
        // =========================================================
        private void OnConfirmClick(object sender, EventArgs e)
        {
            if (ValidateChanges())
            {
                btnConfirm.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                UpdateProductRequested?.Invoke(this, MapNewData());
                this.DialogResult = DialogResult.OK;
                CloseRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private ProductDTO MapNewData()
        {
            return new ProductDTO
            {
                Id = _product.Id,
                Name = txtName.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                Price = decimal.TryParse(txtPrice.Text, out var p) ? p : _product.Price,
                Stock = decimal.TryParse(txtStock.Text, out var s) ? s : _product.Stock,
                Categories = _selectedCategories.ToList(),
                IsActive = _product.IsActive,
                CreatedAt = _product.CreatedAt,
                IsDeleted = _product.IsDeleted,
                DVH = _product.DVH
            };
        }

        private bool ValidateChanges()
        {
            var sb = new StringBuilder();
            bool hasChanges = false;

            foreach (var item in _cacheOriginalValues)
            {
                var tb = item.Key;
                var original = item.Value;
                var nuevo = tb.Text;

                if (original != nuevo)
                {
                    hasChanges = true;
                    var fieldName = tb.Name.Replace("txt", "");
                    sb.AppendLine($"{fieldName}: '{original}' → '{nuevo}'");
                }
            }

            if (CategoriesHaveChanged())
            {
                hasChanges = true;
                var origStr = _originalCategories.Any()
                    ? string.Join(", ", _originalCategories.Select(c => c.Name))
                    : "Sin categorizar";
                var newStr = _selectedCategories.Any()
                    ? string.Join(", ", _selectedCategories.Select(c => c.Name))
                    : "Sin categorizar";

                sb.AppendLine($"Categorías: '{origStr}' → '{newStr}'");
            }

            if (!hasChanges) return true;

            var mensaje = $"{_confirmChanges}\n\n{sb}";
            var result = MessageBox.Show(mensaje, _confirmChangesTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        private bool CategoriesHaveChanged()
        {
            var origIds = _originalCategories.Select(c => c.Id).OrderBy(id => id).ToList();
            var currentIds = _selectedCategories.Select(c => c.Id).OrderBy(id => id).ToList();
            return !origIds.SequenceEqual(currentIds);
        }

        // =========================================================
        // VENTANA Y RESULTADOS
        // =========================================================
        private void CloseRequest(object sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
        private void MinimizeRequest(object sender, EventArgs e) => MinimizeRequested?.Invoke(this, EventArgs.Empty);

        public void MinimizeView() => this.WindowState = FormWindowState.Minimized;
        public void CloseView() => this.Close();

        public void ShowOperationResult(OperationResult<ProductDTO> opRes)
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
                var errors = string.Join(Environment.NewLine, opRes.Errors);
                MessageBox.Show($"{_errorMsg}{Environment.NewLine}{errors}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ShowCategoriesOperationResult(OperationResult<CategoryDTO> opRes)
        {
            if (!opRes.Success)
            {
                MessageBox.Show($"Error en carga de categorías: {opRes.Errors.FirstOrDefault()?.Message ?? "Error desconocido"}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        // =========================================================
        // LIMPIEZA DE MEMORIA
        // =========================================================
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                btnAddCategory.Click -= BtnAddCategory_Click;
                btnRemoveCategory.Click -= BtnRemoveCategory_Click;
                lbSelectCategories.SelectedIndexChanged -= LbCategories_SelectedIndexChanged;
                lbCategoriesAdded.SelectedIndexChanged -= LbCategories_SelectedIndexChanged;

                if (_editButtons != null)
                {
                    _editButtons.ForEach(b => b.Click -= SwitchButtonState);
                    _editButtons.Clear();
                    _editButtons = null;
                }

                _cacheOriginalValues?.Clear();
                _originalCategories?.Clear();

                btnConfirm.Click -= OnConfirmClick;
                btnClose.Click -= CloseRequest;
                btnMinimize.Click -= MinimizeRequest;

                UpdateProductRequested = null;
                ListAllCategoriesRequested = null;
                MinimizeRequested = null;
                CloseRequested = null;

                _transMgr.RemoveFormNotify(this);

                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}