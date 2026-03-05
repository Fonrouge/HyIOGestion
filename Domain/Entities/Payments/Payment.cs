using Domain.BaseContracts;
using Domain.Entities.Payments.ValueObjects; // Asegúrate de crear este namespace
using System;

namespace Domain.Entities
{
    public class Payment : EntityBase, ISoftDeletable
    {
        // --- PROPIEDADES DE DOMINIO ---
        public PaymentAmountVO Amount { get; private set; }
        public DateTime CreationDate { get; private set; }
        public DateTime EffectiveDate { get; private set; }

        public Guid ClientId { get; private set; } // Identificador de relación

        public PaymentMethodVO Method { get; private set; }
        public PaymentReferenceVO Reference { get; private set; }

        // --- CAMPOS TÉCNICOS ---
        public string DVH { get; private set; } // Vital para proteger la integridad del monto y la fecha

        public bool IsDeleted { get; private set; }

        // Constructor privado para forzar el uso de Factories
        private Payment() { }

        /// <summary>
        /// ÚNICO punto de creación para un NUEVO Payment.
        /// Asume que la fecha de creación y efectividad son el momento actual por defecto.
        /// </summary>
        public static Payment Create(
            decimal rawAmount,
            Guid clientId,
            string rawMethod,
            string rawReference = "")
        {
            var now = DateTime.UtcNow;

            return new Payment
            {
                // El Id se genera en EntityBase
                Amount = PaymentAmountVO.Create(rawAmount),
                CreationDate = now,
                EffectiveDate = now,
                ClientId = clientId,
                Method = PaymentMethodVO.Create(rawMethod?.Trim()),
                Reference = PaymentReferenceVO.Create(rawReference?.Trim()),
                DVH = string.Empty
            };
        }

        /// <summary>
        /// Reconstruye un Payment EXISTENTE desde la base de datos.
        /// </summary>
        public static Payment Reconstitute(
            Guid id,
            decimal rawAmount,
            DateTime creationDate,
            DateTime effectiveDate,
            Guid clientId,
            string rawMethod,
            string rawReference,
            string dvh)
        {
            return new Payment
            {
                Id = id,
                Amount = PaymentAmountVO.Create(rawAmount),
                CreationDate = creationDate,
                EffectiveDate = effectiveDate,
                ClientId = clientId,
                Method = PaymentMethodVO.Create(rawMethod),
                Reference = PaymentReferenceVO.Create(rawReference),
                DVH = dvh ?? string.Empty
            };
        }

        // --- COMPORTAMIENTO ---

        /// <summary>
        /// Permite actualizar la fecha efectiva si el pago se acredita en diferido (ej: Cheques o Transferencias que demoran).
        /// </summary>
        public void MarkAsEffective(DateTime effectiveDate)
        {
            if (effectiveDate < CreationDate)
                throw new InvalidOperationException("La fecha efectiva no puede ser anterior a la fecha de creación.");

            EffectiveDate = effectiveDate;
        }

        public void MarkAsDeleted()
        {
            if (IsDeleted) return; 
            IsDeleted = true;           
        }

        public void UpdateDVH(string newDvh)
        {
            if (string.IsNullOrWhiteSpace(newDvh))
                throw new ArgumentException("El DVH no puede estar vacío.", nameof(newDvh));
            DVH = newDvh;
        }
    }
}