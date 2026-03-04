using Domain.Entities.Permisos.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Mappers
{
    public static class UsuarioMapper
    {
        
        public static UsuarioDTO ToDto(Usuario entity)
        {
            

            return new UsuarioDTO()
            {
                Id = entity.Id,
                Username = entity.Username,
                Password = entity.Password,
                Language = entity.Language,
                DVH = entity.DVH,
                EmployeeDTO = EmployeeMapper.ToDto(entity.Employee),
                Permisos = entity.Permisos
            };
        }

        public static Usuario ToEntity(UsuarioDTO dto)
        {
            return new Usuario()
            {
                //Id se genera automáticamente
                Username = dto.Username,
                Password = dto.Password,
                Language = dto.Language,
                DVH = dto.DVH,
                Employee = EmployeeMapper.ToEntity(dto.EmployeeDTO),
                Permisos = dto.Permisos
            };
        }






    }
}
