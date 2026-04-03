using BLL.DTOs;
using Presenter.ForEmployee;
using Presenter.Presenters.ForEmployee;
using Shared;
using System;
using System.Threading.Tasks;
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
            InitializeDGV(this.dgvPanel);

            WireSpecificEvents();
            AddTranslatables();
        }


        // =========================================================
        // IMPLEMENTACIÓN DE IEmployeeView (Mapeo de Eventos a Base)
        // =========================================================
        public event EventHandler CreateEmployeeRequested
        {
            add => CreateRequested += value;
            remove => CreateRequested -= value;
        }

        public event EventHandler<EmployeeDTO> UpdateEmployeeRequested
        {
            add => UpdateRequested += value;
            remove => UpdateRequested -= value;
        }

        public event EventHandler<EmployeeDTO> DeleteEmployeeRequested
        {
            add => DeleteRequested += value;
            remove => DeleteRequested -= value;
        }

        public event EventHandler CachingAllEmployeesRequested
        {
            add => ListAllRequested += value;
            remove => ListAllRequested -= value;
        }

        public event EventHandler CloseEmployeeRequested
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

        public new void ApplyGlobalPalette() => DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());


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

        public Task OpenUpdateView()
        {
            if (_currentSelectedEntity == null)
            {
                MessageBox.Show("Primero seleccione un cliente en la grilla");
                return Task.CompletedTask;
            }

            var newUpdateForm = (UpdateEmployeeForm)_formsFactory.EmployeeUpdateForm<IUpdateEmployeeView>();
            newUpdateForm.SetEmployeeData(_currentSelectedEntity);
            newUpdateForm.ShowDialog();            
            return Task.CompletedTask;
        }
    }
}