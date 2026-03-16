using Domain.BaseContracts;
using Domain.Entities.Suppliers.ValueObjects;
using System;

namespace Domain.Entities
{
    public class Supplier : EntityBase, ISoftDeletable
    {
        // --- PROPIEDADES DE DOMINIO (Encapsuladas con Value Objects) ---
        public CompanyNameVO CompanyName { get; private set; }
        public ContactNameVO ContactName { get; private set; }
        public SupplierEmailVO Mail { get; private set; }
        public SupplierPhoneVO Phone { get; private set; }
        public SupplierTaxIdVO TaxId { get; private set; }
        public SupplierAddressVO Address { get; private set; }
        public SupplierCityVO City { get; private set; }
        public SupplierObservationsVO Observations { get; private set; }


        // --- CAMPOS TÉCNICOS Y DE ESTADO ---
        public DvhVo DVH { get; private set; }
        public bool Active { get; private set; }
        public bool IsDeleted { get; private set; }



        // Constructor privado para forzar el uso de Factory Methods
        private Supplier() { }


        /// <summary>
        /// ÚNICO punto de creación para un NUEVO Supplier.
        /// </summary>
        public static Supplier Create
        (
            string rawCompanyName,
            string rawContactName,
            string rawTaxId,
            string rawPhone,
            string rawMail,
            string rawAddress,
            string rawCity,
            string observations = ""
        )
        {
            return new Supplier
            {
                CompanyName = CompanyNameVO.Create(rawCompanyName),
                ContactName = ContactNameVO.Create(rawContactName),
                TaxId = SupplierTaxIdVO.Create(rawTaxId),
                Phone = SupplierPhoneVO.Create(rawPhone),
                Mail = SupplierEmailVO.Create(rawMail),
                Address = SupplierAddressVO.Create(rawAddress),
                City = SupplierCityVO.Create(rawCity),
                Observations = SupplierObservationsVO.Create(observations),
                DVH = null,
                Active = true,
                IsDeleted = false
            };
        }

        /// <summary>
        /// Reconstruye un Supplier EXISTENTE desde la base de datos.
        /// </summary>
        public static Supplier Reconstitute
        (
            Guid id,
            string rawCompanyName,
            string rawContactName,
            string rawTaxId,
            string rawPhone,
            string rawMail,
            string rawAddress,
            string rawCity,
            string observations,
            string dvh,
            bool active,
            bool isDeleted
        )
        {
            return new Supplier
            {
                Id = id,
                CompanyName = CompanyNameVO.Create(rawCompanyName),
                ContactName = ContactNameVO.Create(rawContactName),
                TaxId = SupplierTaxIdVO.Create(rawTaxId),
                Phone = SupplierPhoneVO.Create(rawPhone),
                Mail = SupplierEmailVO.Create(rawMail),
                Address = SupplierAddressVO.Create(rawAddress),
                City = SupplierCityVO.Create(rawCity),
                Observations = SupplierObservationsVO.Create(observations),
                DVH = !string.IsNullOrEmpty(dvh) ? DvhVo.Create(dvh) : null,
                Active = active,
                IsDeleted = isDeleted
            };
        }


        // --- COMPORTAMIENTO (Transiciones de Estado) ---

        public void MarkAsDeleted()
        {
            if (IsDeleted) return;
            IsDeleted = true;
            Active = false;
        }

        public void UpdateDVH(string dvh) => DVH = DvhVo.Create(dvh);

        public void Activate()
        {
            if (IsDeleted)
                throw new InvalidOperationException("No se puede activar un proveedor eliminado.");
            Active = true;
        }

        public void Deactivate() => Active = false;


        public override string ToString() => $"{CompanyName.Value} (TaxId: {TaxId.Value})";
    }
}