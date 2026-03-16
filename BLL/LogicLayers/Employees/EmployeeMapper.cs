using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DTOs.Mappers
{
    public static class EmployeeMapper
    {
        /// <summary>
        /// Entidad -> DTO
        /// </summary>
        public static EmployeeDTO ToDto(Employee entity)
        {
            if (entity == null) return null;

            return new EmployeeDTO()
            {
                Id = entity.Id,
                FileNumber = entity.FileNumber != null ? entity.FileNumber.Value : string.Empty,
                FirstName = entity.FirstName != null ? entity.FirstName.Value : string.Empty,
                LastName = entity.LastName != null ? entity.LastName.Value : string.Empty,
                NationalId = entity.NationalId != null ? entity.NationalId.Value : string.Empty,
                Email = entity.Email != null ? entity.Email.Value : string.Empty,
                PhoneNumber = entity.PhoneNumber != null ? entity.PhoneNumber.Value : string.Empty,
                HomeAddress = entity.HomeAddress != null ? entity.HomeAddress.Value : string.Empty,

                IsDeleted = entity.IsDeleted,
                Active = entity.Active,
                DVH = entity.DVH != null ? entity.DVH.Value : string.Empty
            };
        }

        /// <summary>
        /// DTO -> Entidad (Con disquisición Create vs Reconstitute)
        /// </summary>
        public static Employee ToEntity(EmployeeDTO dto)
        {
            if (dto == null) return null;

            // DISQUISICIÓN: ¿Es nuevo o viene de la DB?
            if (dto.Id == Guid.Empty)
            {
                // Lógica de CREACIÓN (Nueva Entidad)
                return Employee.Create(
                    dto.FileNumber,
                    dto.FirstName,
                    dto.LastName,
                    dto.NationalId,
                    dto.Email,
                    dto.PhoneNumber,
                    dto.HomeAddress
                );
            }
            else
            {
                // Lógica de RECONSTITUCIÓN (Entidad Existente)
                return Employee.Reconstitute(
                    dto.Id,
                    dto.FileNumber,
                    dto.FirstName,
                    dto.LastName,
                    dto.NationalId,
                    dto.Email,
                    dto.PhoneNumber,
                    dto.HomeAddress,
                    dto.DVH,
                    dto.Active,
                    dto.IsDeleted
                );
            }
        }

        public static IEnumerable<EmployeeDTO> ToListDTO(IEnumerable<Employee> entities)
        {
            return entities?.Select(e => ToDto(e)).ToList() ?? new List<EmployeeDTO>();
        }

        public static IEnumerable<Employee> ToListEntity(IEnumerable<EmployeeDTO> dtos)
        {
            return dtos?.Select(d => ToEntity(d)).ToList() ?? new List<Employee>();
        }
    }
}