using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.ComponentModel;

namespace BLL.LogicLayers
{
    public class SaleDetailDTO : IDto
    {
        [Browsable(false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Browsable(false)]
        public Guid SaleId { get; set; }

        public Guid ProductId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Subtotal { get; set; }

        public override string ToString()
        {
            // Mismo formato que la entidad para mantener consistencia visual en las grillas o logs
            return $"{Quantity} x {UnitPrice:C2} = {Subtotal:C2} (Producto: {ProductId})";
        }
    }
}