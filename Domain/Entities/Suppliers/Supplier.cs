using Domain.Contracts;
using Domain.Entities.Suppliers.ValueObjects;
using System;

namespace Domain.Entities
{
    public class Supplier : EntityBase, ISoftDeletable, IIntegrityCheckable
    {
        // --- PROPIEDADES DE DOMINIO (Encapsuladas con Value Objects) ---
        public CompanyNameVO CompanyName { get; private set; }
        public ContactNameVO ContactName { get; private set; }
        public SupplierEmailVO Mail { get; private set; }
        public SupplierPhoneVO Phone { get; private set; }
        public SupplierTaxIdVO TaxId { get; private set; }
        public SupplierTaxNumber TaxNumber { get; private set; }
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
            string rawTaxNumber,
            string rawPhone,
            string rawMail,
            string rawAddress,
            string rawCity,
            string observations = null
        )
        {            
            return new Supplier
            {
                CompanyName = CompanyNameVO.Create(rawCompanyName),
                ContactName = ContactNameVO.Create(rawContactName),
                TaxId = SupplierTaxIdVO.Create(rawTaxId),
                TaxNumber = SupplierTaxNumber.Create(rawTaxNumber),
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
            string rawTaxNumber,
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
                TaxNumber = SupplierTaxNumber.Create(rawTaxNumber),
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

        /// <summary>
        /// Genera la cadena de serialización para el cálculo del Dígito Verificador Horizontal.
        /// Centraliza el orden y formato de los datos para garantizar consistencia.
        /// </summary>
        public string GetDvhSerialization()
        {
            // Usamos cultura invariante para evitar problemas con símbolos locales
            var culture = System.Globalization.CultureInfo.InvariantCulture;

            return string.Join("|",
                Id.ToString(),
                CompanyName.Value,
                ContactName.Value,
                Mail.Value.ToUpper(culture),
                Phone.Value,
                TaxId.Value,
                TaxNumber.Value,
                Address.Value,
                City.Value,
                (Observations?.Value ?? string.Empty), // Manejo de nulos por seguridad
                Active ? "1" : "0",
                IsDeleted ? "1" : "0"
            );
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