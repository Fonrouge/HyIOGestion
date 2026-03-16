using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
                Date = sale.Date.Value,
                ClientId = sale.ClientId,
                EmployeeId = sale.EmployeeId,
                TotalAmount = sale.TotalAmount.Value,
                IsActive = sale.Active,
                CreatedAt = sale.CreatedAt,
                IsDeleted = sale.IsDeleted,
                DVH = sale.DVH?.Value ?? string.Empty,

                Items = SaleDetailMapper.ToListDto(sale.Items)
            };
        }

        public static Sale ToEntity(SaleDTO dto)
        {
            if (dto == null) return null;

            if (dto.Id == Guid.Empty)
            {
                return Sale.Create
                (
                    dto.ClientId,
                    dto.EmployeeId,
                    SaleDetailMapper.ToListEntity(dto.Items)
                );
            }

            return Sale.Reconstitute
            (
                id: dto.Id,
                date: dto.Date,
                clientId: dto.ClientId,
                employeeId: dto.EmployeeId,
                totalAmountRaw: dto.TotalAmount,
                items: SaleDetailMapper.ToListEntity(dto.Items),
                active: dto.IsActive,
                createdAt: dto.CreatedAt,
                isDeleted: dto.IsDeleted,
                dvh: dto.DVH
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