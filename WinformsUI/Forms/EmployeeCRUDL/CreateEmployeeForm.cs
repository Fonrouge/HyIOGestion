 using BLL.DTOs;
using Presenter.ForEmployee;
using Shared;
using System;
using System.Windows.Forms;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls.Wizard;

namespace WinformsUI.Forms.EmployeeCRUDL
{
    public partial class CreateEmployeeForm : Form, ICreateEmployeeView
    {
        private readonly IApplicationSettings _appSettings;
        private readonly IWizardPanelNavigator _wizard;
        public event EventHandler<EmployeeDTO> CreateEmployeeRequested;
        private readonly string _errorMsg;
        private readonly string _successMsg;

        public CreateEmployeeForm
        (
            IWizardPanelNavigator wizard,
            IApplicationSettings appSettings,
            ITranslatableControlsManager transMgr
        )
        {
            _wizard = wizard;
            _appSettings = appSettings;
            _successMsg = _appSettings.SuccessOnOperation;
            _errorMsg = _appSettings.ErrorOnOperation;
            _transMgr = transMgr;

            InitializeComponent();
            WireEvents();
            AddTranslatables();
        }

        private readonly ITranslatableControlsManager _transMgr;
        private void AddTranslatables()
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<Button>(this.Controls, "Text");

            ApplyTranslation();

            _transMgr.AddFormNotify(this);
        }
        public void NotifiedByTranslationManager() => ApplyTranslation();
        public void ApplyTranslation() => _transMgr.Apply();

        private void WireEvents()
        {
            btnNextId.Click += (s, e) => _wizard.Advance();
            btnBackContact.Click += (s, e) => _wizard.Back();
            btnFinish.Click += (s, e) => ExecuteUseCase();
        }

        private void ExecuteUseCase()
        {
            var newEmployee = new EmployeeDTO
            {
                FileNumber = txtFileNumber.Text, //A futuro con un generador de Legajos en BLL
                FirstName = txtName.Text,
                LastName = txtLastName.Text,
                NationalId = txtDocNumber.Text,
                Email = txtEmail.Text,
                PhoneNumber = txtPhone.Text,
                HomeAddress = txtAddress.Text
            };

            CreateEmployeeRequested?.Invoke(this, newEmployee);
        }


        public void ShowOperationResult(OperationResult<EmployeeDTO> opRes)
        {
            MessageBox.Show(opRes.Success ? $"{_successMsg}" : $"{_errorMsg}. Errors: {string.Join(", ", opRes.Errors)}");

            if (opRes.Success)
                this.Close();
        }
    }
}
