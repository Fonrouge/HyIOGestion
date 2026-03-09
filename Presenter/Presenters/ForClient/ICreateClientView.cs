using BLL.DTOs;
using SharedAbstractions.ArchitecturalMarkers;
using System;

namespace Presenter.ForClient
{
    public interface ICreateClientView: IView
    {
        event EventHandler<ClientDTO> CreateClientRequested;
        void ShowOperationResult(OperationResult<ClientDTO> opRes);

      
    }
}
