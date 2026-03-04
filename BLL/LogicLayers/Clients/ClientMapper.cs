using BLL.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.LogicLayers
{
    public static class ClientMapper
    {
        /// <summary>
        /// Mapea un DTO a una Entidad nueva (usando lógica de creación).
        /// </summary>
        public static Client ToEntity(ClientDTO dto)
        {
            // Usamos el método Create que definimos en la Entidad.
            // Esto dispara todas las validaciones de los Value Objects.
            return Client.Create(
                dto.Name,
                dto.LastName,
                dto.ShipAddress,
                dto.WareHouseAddress,
                dto.Email,
                dto.Phone,
                dto.TaxId,
                dto.DocNumber
            );
        }

        /// <summary>
        /// Mapea una Entidad a un DTO para transporte de datos.
        /// </summary>
        public static ClientDTO ToDto(Client entity)
        {
            return new ClientDTO
            {
                Id = entity.Id,
                Name = entity.Name.Value, // Accedemos a la propiedad .Value del VO
                LastName = entity.LastName.Value,
                ShipAddress = entity.ShipAddress.Value,
                WareHouseAddress = entity.WarehouseAddress.Value,
                Email = entity.Email.Value,
                Phone = entity.Phone.Value,
                TaxId = entity.TaxId.Value,
                DocNumber = entity.DocNumber.Value,
                IsActive = entity.IsActive
            };
        }

        // --- Mapeos de Colecciones ---

        public static IEnumerable<Client> ToEntityList(IEnumerable<ClientDTO> dtos)
        {
            if (dtos == null) return Enumerable.Empty<Client>();
            return dtos.Select(ToEntity);
        }

        public static IEnumerable<ClientDTO> ToDtoList(IEnumerable<Client> entities)
        {
            if (entities == null) return Enumerable.Empty<ClientDTO>();
            return entities.Select(ToDto);
        }

        /// <summary>
        /// Caso especial: Si necesitas reconstruir entidades desde persistencia (DB/Json)
        /// sin disparar las reglas de validación de "Nuevo Cliente".
        /// </summary>
        public static Client ToEntityFromStorage(ClientDTO dto, bool isDeleted)
        {
            return Client.Reconstitute(
                dto.Id,
                dto.Name,
                dto.LastName,
                dto.ShipAddress,
                dto.WareHouseAddress,
                dto.Email,
                dto.Phone,
                dto.TaxId,
                dto.DocNumber,
                dto.IsActive,
                isDeleted
            );
        }
    }
}