using BLL.DTOs;
using System;

namespace Presenter.Presenters.ForSupplier
{
    public interface IUpdateSupplierView
    {
        event EventHandler<SupplierDTO> UpdateSupplierRequested;
        event EventHandler MinimizeRequested;
        event EventHandler CloseRequested;
        void ShowOperationResult(OperationResult<SupplierDTO> opRes);

        void MinimizeView();
        void CloseView();
        void SetSupplierData(SupplierDTO dto);
    }
}
