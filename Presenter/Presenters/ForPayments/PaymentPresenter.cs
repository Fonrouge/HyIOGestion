using BLL.DTOs;
using BLL.LogicLayers.Payments;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Threading.Tasks;

namespace Presenter.ForPayments
{
    public class PaymentPresenter : IPresenter, IDisposable
    {
        private readonly IPaymentView _view;
        private readonly IUCGetAllPayments _ucGetAll;
        private readonly IUCUpdatePayment _ucUpdate;
        private readonly IUCDeletePayment _ucDelete;

        public PaymentPresenter
        (
            IPaymentView view,
            IUCGetAllPayments ucGetAll,
            IUCUpdatePayment ucUpdate,
            IUCDeletePayment ucDelete
        )
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _ucGetAll = ucGetAll ?? throw new ArgumentNullException(nameof(ucGetAll));
            _ucUpdate = ucUpdate ?? throw new ArgumentNullException(nameof(ucUpdate));
            _ucDelete = ucDelete ?? throw new ArgumentNullException(nameof(ucDelete));

            WireViewEvents();
            ApplyDarkTheme();
        }

        private void ApplyDarkTheme() => _view.ApplyGlobalPalette();

        private void WireViewEvents()
        {
            // Suscripción explícita para permitir desuscripción en Dispose
            _view.CreateRequested += HandleCreateRequested;
            _view.UpdateRequested += HandleUpdateRequested;
            _view.DeleteRequested += HandleDeleteRequested;
            _view.ListAllRequested += HandleListAllRequested;
            _view.CloseRequested += HandleCloseRequested;
        }


        // =========================================================
        // Event Handlers (Orquestación de UI)
        // =========================================================
        private void HandleCreateRequested(object sender, EventArgs e) => _view.OpenCreationView();
        private async void HandleUpdateRequested(object sender, PaymentDTO e) => await OnUpdateRequested(e);
        private async void HandleDeleteRequested(object sender, PaymentDTO e) => await OnDeleteRequested(e);
        private async void HandleListAllRequested(object sender, EventArgs e) => await OnGetAllRequested();
        private void HandleCloseRequested(object sender, EventArgs e) => Dispose();


        // =========================================================
        // Lógica de Casos de Uso (Task-based)
        // =========================================================
        private async Task OnUpdateRequested(PaymentDTO payment)
        {
            var opRes = await _ucUpdate.ExecuteAsync(payment);
            _view.ShowOperationResult(opRes);

            if (opRes.Success) await OnGetAllRequested();
        }

        private async Task OnDeleteRequested(PaymentDTO payment)
        {
            var opRes = await _ucDelete.ExecuteAsync(payment);
            _view.ShowOperationResult(opRes);

            if (opRes.Success) await OnGetAllRequested();
        }

        private async Task OnGetAllRequested()
        {
            var (payments, opResult) = await _ucGetAll.ExecuteAsync();

            if (opResult.Success)
            {
                _view.CachingList(payments);
                _view.FillDGV();
            }
            else
            {
                _view.ShowOperationResult(opResult);
            }
        }


        // =========================================================
        // IDisposable (Prevención de fugas de memoria)
        // =========================================================
        public void Dispose()
        {
            if (_view != null)
            {
                _view.CreateRequested -= HandleCreateRequested;
                _view.UpdateRequested -= HandleUpdateRequested;
                _view.DeleteRequested -= HandleDeleteRequested;
                _view.ListAllRequested -= HandleListAllRequested;
                _view.CloseRequested -= HandleCloseRequested;
            }
        }
    }
}