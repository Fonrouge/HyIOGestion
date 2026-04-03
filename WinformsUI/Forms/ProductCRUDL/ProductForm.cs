using BLL.LogicLayers;
using Presenter.ForProducts;
using Presenter.Presenters.ForProducts;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Forms.Base;
using WinformsUI.Infrastructure.Factories;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls.CustomDGV;

namespace WinformsUI.Forms.ProductCRUDL
{
    public partial class ProductForm : BaseManagementForm<ProductDTO>, IProductView
    {
        private readonly IFormsFactory _formsFactory;

        public ProductForm
        (
            IApplicationSettings appSettings,
            ITranslatableControlsManager transMgr,
            ICustomDGVFactory dgvFact,
            IFormsFactory formsFact
        ) : base(appSettings, transMgr, dgvFact)
        {
            _formsFactory = formsFact;
            InitializeComponent();
            InitializeDGV(this.dgvPanel);

            WireSpecificEvents();
            AddTranslatables();
        }


        // =========================================================
        // IMPLEMENTACIÓN DE IProductView (Mapeo de Eventos a Base)
        // =========================================================
        public event EventHandler CreateProductRequested
        {
            add => CreateRequested += value;
            remove => CreateRequested -= value;
        }

        public event EventHandler<ProductDTO> UpdateProductRequested
        {
            add => UpdateRequested += value;
            remove => UpdateRequested -= value;
        }

        public event EventHandler<ProductDTO> DeleteProductRequested
        {
            add => DeleteRequested += value;
            remove => DeleteRequested -= value;
        }

        public event EventHandler CachingAllProductsRequested
        {
            add => ListAllRequested += value;
            remove => ListAllRequested -= value;
        }

        public event EventHandler CloseProductRequested
        {
            add => CloseRequested += value;
            remove => CloseRequested -= value;
        }


        // =========================================================
        // TRADUCCIONES Y PALETA
        // =========================================================
        private void AddTranslatables()
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<Button>(this.Controls, "Text");

            _transMgr.AddSingleObject(btnCreate, "Text");
            _transMgr.AddSingleObject(btnDelete, "Text");
            _transMgr.AddSingleObject(btnRefresh, "Text");
            _transMgr.AddSingleObject(btnUpdate, "Text");

            _transMgr.AddFormNotify(this);

            base.ApplyTranslation();
        }

        public new void ApplyGlobalPalette()
        {
            DarkTheme.RedrawBorders = true;
            DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());
        }


        // =========================================================
        // LÓGICA ESPECÍFICA DE PRODUCTO
        // =========================================================

        public void SetSearchFilters<T>(IEnumerable<T> categories) where T : CategoryDTO
            => _dgvForm.ConfigureFilters<CategoryDTO>(categories.ToList());

        public void OpenCreationView() => ((Form)_formsFactory.ProductCreationForm()).Show();
        public Task OpenUpdateView()
        {
            if (_currentSelectedEntity == null)
            {
                MessageBox.Show("Primero seleccione un producto en la grilla");
                return Task.CompletedTask;
            }

            var newUpdateForm = (UpdateProductForm)_formsFactory.ProductUpdateForm<IUpdateProductView>();
            newUpdateForm.SetProductData(_currentSelectedEntity);
            newUpdateForm.ShowDialog();
            return Task.CompletedTask;
        }
        private void WireSpecificEvents()
        {
            btnCreate.Click += OnCreateRequest;
            btnUpdate.Click += OnUpdateRequest;
            btnDelete.Click += OnDeleteRequest;
            btnRefresh.Click += OnListAllRequest;
        }


        // =========================================================
        // CICLO DE VIDA (Lifecycle)
        // =========================================================
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Desuscripción de eventos de botones
                if (btnCreate != null) btnCreate.Click -= OnCreateRequest;
                if (btnUpdate != null) btnUpdate.Click -= OnUpdateRequest;
                if (btnDelete != null) btnDelete.Click -= OnDeleteRequest;
                if (btnRefresh != null) btnRefresh.Click -= OnListAllRequest;

                // Limpieza de referencias
                _entitiesList = null;
                _dgvForm = null;
                _transMgr.RemoveFormNotify(this);

                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing); // La base se encarga de desuscribir el evento de cierre
        }

    }
}