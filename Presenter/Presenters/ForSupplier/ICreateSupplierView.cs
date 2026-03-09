using BLL.DTOs;
using BLL.LogicLayers;
using System;

namespace Presenter.ForSupplier
{
    public interface ICreateSupplierView
    {
        event EventHandler<SupplierDTO> CreateSupplierRequested;
        void ShowOperationResult(OperationResult<SupplierDTO> opRes);
    }
}
