using System;

namespace Domain.Entities.Sales.ValueObjects
{
    public sealed class UnitPriceVO : IValueObject
{
    public object Value { get; }

    private UnitPriceVO(decimal value) => Value = value;

    public static UnitPriceVO Create(decimal price)
    {
        if (price < 0)
            throw new ArgumentException("El precio unitario no puede ser negativo.");

        // Redondeo monetario estándar
        var rounded = Math.Round(price, 2, MidpointRounding.AwayFromZero);

        return new UnitPriceVO(rounded);
    }
}
}