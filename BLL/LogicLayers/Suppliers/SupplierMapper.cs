using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using BLL.DTOs;

namespace BLL.LogicLayers
{
    public static class SupplierMapper
    {
        public static SupplierDTO ToDto(Supplier supplier)
        {
            if (supplier == null) return null;

            return new SupplierDTO
            {
                Id = supplier.Id,
                CompanyName = supplier.CompanyName?.Value,
                ContactName = supplier.ContactName?.Value,
                TaxId = supplier.TaxId?.Value,
                TaxNumber = supplier.TaxNumber?.Value, // <--- AGREGADO
                Phone = supplier.Phone?.Value,
                Mail = supplier.Mail?.Value,
                Address = supplier.Address?.Value,
                City = supplier.City?.Value,
                Observations = supplier.Observations?.Value,

                // --- MAPEO DE ESTADO Y CONTROL ---
                Active = supplier.Active,
                IsDeleted = supplier.IsDeleted,
                DVH = supplier.DVH?.Value ?? string.Empty
            };
        }

        public static Supplier ToEntity(SupplierDTO dto)
        {
            if (dto == null) return null;

            // DISQUISICIÓN: ¿Es un nuevo proveedor o estamos hidratando uno de la DB?
            if (dto.Id == Guid.Empty)
            {
                // ALTA: Usamos Create (Agregado dto.TaxNumber)
                return Supplier.Create(
                    dto.CompanyName,
                    dto.ContactName,
                    dto.TaxId,
                    dto.TaxNumber, // <--- AGREGADO
                    dto.Phone,
                    dto.Mail,
                    dto.Address,
                    dto.City,
                    dto.Observations
                );
            }

            // RECONSTITUCIÓN: (Este ya lo tenías bien)
            return Supplier.Reconstitute(
                id: dto.Id,
                rawCompanyName: dto.CompanyName,
                rawContactName: dto.ContactName,
                rawTaxId: dto.TaxId,
                rawTaxNumber: dto.TaxNumber,
                rawPhone: dto.Phone,
                rawMail: dto.Mail,
                rawAddress: dto.Address,
                rawCity: dto.City,
                observations: dto.Observations,
                dvh: dto.DVH,
                active: dto.Active,
                isDeleted: dto.IsDeleted
            );
        }

        // --- Mapeos de listas ---
        public static List<SupplierDTO> ToListDto(IEnumerable<Supplier> entities) =>
            entities?.Select(ToDto).ToList() ?? new List<SupplierDTO>();

        public static List<Supplier> ToListEntity(IEnumerable<SupplierDTO> dtos) =>
            dtos?.Select(ToEntity).ToList() ?? new List<Supplier>();
    }
}