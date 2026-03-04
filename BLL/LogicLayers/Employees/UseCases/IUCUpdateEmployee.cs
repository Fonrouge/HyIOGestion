using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Employees
{
    public interface IUCUpdateEmployee
    {
        Task<OperationResult<EmployeeDTO>> Execute(EmployeeDTO dto);
    }
}
