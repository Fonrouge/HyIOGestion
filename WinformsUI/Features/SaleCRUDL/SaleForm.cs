using BLL.DTOs;
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

namespace WinformsUI.Features.SaleCRUDL
{
    public partial class SaleForm : BaseManagementForm<SaleDTO>, ISaleView
    {
        private readonly IFormsFactory _formsFactory;

        public SaleForm
        (
            IApplicationSettings appSettings,
            ITranslatableControlsManager transMgr,
            ICustomDGVFactory dgvFact,
            IFormsFactory formsFact)
            : base(appSettings, transMgr, dgvFact
        )
        {
            _formsFactory = formsFact;

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
        // IMPLEMENTACIÓN DE IEmployeeView (Mapeo de Eventos)
        // =========================================================

        public event EventHandler CreateSaleRequested
        {
            add => CreateRequested += value;
            remove => CreateRequested -= value;
        }

        public event EventHandler<SaleDTO> UpdateSaleRequested
        {
            add => UpdateRequested += value;
            remove => UpdateRequested -= value;
        }

        public event EventHandler<SaleDTO> DeleteSaleRequested
        {
            add => DeleteRequested += value;
            remove => DeleteRequested -= value;
        }

        public event EventHandler CachingAllSaleRequested
        {
            add => ListAllRequested += value;
            remove => ListAllRequested -= value;
        }


        // Los métodos CachingList, FillDGV, ShowOperationResult, ApplyTranslation
        // ya están implementados en la clase Base y coinciden con la firma.
        public new void ApplyGlobalPalette() => DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());


        // =========================================================
        // LÓGICA ESPECÍFICA DE VENTA
        // =========================================================

        public void OpenCreationForm() => ((Form)_formsFactory.SaleCreationForm(this)).Show();

        public void ShowOperationResult(OperationResult<SaleDTO> opRes)
        {
            if (opRes.Success) { }

            else
            {
                foreach (ErrorLogDTO error in opRes.Errors)
                {
                    MessageBox.Show($"{error.Message} \n {error.RecommendedAction}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private void WireSpecificEvents()
        {
            // Conectamos los botones visuales a la lógica base
            btnCreate.Click += (s, e) => OnCreateRequest();
            btnUpdate.Click += (s, e) => OnUpdateRequest();
            btnDelete.Click += (s, e) => OnDeleteRequest();
            btnRefresh.Click += (s, e) => OnListAllRequest();
        }
    }

}
