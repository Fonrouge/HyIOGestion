using BLL.DTOs;
using SharedAbstractions.ArchitecturalMarkers;
using System;

namespace Presenter.ForEmployee
{
    public interface ICreateEmployeeView 
    {
        event EventHandler<EmployeeDTO> CreateEmployeeRequested;
        void ShowOperationResult(OperationResult<EmployeeDTO> opRes);
    }
}
