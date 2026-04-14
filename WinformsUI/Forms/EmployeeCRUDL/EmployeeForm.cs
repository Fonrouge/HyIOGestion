using BLL.DTOs;
using Presenter.ForEmployee;
using Shared;
using System;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Forms.Base;
using WinformsUI.Infrastructure.Factories;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls.CustomDGV;

namespace WinformsUI.Forms.EmployeeCRUDL
{
    public partial class EmployeeForm : BaseManagementForm<EmployeeDTO>, IEmployeeView
    {
        private readonly IFormsFactory _formsFactory;
        public override event EventHandler OnceLoadedAdvice;

        public EmployeeForm
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

        public override void ThemingNotifiedByConfigurationsModule()
        {
            DarkTheme.RedrawBorders = true;
            DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());
        }


        // =========================================================
        // LÓGICA ESPECÍFICA DE EMPLEADO
        // =========================================================
        public void OpenCreationView() => ((Form)_formsFactory.EmployeeCreationForm()).Show();

        private void WireSpecificEvents()
        {
            btnCreate.Click += OnCreateRequest;
            btnUpdate.Click += OnUpdateRequest;
            btnDelete.Click += OnDeleteRequest;
            btnRefresh.Click += OnListAllRequest;
        }

        private void OpenUpdateModule(object sender, EventArgs e)
        {
            if (_currentSelectedEntity == null)
            {
                MessageBox.Show("Primero seleccione un cliente en la grilla");
                return;
            }

            base.OnUpdateRequest(sender, e);
        }

        protected override void OnEntitySelected(EmployeeDTO entity)
        {
            // Hook opcional para acciones dependientes de selección
        }

        public void OpenUpdateView()
        {
            if (_currentSelectedEntity == null)
            {
                MessageBox.Show("Primero seleccione un cliente en la grilla");
            }

            var newUpdateForm = (UpdateEmployeeForm)_formsFactory.EmployeeUpdateForm<IUpdateEmployeeView>();
            newUpdateForm.SetEmployeeData(_currentSelectedEntity);
            newUpdateForm.ShowDialog();
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
                // Desuscripción de eventos de botones específicos
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
            base.Dispose(disposing); // La base desuscribe el cierre automático
        }

      
    }
}