using BLL.DTOs;
using BLL.LogicLayers.Suppliers;
using Presenter.Messaging;
using System;

namespace Presenter.Presenters.ForSupplier
{
    public class UpdateSupplierPresenter
    {

        private readonly IUpdateSupplierView _view;
        private readonly IUCUpdateSupplier _ucUpdate;
        private readonly IMessenger _messenger;

        private readonly Guid _messageId = Guid.NewGuid();

        public UpdateSupplierPresenter
        (
            IUpdateSupplierView view,
            IUCUpdateSupplier ucUpdate,
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
            _view.UpdateSupplierRequested += OnUpdateRequested;
            _view.MinimizeRequested += OnMinimizeWindowtRequest;
            _view.CloseRequested += OnCloseWindowRequest;
        }

        private async void OnUpdateRequested(object sender, SupplierDTO supplier)
        {
            var opRes = await _ucUpdate.ExecuteAsync(supplier);
            _view.ShowOperationResult(opRes);

            if (opRes.Success)
            {
                SendRelistAsk();
            }
        }
        private void SendRelistAsk()
        {
            var relistSuppliersAsk   = new RelistClientsMessage(_messageId, this);
            _messenger.Send(relistSuppliersAsk);
        }

        private void OnMinimizeWindowtRequest(object sender, EventArgs e) => _view.MinimizeView();
        private void OnCloseWindowRequest(object sender, EventArgs e) => _view.CloseView();

    }
}
