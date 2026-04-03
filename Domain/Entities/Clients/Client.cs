using Domain.Contracts;
using Domain.Entities.Clients.ValueObjects;
using System;

namespace Domain.Entities
{
    public class Client : EntityBase, ISoftDeletable, IIntegrityCheckable
    {
        public ClientNameVO Name { get; private set; }
        public ClientLastNameVO LastName { get; private set; }
        public ShipAddressVO ShipAddress { get; private set; }
        public ClientEmailVO Email { get; private set; }
        public ClientPhoneVO Phone { get; private set; }
        public ClientTaxIdVO TaxId { get; private set; }
        public DocNumberVO DocNumber { get; private set; }
        public DvhVo DVH { get; private set; }

        public bool IsDeleted { get; private set; }

        private Client() { }

        public static Client Create
        (
            string rawName,
            string rawLastName,
            string rawShipAddress,
            string rawEmail,
            string rawPhone,
            string rawTaxId,
            string rawDocNumber,
            string dvh = null
        )
        {
            var client = new Client();

            client.Name = ClientNameVO.Create(rawName?.ToUpper() ?? string.Empty);
            client.LastName = ClientLastNameVO.Create(rawLastName?.ToUpper() ?? string.Empty);
            client.ShipAddress = ShipAddressVO.Create(rawShipAddress?.ToUpper() ?? string.Empty);
            client.Email = ClientEmailVO.Create(rawEmail?.ToUpper() ?? string.Empty);
            client.Phone = ClientPhoneVO.Create(rawPhone?.ToUpper() ?? string.Empty);
            client.TaxId = ClientTaxIdVO.Create(rawTaxId?.ToUpper() ?? string.Empty);
            client.DocNumber = DocNumberVO.Create(rawDocNumber?.ToUpper() ?? string.Empty);

            client.IsDeleted = false;
            client.DVH = null;

            return client;
        }

        public static Client Reconstitute
        (
            Guid id,
            string rawName,
            string rawLastName,
            string rawShipAddress,
            string rawEmail,
            string rawPhone,
            string rawTaxId,
            string rawDocNumber,
            bool isDeleted,
            string dvh
        )
        {
            return new Client()
            {
                Id = id,
                Name = ClientNameVO.Create(rawName?.ToUpper() ?? string.Empty),
                LastName = ClientLastNameVO.Create(rawLastName?.ToUpper() ?? string.Empty),
                ShipAddress = ShipAddressVO.Create(rawShipAddress?.ToUpper() ?? string.Empty),
                Email = ClientEmailVO.Create(rawEmail?.ToUpper() ?? string.Empty),
                Phone = ClientPhoneVO.Create(rawPhone?.ToUpper() ?? string.Empty),
                TaxId = ClientTaxIdVO.Create(rawTaxId?.ToUpper() ?? string.Empty), 
                DocNumber = DocNumberVO.Create(rawDocNumber?.ToUpper() ?? string.Empty),
                IsDeleted = isDeleted,
                DVH = !string.IsNullOrEmpty(dvh) ? DvhVo.Create(dvh) : null
            };
        }

        /// <summary>
        /// Genera la cadena de serialización para el cálculo del Dígito Verificador Horizontal.
        /// Protege la integridad de los datos filiatorios y de contacto del cliente.
        /// </summary>
        public string GetDvhSerialization()
        {
            // Mantenemos la consistencia con cultura invariante para el ID (Guid)
            var culture = System.Globalization.CultureInfo.InvariantCulture;

            return string.Join("|",
                Id.ToString(),
                Name.Value,        // Ya viene en UPPER por su VO
                LastName.Value,    // Ya viene en UPPER por su VO
                DocNumber.Value,   // Ya viene en UPPER por su VO (DNI/Pasaporte)
                TaxId.Value,       // Ya viene en UPPER por su VO (CUIT/CUIL)
                Email.Value,       // Ya viene en UPPER por su VO
                Phone.Value,       // Ya viene en UPPER por su VO
                ShipAddress.Value, // Ya viene en UPPER por su VO
                IsDeleted ? "1" : "0"
            );
        }


        public void MarkAsDeleted()
        {
            if (IsDeleted) return;
            IsDeleted = true;
        }

        public override string ToString() => string.Format($"{LastName.Value}, {Name.Value} ({DocNumber.Value})");
       
        public void UpdateDvh(string dvh) => this.DVH = DvhVo.Create(dvh);
    }
}