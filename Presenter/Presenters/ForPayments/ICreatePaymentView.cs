using BLL.LogicLayers;
using BLL.DTOs;
using SharedAbstractions.ArchitecturalMarkers;
using System;

namespace Presenter.ForEmployee
{
    public interface ICreatePaymentView : IView
    {
        event EventHandler<PaymentDTO> CreatePaymentRequested;
        void ShowOperationResult(OperationResult<PaymentDTO> opRes);
    }
}
