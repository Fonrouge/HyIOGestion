using BLL.DTOs;
using Domain.Entities;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.LogicLayers
{
    public static class ClientMapper
    {
        /// <summary>
        /// Entidad -> DTO
        /// </summary>
        public static ClientDTO ToDto(Client entity)
        {
            if (entity == null) return null;

            return new ClientDTO
            {
                Id = entity.Id,
                Name = (string)(entity.Name != null ? entity.Name.Value : string.Empty),
                LastName = (string)(entity.LastName != null ? entity.LastName.Value : string.Empty),
                ShipAddress = (string)(entity.ShipAddress != null ? entity.ShipAddress.Value : string.Empty),
                Email = (string)(entity.Email != null ? entity.Email.Value : string.Empty),
                Phone = (string)(entity.Phone != null ? entity.Phone.Value : string.Empty),
                TaxId = (string)(entity.TaxId != null ? entity.TaxId.Value : string.Empty),
                DocNumber = (string)(entity.DocNumber != null ? entity.DocNumber.Value : string.Empty),
                Observations = (string)(entity.Observations != null ? entity.Observations.Value : string.Empty),
                ShipCountry = (string)(entity.ShipCountry != null ? entity.ShipCountry : string.Empty),
                ShipState = (string)(entity.ShipState!= null ? entity.ShipState : string.Empty),
                ShipZipCode = (string)(entity.ShipZipCode != null ? entity.ShipZipCode.Value : string.Empty),

                IsDeleted = entity.IsDeleted,
                DVH = (string)(entity.DVH?.Value ?? string.Empty)
            };
        }

        /// <summary>
        /// DTO -> Entidad (Con disquisición Create vs Reconstitute)
        /// </summary>
        public static Client ToEntity(ClientDTO dto)
        {
            if (dto == null) return null;

            // DISQUISICIÓN: Si el Id es Empty, es un cliente nuevo.
            if (dto.Id == Guid.Empty)
            {
                return Client.Create
                (
                    dto.Name,
                    dto.LastName,
                    dto.ShipAddress,
                    dto.Email,
                    dto.Phone,
                    dto.TaxId,
                    dto.DocNumber,
                    dto.Observations,
                    dto.ShipCountry,
                    dto.ShipState,
                    dto.ShipZipCode
                );
            }
            else
            {
                // Si tiene Id, viene de la base de datos (Reconstitución)
                return Client.Reconstitute
                (
                    dto.Id,
                    dto.Name,
                    dto.LastName,
                    dto.ShipAddress,
                    dto.Email,
                    dto.Phone,
                    dto.TaxId,
                    dto.DocNumber,
                    dto.Observations,
                    dto.ShipCountry,
                    dto.ShipState,
                    dto.ShipZipCode,
                    dto.IsDeleted,
                    dto.DVH
                );
            }
        }

        // --- Mapeos de Colecciones ---

        public static IEnumerable<ClientDTO> ToDtoList(IEnumerable<Client> entities)
        {
            return entities?.Select(e => ToDto(e)).ToList() ?? new List<ClientDTO>();
        }

        public static IEnumerable<Client> ToEntityList(IEnumerable<ClientDTO> dtos)
        {
            return dtos?.Select(d => ToEntity(d)).ToList() ?? new List<Client>();
        }
    }
}