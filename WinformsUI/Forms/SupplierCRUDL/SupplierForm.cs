using BLL.DTOs;
using Presenter.ForSupplier;
using Presenter.Presenters.ForSupplier;
using Shared;
using System;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Forms.Base;
using WinformsUI.Infrastructure.Factories;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls.CustomDGV;

namespace WinformsUI.Forms.SupplierCRUDL
{
    public partial class SupplierForm : BaseManagementForm<SupplierDTO>, ISupplierView
    {
        private readonly IFormsFactory _formsFact;
        public override event EventHandler OnceLoadedAdvice;

        public SupplierForm
        (
            IApplicationSettings appSettings,
            ITranslatableControlsManager transMgr,
            ICustomDGVFactory dgvFact,
            IFormsFactory formsFact
        ) : base(appSettings, transMgr, dgvFact)
        {
            _formsFact = formsFact;

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
        // TRADUCCIONES
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


        // =========================================================
        // LÓGICA ESPECÍFICA DE PROVEEDORES
        // =========================================================

        public void OpenCreationView() => ((Form)_formsFact.SupplierCreationForm()).Show();

        public void OpenUpdateView()
        {
            if (_currentSelectedEntity == null)
            {
                MessageBox.Show("Primero seleccione un proveedor en la grilla");
            }

            var newUpdateForm = (UpdateSupplierForm)_formsFact.SupplierUpdateForm<IUpdateSupplierView>();
            newUpdateForm.SetSupplierData(_currentSelectedEntity);
            newUpdateForm.ShowDialog();

        }

        protected override void OnEntitySelected(SupplierDTO entity)
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
                // 1. Desuscripción de eventos de controles internos
                if (btnCreate != null) btnCreate.Click -= OnCreateRequest;
                if (btnUpdate != null) btnUpdate.Click -= OnUpdateRequest;
                if (btnDelete != null) btnDelete.Click -= OnDeleteRequest;
                if (btnRefresh != null) btnRefresh.Click -= OnListAllRequest;

                // 2. Limpieza de referencias para el Garbage Collector
                _entitiesList = null;
                _dgvForm = null;
                _transMgr.RemoveFormNotify(this);

                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


    }
}