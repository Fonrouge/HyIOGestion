using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.ComponentModel;

namespace BLL.LogicLayers
{
    public class SaleDetailDTO : IDto
    {
        [Browsable(false)]
        public Guid Id { get; set; } = Guid.Empty;

        [Browsable(false)]
        public Guid SaleId { get; set; }

        public Guid ProductId { get; set; }

        // Manteniendo decimal para soportar ventas por peso/fracción como en Product
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }

        // --- CAMPOS TÉCNICOS DE INTEGRIDAD ---
        [Browsable(false)]
        public bool IsDeleted { get; set; }

        [Browsable(false)]
        public string DVH { get; set; }

        public override string ToString()
        {
            return $"{Quantity} x {UnitPrice:C2} = {Subtotal:C2} (Producto: {ProductId})";
        }
    }
}