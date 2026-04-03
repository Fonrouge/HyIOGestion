using BLL.DTOs;
using Presenter.ForClient;
using Presenter.Presenters.ForClient;
using Shared;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Forms.Base;
using WinformsUI.Infrastructure.Factories;
using WinformsUI.Infrastructure.Shortcuts;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls.CustomDGV;

namespace WinformsUI.Forms.ClientCRUDL
{
    public partial class ClientForm : BaseManagementForm<ClientDTO>, IClientView
    {
        public event EventHandler CloseRequested;
        private readonly IFormsFactory _formsFact;
        private ShortcutManager _shortcutMgr;

        public ClientForm
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

            SetShortcuts();
            
        }

        private void SetShortcuts() => _shortcutMgr = ShortcutManager.Attach(this)
            .Add("ctrl+b", () => MessageBox.Show("1"))
            .Add("Ctrl+B", () => MessageBox.Show("2"))
            .Add("CTRL+B", () => MessageBox.Show("3"))
            .Add("CTRL+b", () => MessageBox.Show("4"))
            .Add("CTRL+1", () => MessageBox.Show("4"))
            .Add("ctrl+f", () => _dgvForm.OpenFiltersPanel());


        private void FocusSearchBar()
        {
            _dgvForm.FocusSearchBar();
        }

        // =========================================================
        // IMPLEMENTACIÓN DE IClientView (Mapeo de Eventos a Base)
        // =========================================================

        public event EventHandler CreateClientRequested
        {
            add => CreateRequested += value;
            remove => CreateRequested -= value;
        }

        public event EventHandler<ClientDTO> UpdateClientRequested
        {
            add => UpdateRequested += value;
            remove => UpdateRequested -= value;
        }

        public event EventHandler<ClientDTO> DeleteClientRequested
        {
            add => DeleteRequested += value;
            remove => DeleteRequested -= value;
        }

        public event EventHandler CachingAllClientsRequested
        {
            add => ListAllRequested += value;
            remove => ListAllRequested -= value;
        }

        public event EventHandler CloseWindowRequested
        {
            add => base.CloseRequested += value;
            remove => base.CloseRequested -= value;
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
        // LÓGICA ESPECÍFICA DE CLIENTE
        // =========================================================

        public void OpenCreationView() => ((Form)_formsFact.ClientCreationForm<ICreateClientView>()).ShowDialog();
        public Task OpenUpdateView()
        {
            if (_currentSelectedEntity == null)
            {
                MessageBox.Show("Primero seleccione un cliente en la grilla");
                return Task.CompletedTask;
            }

            var newUpdateForm = (UpdateClientForm)_formsFact.ClientUpdateForm<IUpdateClientView>();
            newUpdateForm.SetClientData(_currentSelectedEntity);
            newUpdateForm.ShowDialog();

            return Task.CompletedTask;
        }

        protected override void OnEntitySelected(ClientDTO entity)
        {
            // Hook opcional para acciones dependientes de selección
        }

        private void WireSpecificEvents()
        {
            btnCreate.Click += OnCreateRequest;
            btnUpdate.Click += OnUpdateRequest;
            btnDelete.Click += OnDeleteRequest;
            btnRefresh.Click += OnListAllRequest;

            this.FormClosed += HandleFormClosed;
        }

        private void HandleFormClosed(object sender, FormClosedEventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);


        // =============================================================================================================
        // LINKEO DE CONTROLES (instancia genérica -de BaseForm- ahora apunta a instancia específica de este formulario)
        // =============================================================================================================
        private void InitializeDGV()
        {
            _dgvControls = DGVFunctionsControl;
            base.InitializeDGV(this.dgvPanel);
            base.InitializeDGVControls();
        }


        // =========================================================
        // CICLO DE VIDA (Lifecycle)
        // =========================================================
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Desuscripción de eventos
                this.FormClosed -= HandleFormClosed;

                if (btnCreate != null) btnCreate.Click -= OnCreateRequest;
                if (btnUpdate != null) btnUpdate.Click -= OnUpdateRequest;
                if (btnDelete != null) btnDelete.Click -= OnDeleteRequest;
                if (btnRefresh != null) btnRefresh.Click -= OnListAllRequest;

                // Limpieza del toolstrip
                if (DGVFunctionsControl != null)
                {
                    DGVFunctionsControl.TargetDGV = null;
                    DGVFunctionsControl.Dispose();
                    DGVFunctionsControl = null;
                }

                _entitiesList = null;
                _dgvForm = null;
                _transMgr.RemoveFormNotify(this);

                if (components != null)
                    components.Dispose();
            }
        }


    }
}