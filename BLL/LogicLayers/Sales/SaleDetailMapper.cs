using Domain.Entities;
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
                Quantity = detail.Quantity.Value,    // QuantityVO → decimal
                UnitPrice = detail.UnitPrice.Value,  // UnitPriceVO → decimal
                Subtotal = detail.Subtotal           // Ya es primitivo (decimal)
            };
        }

        /// <summary>
        /// Convierte un DTO → Entidad SaleDetail usando Reconstitute (el único punto permitido).
        /// </summary>
        public static SaleDetail ToEntity(SaleDetailDTO dto)
        {
            if (dto == null) return null;

            return SaleDetail.Reconstitute
            (
                id: dto.Id,
                saleId: dto.SaleId,
                productId: dto.ProductId,
                quantityRaw: dto.Quantity,
                unitPriceRaw: dto.UnitPrice,
                subtotal: dto.Subtotal
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