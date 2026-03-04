using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Employees
{
    public interface IUCCreateEmployee
    {
        Task<OperationResult<EmployeeDTO>> ExecuteAsync(EmployeeDTO dto);
    } 
}
