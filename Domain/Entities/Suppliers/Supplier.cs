using Domain.BaseContracts;
using Domain.Entities.Clients.ValueObjects;
using Domain.Entities.Suppliers.ValueObjects; // Asegúrate de crear este namespace
using System;

namespace Domain.Entities
{
    public class Supplier : EntityBase, ISoftDeletable
    {
        // --- PROPIEDADES DE DOMINIO (Encapsuladas con Value Objects) ---
        public CompanyNameVO CompanyName { get; private set; }
        public ContactNameVO ContactName { get; private set; }
        public SupplierTaxIdVO TaxId { get; private set; } // CUIT/RUT/DNI
        public SupplierPhoneVO Phone { get; private set; }
        public SupplierEmailVO Mail { get; private set; }
        public string Observations { get; private set; } // Puede quedar como string si no requiere validación compleja

        // --- CAMPOS TÉCNICOS Y DE ESTADO ---
        public string DVH { get; private set; }
        public bool Active { get; private set; }
        public bool IsDeleted { get; private set; }

        // Constructor privado para forzar el uso de Factory Methods
        private Supplier() { }

        /// <summary>
        /// ÚNICO punto de creación para un NUEVO Supplier.
        /// </summary>
        public static Supplier Create(
            string rawCompanyName,
            string rawContactName,
            string rawTaxId,
            string rawPhone,
            string rawMail,
            string observations = "")
        {
            return new Supplier
            {
                // El Id se genera en EntityBase
                CompanyName = CompanyNameVO.Create(rawCompanyName.Trim().ToUpper()),
                ContactName = ContactNameVO.Create(rawContactName.Trim().ToUpper()),
                TaxId = SupplierTaxIdVO.Create(rawTaxId.Trim().ToUpper()),
                Phone = SupplierPhoneVO.Create(rawPhone.Trim().ToUpper()),
                Mail = SupplierEmailVO.Create(rawMail.Trim().ToLower()),
                Observations = observations?.Trim() ?? string.Empty,
                DVH = string.Empty,
                Active = true,
                IsDeleted = false
            };
        }

        /// <summary>
        /// Reconstruye un Supplier EXISTENTE desde la base de datos.
        /// </summary>
        public static Supplier Reconstitute(
            Guid id,
            string rawCompanyName,
            string rawContactName,
            string rawTaxId,
            string rawPhone,
            string rawMail,
            string observations,
            string dvh,
            bool active,
            bool isDeleted)
        {
            return new Supplier
            {
                Id = id,
                CompanyName = CompanyNameVO.Create(rawCompanyName),
                ContactName = ContactNameVO.Create(rawContactName),
                TaxId = SupplierTaxIdVO.Create(rawTaxId),
                Phone = SupplierPhoneVO.Create(rawPhone),
                Mail = SupplierEmailVO.Create(rawMail),
                Observations = observations,
                DVH = dvh ?? string.Empty,
                Active = active,
                IsDeleted = isDeleted
            };
        }

        // --- COMPORTAMIENTO (Transiciones de Estado) ---

        public void UpdateObservations(string newObservations)
        {
            Observations = newObservations?.Trim() ?? string.Empty;
        }

        public void MarkAsDeleted()
        {
            if (IsDeleted) return;
            IsDeleted = true;
            Active = false;
        }

        public void Activate()
        {
            if (IsDeleted)
                throw new InvalidOperationException("No se puede activar un proveedor eliminado.");
            Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }

        public void UpdateDVH(string newDvh)
        {
            if (string.IsNullOrWhiteSpace(newDvh))
                throw new ArgumentException("El DVH no puede estar vacío.", nameof(newDvh));
            DVH = newDvh;
        }

        public override string ToString()
            => $"{CompanyName.Value} (TaxId: {TaxId.Value})";
    }
}