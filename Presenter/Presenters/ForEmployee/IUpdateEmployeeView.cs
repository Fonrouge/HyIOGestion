using BLL.DTOs;
using System;

namespace Presenter.Presenters.ForEmployee
{
    public interface IUpdateEmployeeView
    {         
        event EventHandler<EmployeeDTO> UpdateEmployeeRequested;
        event EventHandler MinimizeRequested;
        event EventHandler CloseRequested;
        void ShowOperationResult(OperationResult<EmployeeDTO> opRes);

        void MinimizeView();
        void CloseView();
        void SetEmployeeData(EmployeeDTO dto);
    }
}
