using Domain.BaseContracts;
using Domain.Entities.Clients.ValueObjects; // Asegurate de crear este namespace
using System;

namespace Domain.Entities
{
    public class Client : EntityBase, ISoftDeletable
    {
        // --- PROPIEDADES DE DOMINIO (Rich Domain Model) ---
        public ClientNameVO Name { get; private set; }
        public ClientLastNameVO LastName { get; private set; }
        public ShipAddressVO ShipAddress { get; private set; }
        public WarehouseAddressVO WarehouseAddress { get; private set; }
        public ClientEmailVO Email { get; private set; }
        public ClientPhoneVO Phone { get; private set; }
        public ClientTaxIdVO TaxId { get; private set; }
        public DocNumberVO DocNumber { get; private set; }

        // --- CAMPOS TÉCNICOS Y DE ESTADO ---
        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }

        // Constructor privado para forzar el uso de Factories
        private Client() { }

        /// <summary>
        /// ÚNICO punto de creación para un NUEVO Client.
        /// El Domain se defiende desde la primera línea (Fail Fast).
        /// </summary>
        public static Client Create
        (
            string rawName,
            string rawLastName,
            string rawShipAddress,
            string rawWarehouseAddress,
            string rawEmail,
            string rawPhone,
            string rawTaxId,
            string rawDocNumber
        )
        {
            return new Client
            {
                // EntityBase asigna automáticamente el ID
                Name = ClientNameVO.Create(rawName?.ToUpper() ?? "N/I"),
                LastName = ClientLastNameVO.Create(rawLastName?.ToUpper() ?? "N/I"),
                ShipAddress = ShipAddressVO.Create(rawShipAddress?.ToUpper() ?? "N/I"),
                WarehouseAddress = WarehouseAddressVO.Create(rawWarehouseAddress?.ToUpper() ?? "N/I"),
                Email = ClientEmailVO.Create(rawEmail?.ToUpper() ?? "N/I"),
                Phone = ClientPhoneVO.Create(rawPhone?.ToUpper() ?? "N/I"),
                TaxId = ClientTaxIdVO.Create(rawTaxId?.ToUpper() ?? "N/I"),
                DocNumber = DocNumberVO.Create(rawDocNumber?.ToUpper() ?? "N/I"),

                IsActive = true,
                IsDeleted = false
            };
        }

        /// <summary>
        /// Reconstruye un Client EXISTENTE desde la persistencia (DB).
        /// </summary>
        public static Client Reconstitute
        (
            Guid id,
            string rawName,
            string rawLastName,
            string rawShipAddress,
            string rawWarehouseAddress,
            string rawEmail,
            string rawPhone,
            string rawTaxId,
            string rawDocNumber,
            bool isActive,
            bool isDeleted
        )
        {
            return new Client
            {
                Id = Guid.Parse(id.ToString().ToUpper()),
                Name = ClientNameVO.Create(rawName?.ToUpper() ?? "N/I"),
                LastName = ClientLastNameVO.Create(rawLastName?.ToUpper() ?? "N/I"),
                ShipAddress = ShipAddressVO.Create(rawShipAddress?.ToUpper() ?? "--"),
                WarehouseAddress = WarehouseAddressVO.Create(rawWarehouseAddress?.ToUpper() ?? "--"),
                Email = ClientEmailVO.Create(rawEmail?.ToUpper() ?? "N/I"),
                Phone = ClientPhoneVO.Create(rawPhone?.ToUpper() ?? "N/I"),
                TaxId = ClientTaxIdVO.Create(rawTaxId?.ToUpper() ?? "N/I"),
                DocNumber = DocNumberVO.Create(rawDocNumber?.ToUpper() ?? "N/I"),

                IsActive = isActive,
                IsDeleted = isDeleted
            };
        }

        // --- COMPORTAMIENTO (Transiciones de Estado Seguras) ---

        public void MarkAsDeleted()
        {
            if (IsDeleted) return; // Idempotencia
            IsDeleted = true;
            IsActive = false; // Regla de negocio: un cliente borrado lógicamente no puede estar activo
        }

        public void Activate()
        {
            if (IsDeleted)
                throw new InvalidOperationException("No se puede activar un cliente que ha sido eliminado.");

            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public override string ToString()
            => $"{LastName.Value}, {Name.Value} ({DocNumber.Value})";
    }
}