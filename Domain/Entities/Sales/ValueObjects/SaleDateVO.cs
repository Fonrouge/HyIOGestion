using System;

namespace Domain.Entities.Sales.ValueObjects
{
    public sealed class SaleDateVO : IValueObject
    {
        public DateTime Value { get; }

        private SaleDateVO(DateTime value) => Value = value;

        public static SaleDateVO Create(DateTime date)
        {
            if (date == DateTime.MinValue)
                throw new ArgumentException("La fecha de venta no puede ser una fecha inválida.");

            var utcDate = date.Kind == DateTimeKind.Utc
                ? date
                : date.ToUniversalTime();

            if (utcDate > DateTime.UtcNow.AddMinutes(600))
                throw new ArgumentException("La fecha de venta no puede ser futura.");

            return new SaleDateVO(utcDate);
        }
    }
}