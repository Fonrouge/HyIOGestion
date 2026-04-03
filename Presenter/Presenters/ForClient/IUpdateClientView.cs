using BLL.DTOs;
using System;

namespace Presenter.Presenters.ForClient
{
    public interface IUpdateClientView
    {
        event EventHandler<ClientDTO> UpdateClientRequested;
        event EventHandler MinimizeRequested;
        event EventHandler CloseRequested;
        void ShowOperationResult(OperationResult<ClientDTO> opRes);

        void MinimizeView();
        void CloseView();
        void SetClientData(ClientDTO dto);
    }
}
