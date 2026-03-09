using BLL.LogicLayers.Clients;
using BLL.DTOs;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Linq;

namespace Presenter.ForClient
{
    public class ClientPresenter : IPresenter
    {
        private readonly IClientView _view;
        
        private readonly IUCGetAllClients _ucGetAll;
        private readonly IUCUpdateClient _ucUpdate;
        private readonly IUCDeleteClient _ucDelete;

        public ClientPresenter
        (
            IClientView view,
            IUCGetAllClients ucGetAll,
            IUCUpdateClient ucUpdate,
            IUCDeleteClient ucDelete
        )
        {
            _view = view;
            _ucGetAll = ucGetAll;
            _ucUpdate = ucUpdate;
            _ucDelete = ucDelete;

            WireEvents();
            ApplyDarkTheme();
        }

        private void WireEvents()
        {
            // Mapeo de eventos GENÉRICOS (ICrudView) a métodos del Presenter
            _view.CreateRequested += (s, e) => OnOpenCreationForm();

            // El 'e' aquí ya viene tipado como ClientDTO gracias a la interfaz genérica
            _view.UpdateRequested += (s, e) => UpdateClient(e);
            _view.DeleteRequested += (s, e) => DeleteClient(e);
            _view.ListAllRequested += (s, e) => GetAllClients();

            // Disparar ante notificación de cambio de idioma
       //     _view.TranslationRequested += (s, e) => _view.ApplyGlobalLanguage();
        }

        private void ApplyDarkTheme() => _view.ApplyGlobalPalette();

        private void OnOpenCreationForm() => _view.OpenCreationForm();

        private async void UpdateClient(ClientDTO c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));

            // Execute debería ser awaitable
            var opRes = await _ucUpdate.ExecuteAsync(c);
            ShowResult(opRes);

            // Opcional: Recargar la lista si la operación fue exitosa / Oportunidad de refactor: Chequear por BindingList y evitar este paso.
            if (!opRes.Errors.Any()) GetAllClients();
        }

        private async void DeleteClient(ClientDTO c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));

            var opRes = await _ucDelete.ExecuteAsync(c);
            ShowResult(opRes);

            // Opcional: Recargar la lista si la operación fue exitosa / Oportunidad de refactor: Chequear por BindingList y evitar este paso.
            if (!opRes.Success) GetAllClients();
        }

        private async void GetAllClients()
        {
            var tuple = await _ucGetAll.ExecuteAsync();

            var clientList = tuple.Item1;
            var opResult = tuple.Item2;

            if (opResult.Errors.Any())
            {
                ShowResult(opResult);
            }
            else
            {
                try
                {
                    _view.CachingList(clientList);
                }
                finally
                {
                    _view.FillDGV();
                }
            }
        }

        private void ShowResult(OperationResult<ClientDTO> opRes)
            => _view.ShowOperationResult(opRes);
    }
}