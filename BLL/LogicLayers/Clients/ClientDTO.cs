using BLL.Infrastructure;
using Domain.Entities.Employees.ValueObjects;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.ComponentModel;

namespace BLL.DTOs
{
    public class ClientDTO : IDto
    {
        [Browsable(false)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "N/I"; //Not Initialized / Not Implemented

        public string LastName{ get; set; } = "N/I";
        public string ShipAddress { get; set; } = "N/I";
        public string WareHouseAddress { get; set; } = "N/I";
        public string Email { get; set; } = "N/I";

        public string Phone { get; set; } = "N/I";

        public string TaxId { get; set; } = "N/I";// Documento de identidad (DNI, CUIT, etc.)
        public string DocNumber { get; set; } = "N/I";// Documento de identidad (DNI, CUIT, etc.)

        public bool IsActive { get; set; } = true;


        public override string ToString() => $"{LastName}, {Name} Mail: ({Email})";

    }
}
