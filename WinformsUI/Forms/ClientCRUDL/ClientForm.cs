using BLL.DTOs;
using Presenter.ForClient;
using Presenter.Presenters.ForClient;
using Shared;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinformsUI.Forms.Base;
using WinformsUI.Infrastructure.Factories;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls;
using WinformsUI.UserControls.CustomDGV;

namespace WinformsUI.Forms.ClientCRUDL
{
    /*
    BaseManagementForm<ClientDTO>
    Form
    */
    public partial class ClientForm : BaseManagementForm<ClientDTO>, IClientView
    {
        private readonly IFormsFactory _formsFact;
        public override event EventHandler OnceLoadedAdvice;

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

            InitializeRibbonControls();
            InitializePanelToggle();

            this.Load += OnceLoaded;



            base.gigaRibbonContainer = gigaRibbonPanel;
            base.miniRibbonContainer = miniRibbonPanel;

            base.miniRibbonTLP = miniTLP;


            base.AttachBtnToggleRibbon(btnRibbonCollapser);
            base.AttachBtnToggleRibbon(btnRibbonExpander);
            base.AttachFeedbackBarContainer(panel1);

            DoubleBuffering.TryForAllControls(this.Controls);
        }

        public void IdleFeedbackPanel(object sender, EventArgs e) { } //borrar, es para que compile la poronga del diseñador. después sacar el enlace que tiene el botón a este método

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
        // LÓGICA ESPECÍFICA DE CLIENTE
        // =========================================================
        public void OpenCreationView() => ((Form)_formsFact.ClientCreationForm<ICreateClientView>()).ShowDialog();
        public void OpenUpdateView()
        {
            if (_currentSelectedEntity == null)
            {
                MessageBox.Show("Primero seleccione un cliente en la grilla");
            }

            var newUpdateForm = (UpdateClientForm)_formsFact.ClientUpdateForm<IUpdateClientView>();
            newUpdateForm.SetClientData(_currentSelectedEntity);
            newUpdateForm.ShowDialog();
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
        }


        // =============================================================================================================
        // LINKEO DE CONTROLES (instancia genérica -de BaseForm- ahora apunta a instancia específica de este formulario)
        // =============================================================================================================
        private void InitializeDGV() => base.InitializeDGV(this.dgvPanel);

        private new void InitializeRibbonControls()
        {
            _dgvRibbonControls = DgvFunctionsRibbon;
            _eyeRestRibbonControls = EyeRestRibbon;
            base.InitializeRibbonControls();
        }

        public void InitializePanelToggle() => base.ToolStripsPanelToggle(gigaRibbonPanel);


        // =========================================================
        // CICLO DE VIDA (Lifecycle)
        // =========================================================
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Desuscripción de eventos
                if (btnCreate != null) btnCreate.Click -= OnCreateRequest;
                if (btnUpdate != null) btnUpdate.Click -= OnUpdateRequest;
                if (btnDelete != null) btnDelete.Click -= OnDeleteRequest;
                if (btnRefresh != null) btnRefresh.Click -= OnListAllRequest;

                // Limpieza del toolstrip
                if (DgvFunctionsRibbon != null)
                {
                    DgvFunctionsRibbon.TargetDGV = null;
                    DgvFunctionsRibbon.Dispose();
                    DgvFunctionsRibbon = null;
                }

                _entitiesList = null;
                _dgvForm = null;
                _transMgr.RemoveFormNotify(this);
                _dgvRibbonControls = null;
                _eyeRestRibbonControls = null;

                if (components != null)
                    components.Dispose();
            }
        }



        //====================================================================================
        //                   PARA LLAMADOS INTERNOS DESDE BOTONES    (etapa debug y testing)
        //====================================================================================
        private void btnDebugCargar(object sender, EventArgs e) => SetFeedbackState(FeedbackState.Loading);
        private void btnDebugSuccess(object sender, EventArgs e) => SetFeedbackState(FeedbackState.Success);
        private void btnDebugError(object sender, EventArgs e) => SetFeedbackState(FeedbackState.Error);
        private void btnDebugActiveToogle(object sender, EventArgs e) => base.ChangeActivationStateFeedbackBar();


        //====================================================================================
        //                   API PÚBLICA PARA PRESENTER (etapa debug y testing)
        //====================================================================================       

        public async Task TryLoadingState() => await SetFeedbackState(FeedbackState.Loading);
        public async Task TrySuccessCommand() => await SetFeedbackState(FeedbackState.Success);
        public async Task TryErrorCommand() => await SetFeedbackState(FeedbackState.Error);
    }
}