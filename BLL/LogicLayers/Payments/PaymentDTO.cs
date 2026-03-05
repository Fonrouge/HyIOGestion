using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.ComponentModel;

namespace BLL.LogicLayers
{
    public class PaymentDTO : IDto
    {
        [Browsable(false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public decimal Amount { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime EffectiveDate { get; set; }

        [Browsable(false)]
        public Guid ClientId { get; set; } 

        public string Method { get; set; } 
        public string Reference { get; set; }
    }
}