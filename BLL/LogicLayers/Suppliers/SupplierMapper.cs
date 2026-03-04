using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

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
                // Extraemos el valor primitivo de los Value Objects
                CompanyName = supplier.CompanyName?.Value,
                ContactName = supplier.ContactName?.Value,
                TaxId = supplier.TaxId?.Value,
                Phone = supplier.Phone?.Value,
                Mail = supplier.Mail?.Value,
                Observations = supplier.Observations // Observations quedó como string en el Entity
            };
        }

        public static Supplier ToEntity(SupplierDTO supplierDto)
        {
            if (supplierDto == null) return null;

            // Usamos Reconstitute porque el DTO ya trae un Id asignado.
            // Como el DTO no maneja estados internos (Active, IsDeleted, DVH), 
            // pasamos valores por defecto seguros para evitar que el dominio quede inconsistente.
            return Supplier.Reconstitute
            (
                id: supplierDto.Id,
                rawCompanyName: supplierDto.CompanyName,
                rawContactName: supplierDto.ContactName,
                rawTaxId: supplierDto.TaxId,
                rawPhone: supplierDto.Phone,
                rawMail: supplierDto.Mail,
                observations: supplierDto.Observations,
                dvh: string.Empty,
                active: true,
                isDeleted: false
            );
        }

        public static IEnumerable<SupplierDTO> ToListDto(IEnumerable<Supplier> suppliers)
        {
            if (suppliers == null) return Enumerable.Empty<SupplierDTO>();

            return suppliers.Select(ToDto).ToList();
        }

        public static IEnumerable<Supplier> ToListEntity(IEnumerable<SupplierDTO> supplierDtos)
        {
            if (supplierDtos == null) return Enumerable.Empty<Supplier>();

            return supplierDtos.Select(ToEntity).ToList();
        }
    }
}