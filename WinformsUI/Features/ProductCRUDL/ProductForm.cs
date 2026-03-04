using BLL.LogicLayers;
using BLL.DTOs;
using Presenter.ForProducts;
using Shared;
using System;
using System.Linq;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Forms.Base;
using WinformsUI.Infrastructure.Factories;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls.CustomDGV;
using System.Collections.Generic;

namespace WinformsUI.Features.ProductCRUDL
{
    public partial class ProductForm : BaseManagementForm<ProductDTO>, IProductView //BaseManagementForm<ProductDTO>
    {
        private readonly IFormsFactory _formsFactory;

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

            InitializeDGV(this.dgvPanel);

            WireSpecificEvents();
            WireCommonEvents();
            AddTranslatables();
            DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());
        }


        
        public void SetSearchFilters<T>(IEnumerable<T> categories) where T: CategoryDTO
        {
            _dgvForm.ConfigureFilters<CategoryDTO>(categories.ToList());
        }


        private void WireCommonEvents()
        {




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

        public event EventHandler CreateEmployeeRequested
        {
            add => CreateRequested += value;
            remove => CreateRequested -= value;
        }

        public event EventHandler<ProductDTO> UpdateEmployeeRequested
        {
            add => UpdateRequested += value;
            remove => UpdateRequested -= value;
        }

        public event EventHandler<ProductDTO> DeleteEmployeeRequested
        {
            add => DeleteRequested += value;
            remove => DeleteRequested -= value;
        }

        public event EventHandler CachingAllEmployeesRequested
        {
            add => ListAllRequested += value;
            remove => ListAllRequested -= value;
        }



        // Los métodos CachingList, FillDGV, ShowOperationResult, ApplyTranslation
        // ya están implementados en la clase Base y coinciden con la firma.

        // =========================================================
        // LÓGICA ESPECÍFICA DE EMPLEADO
        // =========================================================

        public void OpenCreationForm() => ((Form)_formsFactory.ProductCreationForm(this)).Show();

        public new void ApplyGlobalPalette() => DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());

        public void ShowOperationResult(OperationResult<ProductDTO> opRes)
        {
            if (!opRes.Errors.Any()) MessageBox.Show("Ok" + $"No");

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
