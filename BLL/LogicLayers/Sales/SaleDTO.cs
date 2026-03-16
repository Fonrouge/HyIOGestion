using BLL.DTOs;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BLL.LogicLayers
{
    public class SaleDTO : IDto
    {
        [Browsable(false)]
        public Guid Id { get; set; } = Guid.Empty;

        // --- RELACIONES ---
        [Browsable(false)]
        public Guid ClientId { get; set; }
        public ClientDTO Client { get; set; }

        [Browsable(false)]
        public Guid EmployeeId { get; set; }
        public EmployeeDTO Employee { get; set; }

        // --- DATOS DE VENTA ---
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }

        // Agregado para paridad con la Entidad
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // --- DETALLE ---
        public IEnumerable<SaleDetailDTO> Items { get; set; }

        // --- CAMPOS TÉCNICOS (Integridad y Auditoría) ---
        [Browsable(false)]
        public bool IsDeleted { get; set; }

        [Browsable(false)]
        public string DVH { get; set; }
    }
}