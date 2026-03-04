using Domain.Entities.Permisos.Concrete;
using System;

namespace BLL.DTOs.Mappers
{
    public static class UsuarioMapper
    {
        public static UsuarioDTO ToDto(Usuario entity)
        {
            if (entity == null) return null;

            return new UsuarioDTO()
            {
                Id = entity.Id,
                Username = entity.Username,
                Password = entity.Password,
                Language = entity.Language,
                DVH = entity.DVH,

                EmployeeDTO = new EmployeeDTO { Id = entity.EmployeeId },

                Permisos = entity.Permisos
            };
        }

        public static Usuario ToEntity(UsuarioDTO dto)
        {
            if (dto == null) return null;

            return new Usuario()
            {
                Id = dto.Id,
                Username = dto.Username,
                Password = dto.Password,
                Language = dto.Language,
                DVH = dto.DVH,
                EmployeeId = dto.EmployeeDTO != null ? dto.EmployeeDTO.Id : Guid.Empty,

                Permisos = dto.Permisos
            };
        }
    }
}