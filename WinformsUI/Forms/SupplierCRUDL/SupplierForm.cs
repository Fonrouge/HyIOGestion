using BLL.DTOs;
using Presenter.ForClient;
using Presenter.ForSupplier;
using Presenter.Presenters.ForClient;
using Presenter.Presenters.ForSupplier;
using Shared;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Forms.Base;
using WinformsUI.Forms.ClientCRUDL;
using WinformsUI.Infrastructure.Factories;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls.CustomDGV;

namespace WinformsUI.Forms.SupplierCRUDL
{
    public partial class SupplierForm : BaseManagementForm<SupplierDTO>, ISupplierView
    {
        public event EventHandler CloseRequested;
        private readonly IFormsFactory _formsFact;

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
            InitializeDGV(this.dgvPanel);

            WireSpecificEvents();
            AddTranslatables();
        }

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
        // IMPLEMENTACIÓN DE ISupplierView (Mapeo de Eventos a Base)
        // =========================================================

        public event EventHandler CreateSupplierRequested
        {
            add => CreateRequested += value;
            remove => CreateRequested -= value;
        }

        public event EventHandler<SupplierDTO> UpdateSupplierRequested
        {
            add => UpdateRequested += value;
            remove => UpdateRequested -= value;
        }

        public event EventHandler<SupplierDTO> DeleteSupplierRequested
        {
            add => DeleteRequested += value;
            remove => DeleteRequested -= value;
        }

        public event EventHandler CachingAllSupplierRequested
        {
            add => ListAllRequested += value;
            remove => ListAllRequested -= value;
        }

        public event EventHandler CloseSupplierRequested
        {
            add => CloseRequested += value;
            remove => CloseRequested -= value;
        }

        // =========================================================
        // LÓGICA DE UI Y CONTROL
        // =========================================================

        public new void ApplyGlobalPalette() => DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());

        public void OpenCreationView() => ((Form)_formsFact.SupplierCreationForm()).Show();

        private void WireSpecificEvents()
        {
            btnCreate.Click += OnCreateRequest;
            btnUpdate.Click += OnUpdateRequest;
            btnDelete.Click += OnDeleteRequest;
            btnRefresh.Click += OnListAllRequest;

            // Suscribimos al cierre para avisar al Presenter
            this.FormClosed += HandleFormClosed;
        }

        private void HandleFormClosed(object sender, FormClosedEventArgs e)
            => CloseRequested?.Invoke(this, EventArgs.Empty);

        public void CloseView()
        {
            // Verificamos que no esté ya en proceso de eliminación
            if (!this.IsDisposed)
            {
                this.Close(); // Close es más seguro que Dispose directo en WinForms
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 1. Desuscripción de eventos de controles internos
                this.FormClosed -= HandleFormClosed;

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

        public Task OpenUpdateView()
        {
            if (_currentSelectedEntity == null)
            {
                MessageBox.Show("Primero seleccione un proveedor en la grilla");
                return Task.CompletedTask;
            }

            var newUpdateForm = (UpdateSupplierForm)_formsFact.SupplierUpdateForm<IUpdateSupplierView>();
            newUpdateForm.SetSupplierData(_currentSelectedEntity);
            newUpdateForm.ShowDialog();

            return Task.CompletedTask;
        }
    }
}