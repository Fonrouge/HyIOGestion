using BLL.DTOs;
using BLL.LogicLayers.Clients;
using Presenter.Messaging;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Threading.Tasks;

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
            SubscribeToMessenger();
        }

        // =========================================================
        // SUSCRIPCIÓN A MENSAJERÍA GLOBAL
        // =========================================================
        private void SubscribeToMessenger() => _messenger.Subscribe<RelistClientsMessage>(OnGetAllRequested);


        private void ApplyDarkTheme() => _view.ApplyGlobalPalette();


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
        }


        // =========================================================
        // Event Handlers (Orquestación de UI)
        // =========================================================
        private void HandleCreateRequested(object sender, EventArgs e) => _view.OpenCreationView();
        private async void HandleUpdateRequested(object sender, ClientDTO e) => await OnUpdateRequested(e);
        private async void HandleDeleteRequested(object sender, ClientDTO e) => await OnDeleteRequested(e);
        private async void HandleListAllRequested(object sender, EventArgs e) => await OnGetAllRequested();
        private void HandleCloseRequested(object sender, EventArgs e) => Dispose();


        // =========================================================
        // Lógica de Casos de Uso
        // =========================================================

        private async Task OnDeleteRequested(ClientDTO client)
        {
            var opRes = await _ucDelete.ExecuteAsync(client);
            _view.ShowOperationResult(opRes);

            if (opRes.Success) await OnGetAllRequested();
        }

        private async void OnGetAllRequested(RelistClientsMessage message) => await OnGetAllRequested();
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

        private async Task OnUpdateRequested(ClientDTO client) => await _view.OpenUpdateView();



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
            }
        }
    }
}