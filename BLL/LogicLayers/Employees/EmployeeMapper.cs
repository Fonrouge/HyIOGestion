using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DTOs.Mappers
{
    public static class EmployeeMapper
    {
        /// <summary>
        /// Convierte una Entidad de Dominio rica en un DTO plano (para la vista o API).
        /// Desempaqueta los Value Objects obteniendo su .Value
        /// </summary>
        public static EmployeeDTO ToDto(Employee entity)
        {
            if (entity == null) return null;

            return new EmployeeDTO()
            {
                Id = entity.Id,
                // Se extraemos el valor primitivo de cada Value Object
                FileNumber = entity.FileNumber?.Value,
                FirstName = entity.FirstName?.Value,
                LastName = entity.LastName?.Value,
                NationalId = entity.NationalId?.Value,
                Email = entity.Email?.Value,
                PhoneNumber = entity.PhoneNumber?.Value,
                HomeAddress = entity.HomeAddress?.Value,

                // Tipos primitivos directos
                Active = entity.Active,
                DVH = entity.DVH
            };
        }

        /// <summary>
        /// Convierte un DTO plano en una Entidad de Dominio.
        /// Los strings crudos pasarán por el validador de los Value Objects automáticamente.
        /// </summary>
        public static Employee ToEntity(EmployeeDTO dto)
        {
            if (dto == null) return null;

            // Dependiendo de si el DTO trae un ID válido o no, decidimos si estamos
            // creando un Empleado NUEVO o reconstituyendo uno EXISTENTE.
            if (dto.Id == Guid.Empty)
            {
                // Es un empleado nuevo (no tiene ID asignado desde el frontend)
                return Employee.Create
                (
                    rawFileNumber: dto.FileNumber,
                    rawFirstName: dto.FirstName,
                    rawLastName: dto.LastName,
                    rawNationalId: dto.NationalId,
                    rawEmail: dto.Email,
                    rawPhoneNumber: dto.PhoneNumber,
                    rawHomeAddress: dto.HomeAddress
                );
            }
            else
            {
                // Es un empleado existente (viene con ID para una actualización, por ejemplo)
                return Employee.Reconstitute
                (
                    id: dto.Id,
                    rawFileNumber: dto.FileNumber,
                    rawFirstName: dto.FirstName,
                    rawLastName: dto.LastName,
                    rawNationalId: dto.NationalId,
                    rawEmail: dto.Email,
                    rawPhoneNumber: dto.PhoneNumber,
                    rawHomeAddress: dto.HomeAddress,
                    dvh: dto.DVH,
                    active: dto.Active,
                    isDeleted: false // Asumimos que no viaja en el DTO, o lo agregas si lo necesitas
                );
            }
        }

        /// <summary>
        /// Convierte una lista de Entidades a una lista de DTOs.
        /// </summary>
        public static IEnumerable<EmployeeDTO> ToListDTO(IEnumerable<Employee> entities)
        {
            if (entities == null) return Enumerable.Empty<EmployeeDTO>();

            return entities.Select(ToDto).ToList();
        }

        /// <summary>
        /// Convierte una lista de DTOs a una lista de Entidades.
        /// </summary>
        public static IEnumerable<Employee> ToListEntity(IEnumerable<EmployeeDTO> dtos)
        {
            if (dtos == null) return Enumerable.Empty<Employee>();

            return dtos.Select(ToEntity).ToList();
        }
    }
}