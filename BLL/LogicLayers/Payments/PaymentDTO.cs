using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.ComponentModel;

namespace BLL.LogicLayers
{
    public class PaymentDTO : IDto
    {
        [Browsable(false)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Amount { get; set; }
        public string CreationDate { get; set; } = DateTime.UtcNow.ToString();
        public string EffectiveDate { get; set; }
        
        [Browsable(false)]
        public string ClientId { get; set; } // Relación con el Cliente

        public string Method { get; set; } // Método de pago (Efectivo, Transferencia, Tarjeta, etc.)
        public string Reference { get; set; }


    }
}
