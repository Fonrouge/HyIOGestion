using BLL.DTOs;
using Domain.Entities.Permisos.Concrete;
using System;
using System.Linq;

namespace BLL.LogicLayers
{
    public static class UsuarioMapper
    {
        public static UsuarioDTO ToDto(Usuario entity)
        {
            if (entity == null) return null;

            var dto = new UsuarioDTO
            {
                Id = entity.Id,
                Username = entity.Username,
                Password = entity.Password,
                LanguageCode = entity.Language,
                DVH = entity.DVH?.Value ?? string.Empty,
                IsDeleted = entity.IsDeleted,
                // Aquí podrías inyectar el EmployeeDTO si lo tienes
            };

            // Aplanamos los permisos para el DTO
            dto.Permisos = entity.Permisos.Select(p => p.PermisoCode).ToList();

            return dto;
        }

        public static Usuario ToEntity(UsuarioDTO dto)
        {
            if (dto == null) return null;

            if (dto.Id == Guid.Empty)
            {
                return Usuario.Create(dto.Username, dto.Password, dto.LanguageCode, dto.EmployeeId);
            }

            return Usuario.Reconstitute
            (
                dto.Id,
                dto.Username,
                dto.Password,
                dto.LanguageCode,
                dto.DVH,
                dto.EmployeeId,
                dto.IsDeleted
            );
        }
    }
}