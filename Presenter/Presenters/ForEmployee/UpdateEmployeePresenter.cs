using BLL.DTOs;
using BLL.LogicLayers.Employees;
using Presenter.Messaging;
using System;

namespace Presenter.Presenters.ForEmployee
{
    public class UpdateEmployeePresenter
    {

        private readonly IUpdateEmployeeView _view;
        private readonly IUCUpdateEmployee _ucUpdate;
        private readonly IMessenger _messenger;

        private readonly Guid _messageId = Guid.NewGuid();

        public UpdateEmployeePresenter
        (
            IUpdateEmployeeView view,
            IUCUpdateEmployee ucUpdate,
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
            _view.UpdateEmployeeRequested += OnUpdateRequested;
            _view.MinimizeRequested += OnMinimizeWindowtRequest;
            _view.CloseRequested += OnCloseWindowRequest;
        }

        private async void OnUpdateRequested(object sender, EmployeeDTO employee)
        {
            var opRes = await _ucUpdate.ExecuteAsync(employee);
            _view.ShowOperationResult(opRes);

            if (opRes.Success)
            {
                SendRelistAsk();
            }
        }
        private void SendRelistAsk()
        {
            var relistSuppliersAsk = new RelistClientsMessage(_messageId, this);
            _messenger.Send(relistSuppliersAsk);
        }

        private void OnMinimizeWindowtRequest(object sender, EventArgs e) => _view.MinimizeView();
        private void OnCloseWindowRequest(object sender, EventArgs e) => _view.CloseView();

    }
}
