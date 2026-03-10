using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.ComponentModel;

namespace BLL.LogicLayers
{
    public class SupplierDTO : IDto
    {
        [Browsable(false)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CompanyName { get; set; } = string.Empty;
        public string ContactName { get; set; }
        public string TaxId { get; set; } // CUIT/cuil/
        public string Mail { get; set; }
        public string Phone { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string Observations { get; set; }

    }
}
