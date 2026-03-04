using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BLL.LogicLayers
{
    public static class PaymentMapper
    {
        public static PaymentDTO ToDto(Payment entity)
        {
            if (entity == null) return null;

            return new PaymentDTO
            {
                Id = entity.Id,
                // Extraemos el Value primitivo y usamos InvariantCulture para evitar problemas con comas y puntos
                Amount = entity.Amount?.Value.ToString("F2", CultureInfo.InvariantCulture),
                // Formato ISO 8601 (Round-trip) para que las fechas viajen como string sin perder precisión
                CreationDate = entity.CreationDate.ToString("O"),
                EffectiveDate = entity.EffectiveDate.ToString("O"),
                ClientId = entity.ClientId.ToString(),
                Method = entity.Method?.Value,
                Reference = entity.Reference?.Value
            };
        }

        public static Payment ToEntity(PaymentDTO dto)
        {
            if (dto == null) return null;

            // Parseos seguros (TryParse) para evitar excepciones en tiempo de ejecución si el DTO viene con basura.
            // Si el parseo falla, asignamos valores por defecto (que luego fallarán en las validaciones del dominio si no son válidos).
            decimal amount = decimal.TryParse(dto.Amount, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedAmount) ? parsedAmount : 0;
            DateTime creationDate = DateTime.TryParse(dto.CreationDate, null, DateTimeStyles.RoundtripKind, out var parsedCreation) ? parsedCreation : DateTime.UtcNow;
            DateTime effectiveDate = DateTime.TryParse(dto.EffectiveDate, null, DateTimeStyles.RoundtripKind, out var parsedEffective) ? parsedEffective : DateTime.UtcNow;
            Guid clientId = Guid.TryParse(dto.ClientId, out var parsedClient) ? parsedClient : Guid.Empty;

            // Usamos Reconstitute respetando el encapsulamiento de tu Rich Domain Model
            return Payment.Reconstitute(
                id: dto.Id,
                rawAmount: amount,
                creationDate: creationDate,
                effectiveDate: effectiveDate,
                clientId: clientId,
                rawMethod: dto.Method,
                rawReference: dto.Reference,
                dvh: string.Empty // El DVH lo maneja la capa de persistencia (DAL) al leer/escribir en BD
            );
        }

        public static IEnumerable<PaymentDTO> ToListDto(IEnumerable<Payment> entities)
        {
            if (entities == null) return Enumerable.Empty<PaymentDTO>();
            return entities.Select(ToDto).ToList();
        }

        public static IEnumerable<Payment> ToListEntity(IEnumerable<PaymentDTO> dtos)
        {
            if (dtos == null) return Enumerable.Empty<Payment>();
            return dtos.Select(ToEntity).ToList();
        }
    }
}