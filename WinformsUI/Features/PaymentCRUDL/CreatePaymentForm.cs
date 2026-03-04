using BLL.DTOs;
using BLL.LogicLayers;
using Presenter.ForEmployee;
using Shared;
using System;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Infrastructure.Translations;

namespace WinformsUI.Features.PaymentCRUDL
{
    public partial class CreatePaymentForm : Form, ICreatePaymentView
    {
        private readonly IApplicationSettings _appSettings;
        private readonly string _errorMsg;
        private readonly string _successMsg;

        public event EventHandler<PaymentDTO> CreatePaymentRequested;

        public CreatePaymentForm
        (
            IApplicationSettings appSettings,
            ITranslatableControlsManager transMgr
        )
        {
            _appSettings = appSettings;
            _transMgr = transMgr;

            _successMsg = _appSettings.SuccessOnOperation;
            _errorMsg = _appSettings.ErrorOnOperation;
            

            InitializeComponent();

            ApplyDarkTheme();
            WireFlowButtonsEvents();
            UpdateFormSize();
            ApplyGlobalPalette();
            AddTranslatables();
        }
        public void ApplyGlobalPalette() => DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());
        private void UpdateFormSize()
        {
            this.ClientSize = tlpAddPayment.Size;
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

        /// <summary>
        /// Limpia los recursos utilizados por el formulario.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CreatePaymentRequested = null;

                if (components != null)
                    components.Dispose();

            }

            base.Dispose(disposing);
        }

        public void ShowOperationResult(OperationResult<PaymentDTO> opRes)
        {
            MessageBox.Show(opRes.Success ? $"{_successMsg}" : $"{_errorMsg}. Errors: {string.Join(", ", opRes.Errors)}");
            this.Close();
        }
        private void ApplyDarkTheme()
        {
            DarkTheme.RedrawBorders = true;
            DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());
        }


        private void WireFlowButtonsEvents()
        {
            btnFinish.Click += (s, e) => ExecuteCreation();
        }


        private void ExecuteCreation()
        {
            var dto = new PaymentDTO()
            {
                //Id = Automático
                Amount = txtAmount.Text,
                EffectiveDate = txtEffectiveDate.Text,
                ClientId = txtClient.Text,
                Method = txtMethod.Text,
                Reference = txtReference.Text
            };


            CreatePaymentRequested?.Invoke(this, dto);

        }
    }
}
