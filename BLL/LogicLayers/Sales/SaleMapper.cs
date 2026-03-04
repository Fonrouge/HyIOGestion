using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
// Asumo que SaleDetailMapper y SaleDetailDTO están en este mismo namespace o agregá el using correspondiente
using BLL.LogicLayers.Sales;

namespace BLL.LogicLayers
{
    public static class SaleMapper
    {
        public static SaleDTO ToDto(Sale sale)
        {
            if (sale == null) return null;

            return new SaleDTO
            {
                Id = sale.Id,
                Date = sale.Date.Value, // Ajustá según si Date en Entity es ValueObject (.Value) o DateTime primitivo
                ClientId = sale.ClientId,
                EmployeeId = sale.EmployeeId,
                TotalAmount = sale.TotalAmount.Value, // Ajustá a .Value si es ValueObject

                // --- CAMBIO CLAVE: Mapear la colección de detalles ---
                Items = SaleDetailMapper.ToListDto(sale.Items)
            };
        }

        public static Sale ToEntity(SaleDTO saleDto)
        {
            if (saleDto == null) return null;

            return Sale.Reconstitute
            (
                id: saleDto.Id,
                date: saleDto.Date,
                clientId: saleDto.ClientId,
                employeeId: saleDto.EmployeeId,
                totalAmountRaw: saleDto.TotalAmount,
                items: SaleDetailMapper.ToListEntity(saleDto.Items),
                active: true,
                createdAt: DateTime.UtcNow,
                isDeleted: false
            );
        }

        public static List<SaleDTO> ToListDto(IEnumerable<Sale> sales)
        {
            return sales?.Select(ToDto).ToList() ?? new List<SaleDTO>();
        }

        public static List<Sale> ToListEntity(IEnumerable<SaleDTO> saleDtos)
        {
            return saleDtos?.Select(ToEntity).ToList() ?? new List<Sale>();
        }
    }
}