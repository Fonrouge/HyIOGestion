using BLL.LogicLayers;
using Presenter.ForSale;
using Shared;
using System;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Forms.Base;
using WinformsUI.Infrastructure.Factories;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls.CustomDGV;

namespace WinformsUI.Forms.SaleCRUDL
{
    public partial class SaleForm : BaseManagementForm<SaleDTO>, ISaleView
    {
        private readonly IFormsFactory _formsFactory;
        public override event EventHandler OnceLoadedAdvice;

        public SaleForm
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

        public new void ThemingNotifiedByConfigurationsModule() => DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());


        // =========================================================
        // LÓGICA ESPECÍFICA DE VENTA
        // =========================================================
        public void OpenCreationView() => ((Form)_formsFactory.SaleCreationForm()).Show();

        public void OpenUpdateView()
        {
            MessageBox.Show("Un pago, por ahora, no puede editarse. Esta funcionalidad se implementará en el futuro.");
            //throw new NotImplementedException(); //Un pago, por ahora, no puede editarse. Se quita la expceción para evitar corte de programa.
        }

        protected override void OnEntitySelected(SaleDTO entity)
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
                // Limpieza de eventos de controles específicos
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
            base.Dispose(disposing); // La base se encarga de desuscribir el cierre
        }

      
    }
}