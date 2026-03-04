using BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Employees
{
    public interface IUCGetAllEmployees
    {
        Task<(IEnumerable<EmployeeDTO>, OperationResult<EmployeeDTO>)> ExecuteAsync();
    }
}
