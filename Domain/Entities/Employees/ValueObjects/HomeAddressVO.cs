using Domain.Entities;
using System.Linq;
using System;

public sealed class HomeAddressVO : IValueObject
{
    public string Value { get; }

    private HomeAddressVO(string value) => Value = value;

    public static HomeAddressVO Create(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("La dirección no puede estar vacía");

        var cleaned = address.Trim();

        if (cleaned.Length < 5 || cleaned.Length > 60)
            throw new ArgumentException("La dirección debe tener entre 5 y 60 caracteres");

        if (!cleaned.Any(char.IsLetter))
            throw new ArgumentException("La dirección debe contener al menos una letra");

        // Regla coherente: no puede ser solo números o símbolos (evita "1234" o "///")
        if (cleaned.All(c => char.IsDigit(c) || char.IsPunctuation(c) || char.IsWhiteSpace(c)))
            throw new ArgumentException("La dirección debe contener texto descriptivo (calle, barrio, etc.)");

        return new HomeAddressVO(cleaned);
    }
}