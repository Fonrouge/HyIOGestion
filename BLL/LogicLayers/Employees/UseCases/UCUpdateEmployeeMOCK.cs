using BLL.DTOs;
using Domain.Infrastructure;
using System;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Employees
{
    public class UCUpdateEmployeeMOCK: IUCUpdateEmployee
    {
        private readonly IUnitOfWork _uow;

        public UCUpdateEmployeeMOCK
        (
            IUnitOfWork uow
        )
        {
            _uow = uow ?? throw new ArgumentNullException($"{nameof(uow)} cannot be null");
        }

        public async Task<OperationResult<EmployeeDTO>> Execute(EmployeeDTO dto)
        {
            return new OperationResult<EmployeeDTO>();
        }
    }
}
