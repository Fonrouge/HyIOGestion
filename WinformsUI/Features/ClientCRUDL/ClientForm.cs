using BLL.DTOs;
using Presenter.ForClient;
using Shared;
using System;
using System.Linq;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Forms.Base;
using WinformsUI.Infrastructure.Factories;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls.CustomDGV;

namespace WinformsUI.Features.ClientCRUDL
{
    public partial class ClientForm : BaseManagementForm<ClientDTO>, IClientView
    {

        private readonly IFormsFactory _formsFact;

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

            InitializeDGV(this.dgvPanel);
            WireSpecificEvents();
            AddTranslatables();
        }

        // Mapeamos los eventos de la interfaz a los eventos de la clase base
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


        public void ShowOperationResult(OperationResult<ClientDTO> opRes)
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

        private void AddTranslatables()  //Se desuscribe en la clase padre BaseManagementForm FormClosed() => _transMgr.RemoveFormNotify(this);
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

        public void OpenCreationForm()
        {
            // Lógica específica: Usar el Factory de Clientes
            var form = _formsFact.ClientCreationForm<ICreateClientView>(this as IClientView);
            ((Form)form).Show();
        }


        protected override void OnEntitySelected(ClientDTO entity)
        {
            // Hook opcional: Para habilitar/deshabilitar el botón de Venta
            // btnGenerateSale.Enabled = entity != null;
        }

        private void WireSpecificEvents()
        {
            // Conectamos los clicks de los botones a los métodos protegidos del PADRE
            btnCreate.Click += (s, e) => OnCreateRequest();
            btnUpdate.Click += (s, e) => OnUpdateRequest();
            btnDelete.Click += (s, e) => OnDeleteRequest();
            btnRefresh.Click += (s, e) => OnListAllRequest();


        }


    }
}