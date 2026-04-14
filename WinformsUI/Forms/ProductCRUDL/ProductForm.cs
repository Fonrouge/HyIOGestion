using BLL.LogicLayers;
using Presenter.ForProducts;
using Presenter.Presenters.ForProducts;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public override event EventHandler OnceLoadedAdvice;

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
            InitializeDGV();
            WireSpecificEvents();
            AddTranslatables();

            InitializeRibbonControls();
            InitializePanelToggle();

            this.Load += OnceLoaded;
        }

        private void OnceLoaded(object sender, EventArgs e) => OnceLoadedAdvice?.Invoke(this, EventArgs.Empty);

        
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

        public new void ThemingNotifiedByConfigurationsModule()
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
        public void OpenUpdateView()
        {
            if (_currentSelectedEntity == null)
            {
                MessageBox.Show("Primero seleccione un producto en la grilla");
            }

            var newUpdateForm = (UpdateProductForm)_formsFactory.ProductUpdateForm<IUpdateProductView>();
            newUpdateForm.SetProductData(_currentSelectedEntity);
            newUpdateForm.ShowDialog();
        }

        protected override void OnEntitySelected(ProductDTO entity)
        {
            // Hook opcional para acciones dependientes de selección
        }

        private void WireSpecificEvents()
        {
            btnCreate.Click += OnCreateRequest;
            btnUpdate.Click += OnUpdateRequest;
            btnDelete.Click += OnDeleteRequest;
            btnRefresh.Click += OnListAllRequest;
        }


        // =============================================================================================================
        // LINKEO DE CONTROLES (instancia genérica -de BaseForm- ahora apunta a instancia específica de este formulario)
        // =============================================================================================================
        private void InitializeDGV() => base.InitializeDGV(this.dgvPanel);

        private new void InitializeRibbonControls()
        {
            _dgvRibbonControls = DGVFunctionsControl;
            _eyeRestRibbonControls = eyeRestRibbon;
            base.InitializeRibbonControls();
        }

        public void InitializePanelToggle() => base.ToolStripsPanelToggle(toolStripsPanel);


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