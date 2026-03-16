using Domain.BaseContracts;
using Domain.Entities.Payments.ValueObjects;
using System;

namespace Domain.Entities
{
    public class Payment : EntityBase, ISoftDeletable
    {
        public PaymentAmountVO Amount { get; private set; }
        public DateTime CreationDate { get; private set; }
        public DateTime EffectiveDate { get; private set; }
        public Guid ClientId { get; private set; }
        public PaymentMethodVO Method { get; private set; }
        public PaymentReferenceVO Reference { get; private set; }
        public DvhVo DVH { get; private set; }
        public bool IsDeleted { get; private set; }

        private Payment() { }

        public static Payment Create(
            decimal rawAmount,
            Guid clientId,
            string rawMethod,
            string rawReference)
        {
            var payment = new Payment();
            var now = DateTime.UtcNow;

            payment.Amount = PaymentAmountVO.Create(rawAmount);
            payment.CreationDate = now;
            payment.EffectiveDate = now;
            payment.ClientId = clientId;
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
            Guid clientId,
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
                ClientId = clientId,
                Method = PaymentMethodVO.Create(rawMethod ?? string.Empty),
                Reference = PaymentReferenceVO.Create(rawReference ?? string.Empty),
                DVH = !string.IsNullOrEmpty(dvh) ? DvhVo.Create(dvh) : null,
                IsDeleted = isDeleted
            };
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