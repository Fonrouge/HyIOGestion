using System;

namespace Domain.Entities.Sales.ValueObjects
{
    public sealed class QuantityVO : IValueObject
    {
        public decimal Value { get; }

        private QuantityVO(decimal value) => Value = value;

        public static QuantityVO Create(decimal quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero.");

            return new QuantityVO(quantity);
        }
    }
}