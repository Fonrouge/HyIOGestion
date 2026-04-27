using BLL.DTOs;
using BLL.LogicLayers;
using Presenter.ForSale;
using Shared;
using System.Windows.Forms;
using WinformsUI.Forms.Base;
using WinformsUI.Infrastructure.Factories;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls.CustomDGV;

namespace WinformsUI.Forms.SaleCRUDL
{

    public partial class SaleForm : BaseManagementForm<SaleDTO>, ISaleView
    {
        private readonly IFormsFactory _formsFact;


        public SaleForm
        (
            IApplicationSettings appSettings,
            ITranslatableControlsManager transMgr,
            ICustomDGVFactory dgvFact,
            IFormsFactory formsFact

        ) : base(appSettings, transMgr, dgvFact)

        {
            _formsFact = formsFact;

            InitializeComponent();
            WireSpecificEvents();
            AddTranslatables();
            InitializeBaseForm();
        }



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
        public void OpenCreationView() => ((Form)_formsFact.SaleCreationForm()).ShowDialog();
        public void OpenUpdateView()
        {
            if (_currentSelectedEntity == null)
            {
                MessageBox.Show("Primero seleccione un cliente en la grilla");
            }

//            var newUpdateForm = (UpdateSaleForm)_formsFact.ClientUpdateForm<IUpdateSaleView>();
//            newUpdateForm.SetClientData(_currentSelectedEntity);
//            newUpdateForm.ShowDialog();
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
            this.Load += OnceLoaded;
        }


        // =============================================================================================================
        // LINKEO DE CONTROLES (instancia genérica -de BaseForm- ahora apunta a instancia específica de este formulario)
        // =============================================================================================================
        private void InitializeBaseForm()
        {
            base.BaseFormInitializer
            (
                CustomDgvContainer: pnlDgv,
                ExpandedRibbonsContainer: pnlExpandedRibbons,
                CollapsedRibbonsContainer: pnlCollapsedRibbons,
                CollapsedRibbonTLP: miniTLP,
                RibbonCollapserButton: btnRibbonCollapser,
                RibbonExpanderButton: btnRibbonExpander,
                DgvFunctionalitiesRibbon: ribbonDgvFunctions,
                DirectButtonsToEyeRestModesRibbon: ribbonEyeRest,
                FeedbackBarContainer: pnlFeedbackBar
            );
        }


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
                if (ribbonDgvFunctions != null)
                {
                    ribbonDgvFunctions.TargetDGV = null;
                    ribbonDgvFunctions.Dispose();
                    ribbonDgvFunctions = null;
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
