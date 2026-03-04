using System;

namespace Domain.Entities.Sales.ValueObjects
{
    public sealed class TotalAmountVO : IValueObject
    {
        public decimal Value { get; }

        private TotalAmountVO(decimal value) => Value = value;

        public static TotalAmountVO Create(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("El monto total no puede ser negativo.");

            // Redondeo a 2 decimales (estándar monetario esencial)
            var roundedAmount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);

            return new TotalAmountVO(roundedAmount);
        }
    }
}