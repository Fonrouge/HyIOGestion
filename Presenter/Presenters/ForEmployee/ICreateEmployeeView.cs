using BLL.DTOs;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;

namespace Presenter.ForEmployee
{
    public interface ICreateEmployeeView 
    {
        event EventHandler<EmployeeDTO> CreateEmployeeRequested;
        void ShowOperationResult(OperationResult<EmployeeDTO> opRes);
        void FillCountries(IEnumerable<object> countries);
    }
}
