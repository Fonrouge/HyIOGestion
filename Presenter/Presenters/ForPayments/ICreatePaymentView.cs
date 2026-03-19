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
        event EventHandler GetAllSalesRequested;
        event EventHandler CloseRequested;

        void FillPaymentMethods(IEnumerable<object> countries);
        void ShowOperationResult(OperationResult<PaymentDTO> opRes);
        void ShowOperationResult(OperationResult<SaleDTO> opRes);
        void InitializeGrid(List<SaleDTO> allClientsList);
        void Dispose();
    }
}
