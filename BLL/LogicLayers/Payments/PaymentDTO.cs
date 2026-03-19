using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.ComponentModel;

namespace BLL.DTOs
{
    public class PaymentDTO : IDto
    {
        [Browsable(false)]
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EffectiveDate { get; set; }

        [Browsable(false)]
        public Guid SaleId { get; set; }
        public string Method { get; set; }
        public string Reference { get; set; }


        // --- CAMPOS DE CONTROL E INTEGRIDAD ---
        public bool IsDeleted { get; set; }

        [Browsable(false)]
        public string DVH { get; set; }

        public PaymentDTO()
        {
            Id = Guid.Empty;
            Amount = 0m;
            CreationDate = DateTime.UtcNow;
            EffectiveDate = DateTime.UtcNow;
            SaleId = Guid.Empty;
            Method = string.Empty;
            Reference = string.Empty;
            DVH = string.Empty;
            IsDeleted = false;
        }

        public override string ToString()
        {
            return string.Format("Monto: {0} - Fecha: {1:dd/MM/yyyy}", Amount, EffectiveDate);
        }
    }
}