using BLL.DTOs;
using Presenter.ForPayments;
using Shared;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Forms.Base;
using WinformsUI.Infrastructure.Factories;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls.CustomDGV;

namespace WinformsUI.Forms.PaymentCRUDL
{
    public partial class PaymentForm : BaseManagementForm<PaymentDTO>, IPaymentView
    {
        private readonly IFormsFactory _formsFactory;

        public PaymentForm
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
        // IMPLEMENTACIÓN DE IPaymentView (Mapeo de Eventos a Base)
        // =========================================================
        public event EventHandler CreatePaymentRequested
        {
            add => CreateRequested += value;
            remove => CreateRequested -= value;
        }

        public event EventHandler<PaymentDTO> UpdatePaymentRequested
        {
            add => UpdateRequested += value;
            remove => UpdateRequested -= value;
        }

        public event EventHandler<PaymentDTO> DeletePaymentRequested
        {
            add => DeleteRequested += value;
            remove => DeleteRequested -= value;
        }

        public event EventHandler CachingAllPaymentsRequested
        {
            add => ListAllRequested += value;
            remove => ListAllRequested -= value;
        }

        public event EventHandler ClosePaymentRequested
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
        // LÓGICA ESPECÍFICA DE PAGO
        // =========================================================
        public void OpenCreationView() => ((Form)_formsFactory.PaymentCreationForm()).ShowDialog();

        private void WireSpecificEvents()
        {
            btnCreate.Click += OnCreateRequest;
            btnUpdate.Click += OnUpdateRequest;
            btnDelete.Click += OnDeleteRequest;
            btnRefresh.Click += OnListAllRequest;
        }


        // =========================================================
        // CICLO DE VIDA (Lifecycle)
        // =========================================================

        // CloseView(): Eliminado por redundancia (implementado en BaseManagementForm)

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Desuscripción de eventos de controles específicos
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
            base.Dispose(disposing); // La base se encarga de desuscribir el evento de cierre global
        }

        public Task OpenUpdateView()
        {
            throw new NotImplementedException();
        }
    }
}