using BLL.DTOs;
using BLL.LogicLayers;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;

namespace Presenter.ForEmployee
{
    public interface ICreatePaymentView : IView
    {
        event EventHandler<PaymentDTO> CreatePaymentRequested;
        event EventHandler GetAllClientsRequested;

        void ShowOperationResult(OperationResult<PaymentDTO> opRes);
        void ShowOperationResult(OperationResult<ClientDTO> opRes);
        void CatchingClientList(List<ClientDTO> allClientsList);
    }
}
