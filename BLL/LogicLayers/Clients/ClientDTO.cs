using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.ComponentModel;

namespace BLL.DTOs
{
    public class ClientDTO : IDto
    {
        [Browsable(false)]
        public Guid Id { get; set; } = Guid.Empty;

        public string Name { get; set; }
        public string LastName { get; set; }

        public string TaxId { get; set; }
        public string DocNumber { get; set; }

        public string ShipAddress { get; set; }

        public string ShipCountry { get; set; }
        public string ShipState { get; set; }
        public string ShipZipCode { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Observations { get; set; }

        // --- CAMPOS DE ESTADO Y CONTROL ---
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [Browsable(false)]
        public string DVH { get; set; }


        public ClientDTO()
        {
            Id = Guid.NewGuid();
            Name = "N/I";
            LastName = "N/I";
            TaxId = "N/I";
            DocNumber = "N/I";
            ShipAddress = "N/I";
            ShipCountry = "N/I";
            ShipState = "N/I";
            ShipZipCode = "N/I";
            Email = "N/I";
            Phone = "N/I";
            Observations = "N/I";
            IsActive = true;
            IsDeleted = false;
            DVH = "";
        }

        public override string ToString()
        {
            return string.Format($"{LastName}, {Name} Mail: ({Email})");
        }
    }
}