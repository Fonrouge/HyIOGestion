using BLL.DTOs;
using BLL.LogicLayers;
using System;

namespace Presenter.ForSupplier
{
    public interface ICreateSupplierView
    {
        event EventHandler<SupplierDTO> CreateSupplierRequested;
        event EventHandler CloseRequested;
        void ShowOperationResult(OperationResult<SupplierDTO> opRes);
        void ThemingNotifiedByConfigurationsModule();
    }
}
