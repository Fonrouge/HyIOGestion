using System;
using System.Linq;

namespace Domain.Entities.Suppliers.ValueObjects
{
    public sealed class SupplierPhoneVO : IValueObject
    {
        public string Value { get; }

        private SupplierPhoneVO(string value) => Value = value;

        public static SupplierPhoneVO Create(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("El número de teléfono no puede estar vacío");

            var cleaned = phone.Trim();

            // 1. Longitud razonable
            if (cleaned.Length < 8 || cleaned.Length > 15)
                throw new ArgumentException("El número de teléfono debe tener entre 8 y 15 caracteres");

            // 2. NO permite letras (validación explícita que pediste)
            if (cleaned.Any(char.IsLetter))
                throw new ArgumentException("El número de teléfono no puede contener letras");

            // 3. Solo caracteres permitidos
            if (!cleaned.All(c => char.IsDigit(c) || c == '+' || c == ' ' || c == '-' || c == '(' || c == ')'))
                throw new ArgumentException("El teléfono solo puede contener dígitos, +, -, espacios o paréntesis");

            // 4. Debe tener al menos 7 dígitos reales
            if (cleaned.Count(char.IsDigit) < 7)
                throw new ArgumentException("El número de teléfono debe contener al menos 7 dígitos");

            // Normalizamos (quitamos espacios extras, pero mantenemos formato legible)
            return new SupplierPhoneVO(cleaned);
        }
    }
}
