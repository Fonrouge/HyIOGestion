using Domain.Entities;
using System;

public sealed class FirstNameVO : IValueObject
{
    public object Value { get; }

    private FirstNameVO(string value) => Value = value;

    public static FirstNameVO Create(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("El nombre no puede estar vacío");

        if (firstName.Length > 50)
            throw new ArgumentException("El nombre no puede superar los 50 caracteres");

        return new FirstNameVO(firstName.Trim().ToUpper());
    }
}