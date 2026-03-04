using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Mappers
{
    public static class EmployeeMapper
    {
        public static EmployeeDTO ToDto(Employee entity)
        {
            return new EmployeeDTO()
            {
                Id = entity.Id,
                FileNumber = entity.FileNumber,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                NationalId = entity.NationalId,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                HomeAddress = entity.HomeAddress,
                Active = entity.Active,
                DVH = entity.DVH
            };
        }

        public static Employee ToEntity(EmployeeDTO dto)
        {
            return new Employee()
            {
                //Id se genera automáticamente
                FileNumber = dto.FileNumber,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                NationalId = dto.NationalId,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                HomeAddress = dto.HomeAddress,
                Active = dto.Active,
                DVH = dto.DVH
            };
        }


    }
}
