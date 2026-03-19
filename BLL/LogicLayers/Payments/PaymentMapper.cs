using BLL.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.LogicLayers
{
    public static class PaymentMapper
    {
        /// <summary>
        /// Entidad -> DTO
        /// </summary>
        public static PaymentDTO ToDto(Payment entity)
        {
            if (entity == null) return null;

            return new PaymentDTO
            {
                Id = entity.Id,
                Amount = entity.Amount != null ? entity.Amount.Value : 0m,
                CreationDate = entity.CreationDate,
                EffectiveDate = entity.EffectiveDate,
                SaleId = entity.SaleId,
                Method = entity.Method != null ? entity.Method.Value : string.Empty,
                Reference = entity.Reference != null ? entity.Reference.Value : string.Empty,
                IsDeleted = entity.IsDeleted,
                DVH = entity.DVH != null ? entity.DVH.Value : string.Empty
            };
        }

        /// <summary>
        /// DTO -> Entidad (Con disquisición según Id)
        /// </summary>
        public static Payment ToEntity(PaymentDTO dto)
        {
            if (dto == null) return null;

            // DISQUISICIÓN: Si el Id es Empty, es un pago nuevo (Alta)
            if (dto.Id == Guid.Empty)
            {
                return Payment.Create(
                    dto.Amount,
                    dto.SaleId,
                    dto.Method ?? string.Empty,
                    dto.Reference ?? string.Empty
                );
            }
            else
            {
                // Si tiene Id, es persistencia (Reconstitución)
                return Payment.Reconstitute(
                    dto.Id,
                    dto.Amount,
                    dto.CreationDate,
                    dto.EffectiveDate,
                    dto.SaleId,
                    dto.Method ?? string.Empty,
                    dto.Reference ?? string.Empty,
                    dto.DVH ?? string.Empty,
                    dto.IsDeleted
                );
            }
        }

        // --- Mapeos de Colecciones ---

        public static IEnumerable<PaymentDTO> ToDtoList(IEnumerable<Payment> entities)
        {
            return entities?.Select(e => ToDto(e)).ToList() ?? new List<PaymentDTO>();
        }

        public static IEnumerable<Payment> ToEntityList(IEnumerable<PaymentDTO> dtos)
        {
            return dtos?.Select(d => ToEntity(d)).ToList() ?? new List<Payment>();
        }
    }
}