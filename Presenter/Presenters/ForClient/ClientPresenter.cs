using SharedAbstractions.ArchitecturalMarkers;
using BLL.LogicLayers.Clients;
using System.Threading.Tasks;
using Presenter.Messaging;
using BLL.DTOs;
using System;

namespace Presenter.ForClient
{
    public class ClientPresenter : IPresenter, IDisposable
    {
        private readonly IClientView _view;
        private readonly IUCGetAllClients _ucGetAll;
        private readonly IUCDeleteClient _ucDelete;
        private readonly IMessenger _messenger;

        public ClientPresenter
        (
            IClientView view,
            IUCGetAllClients ucGetAll,
            IUCDeleteClient ucDelete,
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
        // Mensajería global
        // =========================================================
        private void SubscribeMessages()
        {
            _messenger.Subscribe<ClientsRelistRequestMessage>(OnGetAllRequested);            
            //Ready for more...
        }
        private void CloseHostFormMessage(object viewId) => _messenger.Send(new HostFormCloseRequestMessage((Guid)viewId, this));


        // =============================================================
        // Estética general
        // =============================================================
        private void ApplyDarkTheme() => _view.ThemingNotifiedByConfigurationsModule();


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
        private void HandleUpdateRequested(object sender, ClientDTO e) => OnUpdateRequested(e);
        private async void HandleDeleteRequested(object sender, ClientDTO e) => await OnDeleteRequested(e);
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
        private async Task OnDeleteRequested(ClientDTO client)
        {
            var opRes = await _ucDelete.ExecuteAsync(client);
            _view.ShowOperationResult(opRes);

            if (opRes.Success) await OnGetAllRequested();
        }

        private async void OnGetAllRequested(ClientsRelistRequestMessage message) => await OnGetAllRequested();

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

        private void OnUpdateRequested(ClientDTO client) => _view.OpenUpdateView();


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
                _messenger.Unsubscribe<ClientsRelistRequestMessage>(OnGetAllRequested);
            }
        }
    }
}