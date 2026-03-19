using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.LogicLayers.Sales
{
    public static class SaleDetailMapper
    {
        /// <summary>
        /// Convierte una entidad SaleDetail (Rich Domain) → DTO (con valores primitivos).
        /// Extrae .Value de los Value Objects para que el DTO quede plano.
        /// </summary>
        public static SaleDetailDTO ToDto(SaleDetail detail)
        {
            if (detail == null) return null;

            return new SaleDetailDTO
            {
                Id = detail.Id,
                SaleId = detail.SaleId,
                ProductId = detail.ProductId,
                Quantity = detail.Quantity.Value,
                UnitPrice = detail.UnitPrice.Value,
                Subtotal = detail.Subtotal,

                // --- CAMPOS TÉCNICOS SINCRONIZADOS ---
                IsDeleted = detail.IsDeleted,
                DVH = detail.DVH?.Value ?? string.Empty
            };
        }

        /// <summary>
        /// Convierte un DTO → Entidad SaleDetail usando Reconstitute (el único punto permitido).
        /// </summary>
        public static SaleDetail ToEntity(SaleDetailDTO dto)
        {
            if (dto == null) return null;

            if (dto.Id == Guid.Empty)
            {
                // ← Aquí se soluciona el Guid.Empty de una vez por todas
                return SaleDetail.Create(
                    productId: dto.ProductId,
                    rawQuantity: dto.Quantity,
                    rawUnitPrice: dto.UnitPrice
                );
            }

            // Usamos Reconstitute pasando el estado técnico completo
            return SaleDetail.Reconstitute
            (
                id: dto.Id,
                saleId: dto.SaleId,
                productId: dto.ProductId,
                quantityRaw: dto.Quantity,
                unitPriceRaw: dto.UnitPrice,
                subtotal: dto.Subtotal,
                isDeleted: dto.IsDeleted, // Parámetro agregado
                dvh: dto.DVH              // Parámetro agregado
            );
        }

        /// <summary>
        /// Lista completa: entidades → DTOs
        /// </summary>
        public static List<SaleDetailDTO> ToListDto(IEnumerable<SaleDetail> details)
        {
            return details?.Select(ToDto).ToList() ?? new List<SaleDetailDTO>();
        }

        /// <summary>
        /// Lista completa: DTOs → entidades
        /// </summary>
        public static List<SaleDetail> ToListEntity(IEnumerable<SaleDetailDTO> dtos)
        {
            return dtos?.Select(ToEntity).ToList() ?? new List<SaleDetail>();
        }
    }
}