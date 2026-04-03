using Domain.Contracts;
using Domain.Entities.Payments.ValueObjects;
using System;

namespace Domain.Entities
{
    public class Payment : EntityBase, ISoftDeletable, IIntegrityCheckable
    {
        public PaymentAmountVO Amount { get; private set; }
        public DateTime CreationDate { get; private set; }
        public DateTime EffectiveDate { get; private set; }
        public Guid SaleId { get; private set; }
        public PaymentMethodVO Method { get; private set; }
        public PaymentReferenceVO Reference { get; private set; }
        public DvhVo DVH { get; private set; }
        public bool IsDeleted { get; private set; }

        private Payment() { }

        public static Payment Create(
            decimal rawAmount,
            Guid saleId,
            string rawMethod,
            string rawReference)
        {
            var payment = new Payment();
            var now = DateTime.UtcNow;

            payment.Amount = PaymentAmountVO.Create(rawAmount);
            payment.CreationDate = now;
            payment.EffectiveDate = now;
            payment.SaleId = saleId;
            payment.Method = PaymentMethodVO.Create(rawMethod?.Trim() ?? string.Empty);
            payment.Reference = PaymentReferenceVO.Create(rawReference?.Trim() ?? string.Empty);
            payment.IsDeleted = false;
            // DVH se calculará antes de persistir
            return payment;
        }

        public static Payment Reconstitute
        (
            Guid id,
            decimal rawAmount,
            DateTime creationDate,
            DateTime effectiveDate,
            Guid saleId,
            string rawMethod,
            string rawReference,
            string dvh,
            bool isDeleted)
        {
            return new Payment()
            {
                Id = id,
                Amount = PaymentAmountVO.Create(rawAmount),
                CreationDate = creationDate,
                EffectiveDate = effectiveDate,
                SaleId = saleId,
                Method = PaymentMethodVO.Create(rawMethod ?? string.Empty),
                Reference = PaymentReferenceVO.Create(rawReference ?? string.Empty),
                DVH = !string.IsNullOrEmpty(dvh) ? DvhVo.Create(dvh) : null,
                IsDeleted = isDeleted
            };
        }

        public string GetDvhSerialization()
        {
            // Usamos cultura invariante para normalizar fechas, decimales y Guids
            var culture = System.Globalization.CultureInfo.InvariantCulture;

            return string.Join("|",
                Id.ToString(),
                Amount.Value.ToString("F2", culture),
                CreationDate.ToString("yyyyMMddHHmmss", culture),
                EffectiveDate.ToString("yyyyMMddHHmmss", culture),
                SaleId.ToString(),
                Method.Value,    // Ya viene en Mayúsculas por su VO
                Reference.Value, // Ya viene en Mayúsculas por su VO
                IsDeleted ? "1" : "0"
            );
        }
        

        public void MarkAsEffective(DateTime effectiveDate)
        {
            if (effectiveDate < CreationDate)
                throw new InvalidOperationException("La fecha efectiva no puede ser anterior a la de creación.");

            EffectiveDate = effectiveDate;
        }

        public void MarkAsDeleted()
        {
            if (IsDeleted) return;
            IsDeleted = true;
        }

        public void UpdateDVH(string newDvh)
        {
            DVH = DvhVo.Create(newDvh ?? string.Empty);
        }
    }
}