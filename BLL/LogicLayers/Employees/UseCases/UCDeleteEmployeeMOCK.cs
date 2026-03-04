using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Employees
{
    public class UCDeleteEmployeeMOCK : IUCDeleteEmployee
    {
        public async Task<OperationResult<EmployeeDTO>> Execute(EmployeeDTO dto)
        {
            return new OperationResult<EmployeeDTO>();
        }
    }
}
