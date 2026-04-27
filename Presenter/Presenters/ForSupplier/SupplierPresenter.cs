using BLL.DTOs;
using BLL.LogicLayers.Suppliers;
using Presenter.Messaging;
using SharedAbstractions.ArchitecturalMarkers;
using SharedAbstractions.Enums;
using System;
using System.Threading.Tasks;

namespace Presenter.ForSupplier
{
    public class SupplierPresenter : IPresenter, IDisposable
    {
        private readonly ISupplierView _view;
        private readonly IUCGetAllSuppliers _ucGetAll;
        private readonly IUCDeleteSupplier _ucDelete;
        private readonly IMessenger _messenger;

        public SupplierPresenter
        (
            ISupplierView view,
            IUCGetAllSuppliers ucGetAll,
            IUCDeleteSupplier ucDelete,
            IMessenger messenger
        )
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _ucGetAll = ucGetAll ?? throw new ArgumentNullException(nameof(ucGetAll));
            _ucDelete = ucDelete ?? throw new ArgumentNullException(nameof(ucDelete));
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));

            WireViewEvents();
            ApplyDarkTheme();
            SubscribeMessages();
        }


        // =========================================================
        // Estética general
        // =========================================================
        private void ApplyDarkTheme() => _view.ThemingNotifiedByConfigurationsModule();


        // =========================================================
        // Suscripción a mensajería global
        // =========================================================
        private void SubscribeMessages()
        {
            _messenger.Subscribe<SuppliersRelistRequestMessage>(OnGetAllRequested);
            _messenger.Subscribe<ViewContainerGotFocusNotificationMessage>(GotFocusHostForm);
            //Ready for more...
        }
        private void CloseHostFormMessage(object viewId) => _messenger.Send(new ViewContainerCloseRequestMessage((Guid)viewId, this));
        private async void GotFocusHostForm(ViewContainerGotFocusNotificationMessage message)
        {
            if (message.Payload == _view.ViewId)
            {
                _view.ChangeActivationStateFeedbackBar(true);
                await Task.WhenAll(_view.SetFeedbackState(FeedbackState.Idle));
            }
            else
            {
                _view.ChangeActivationStateFeedbackBar(false);
            }
        }


        // =============================================================
        // Suscripción a eventos no anónimos para desuscribir al cerrar
        // =============================================================
        private void WireViewEvents()
        {
            _view.CreateRequested += HandleCreateRequested;
            _view.UpdateRequested += HandleUpdateRequested;
            _view.DeleteRequested += HandleDeleteRequested;
            _view.ListAllRequested += HandleListAllRequested;
            _view.CloseRequested += HandleCloseRequested;
            _view.OnceLoadedAdvice += HandleLoadedOperations;
        }


        // =========================================================
        // Event Handlers (Orquestación de UI)
        // =========================================================
        private void HandleCreateRequested(object sender, EventArgs e) => _view.OpenCreationView();
        private void HandleUpdateRequested(object sender, SupplierDTO e) => OnUpdateRequested(e);
        private async void HandleDeleteRequested(object sender, SupplierDTO e) => await OnDeleteRequested(e);
        private async void HandleListAllRequested(object sender, EventArgs e) => await OnGetAllRequested();
        private void HandleCloseRequested(object sender, EventArgs e)
        {
            CloseHostFormMessage((Guid)sender);
            Dispose();
            _view.Dispose();
        }

        private void HandleLoadedOperations(object sender, EventArgs e) => _view.SelectFirstGridRow();


        // =========================================================
        // Lógica de Casos de Uso
        // =========================================================
        private async Task OnDeleteRequested(SupplierDTO supplier)
        {
            var opRes = await _ucDelete.ExecuteAsync(supplier);
            _view.ShowOperationResult(opRes);

            if (opRes.Success) await OnGetAllRequested();
        }

        private async void OnGetAllRequested(SuppliersRelistRequestMessage message) => await OnGetAllRequested();

        private async Task OnGetAllRequested()
        {
            var (clients, opResult) = await _ucGetAll.ExecuteAsync();

            if (opResult.Success)
            {
                _view.CachingList(clients);
                _view.FillDGV();
            }
            else
            {
                _view.ShowOperationResult(opResult);
            }
        }

        private void OnUpdateRequested(SupplierDTO client) =>  _view.OpenUpdateView();


        // =========================================================
        // IDisposable (Limpieza de Memoria)
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
                _messenger.Unsubscribe<SuppliersRelistRequestMessage>(OnGetAllRequested);
                _messenger.Unsubscribe<ViewContainerGotFocusNotificationMessage>(GotFocusHostForm);
            }
        }
    }
}