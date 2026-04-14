using BLL.DTOs;
using BLL.LogicLayers.Clients;
using Presenter.Messaging;
using System;

namespace Presenter.Presenters.ForClient
{
    public class UpdateClientPresenter
    {

        private readonly IUpdateClientView _view;
        private readonly IUCUpdateClient _ucUpdate;
        private readonly IMessenger _messenger;

        public UpdateClientPresenter
        (
            IUpdateClientView view,
            IUCUpdateClient ucUpdate,
            IMessenger messenger
        )
        {
            _view = view;
            _ucUpdate = ucUpdate;
            _messenger = messenger;

            WireViewEvents();
        }


        private void WireViewEvents()
        {
            _view.UpdateClientRequested += OnUpdateRequested;
            _view.MinimizeRequested += OnMinimizeWindowtRequest;
            _view.CloseRequested += OnCloseWindowRequest;
        }

        private async void OnUpdateRequested(object sender, ClientDTO client)
        {
            var opRes = await _ucUpdate.ExecuteAsync(client);
            _view.ShowOperationResult(opRes);

            if (opRes.Success)
            {
                SendRelistAsk();
            }
        }
        
        private void SendRelistAsk()
        {
            var relistClientsAsk = new ClientsRelistRequestMessage(this);
            _messenger.Send(relistClientsAsk);
        }

        private void OnMinimizeWindowtRequest(object sender, EventArgs e) => _view.MinimizeView();
        private void OnCloseWindowRequest(object sender, EventArgs e) => _view.CloseView();




    }
}
