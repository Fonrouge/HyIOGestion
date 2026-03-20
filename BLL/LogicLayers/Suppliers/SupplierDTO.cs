using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.ComponentModel;

namespace BLL.LogicLayers
{
    public class SupplierDTO : IDto
    {
        [Browsable(false)]
        public Guid Id { get; set; } = Guid.Empty;

        public string CompanyName { get; set; } = string.Empty;
        public string ContactName { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Observations { get; set; }

        // --- CAMPOS DE ESTADO (Sincronizados con la Entidad) ---
        public bool Active { get; set; } = true;


        // --- CAMPOS TÉCNICOS (Integridad y Auditoría) ---
        [Browsable(false)]
        public bool IsDeleted { get; set; } = false;

        [Browsable(false)]
        public string DVH { get; set; }

        public override string ToString() => CompanyName;
    }
}