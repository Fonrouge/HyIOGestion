using SharedAbstractions.ArchitecturalMarkers;
using System;

namespace BLL.LogicLayers
{
    public class SupplierDTO : IDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CompanyName { get; set; } = string.Empty;
        public string ContactName { get; set; }
        public string TaxId { get; set; } // CUIT/RUT/DNI
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Observations { get; set; }
    }
}
