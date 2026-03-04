using Domain.Entities;
using System;

public class StockVO : IValueObject
{    
    public decimal Value { get; }

    private StockVO(decimal value) => Value = value;

    public static StockVO Create(string stockInput)
    {
        if (stockInput == null)
            throw new ArgumentNullException(nameof(stockInput));

        if (!decimal.TryParse(stockInput, out var parsedValue))
            throw new ArgumentException("Invalid format for stock", nameof(stockInput));

        if (parsedValue < 0)
            throw new ArgumentOutOfRangeException(nameof(stockInput), "Stock cannot be below 0");

        return new StockVO(parsedValue);
    }     
}