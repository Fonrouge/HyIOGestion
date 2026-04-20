using Domain.Entities;
using System;

public sealed class LastNameVO : IValueObject
{
    public object Value { get; }

    private LastNameVO(string value) => Value = value;

    public static LastNameVO Create(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("El apellido no puede estar vacío");

        if (lastName.Length > 50)
            throw new ArgumentException("El apellido no puede superar los 50 caracteres");


        return new LastNameVO(lastName.Trim().ToUpper());
    }
}