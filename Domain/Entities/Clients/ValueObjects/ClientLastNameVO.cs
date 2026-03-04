using System;
using System.Text.RegularExpressions;

namespace Domain.Entities.Clients.ValueObjects
{
    public class ClientLastNameVO : IValueObject
    {
        public string Value { get; private set; }

        private ClientLastNameVO(string value)
        {
            Value = value;
        }

        public static ClientLastNameVO Create(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("El apellido del cliente no puede estar vacío o ser nulo.", nameof(lastName));

            lastName = lastName.Trim();

            // 1. Manejo estricto de obligatoriedad (Rechaza el valor por defecto)
            if (lastName == "N/I")
                throw new ArgumentException("El apellido del cliente es un dato obligatorio.", nameof(lastName));

            // 2. Validación de Longitud (Mínimo 2, Máximo 50 caracteres)
            if (lastName.Length < 2 || lastName.Length > 50)
                throw new ArgumentException("El apellido del cliente debe tener entre 2 y 50 caracteres.", nameof(lastName));

            // 3. Validación de Caracteres (Solo letras, acentos, ñ, espacios y guiones)
            if (!Regex.IsMatch(lastName, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-]+$"))
                throw new ArgumentException("El apellido del cliente contiene caracteres inválidos. Solo se permiten letras.", nameof(lastName));

            return new ClientLastNameVO(lastName);
        }

        // --- COMPORTAMIENTO DE VALUE OBJECT (Comparación por valor) ---

        public override bool Equals(object obj)
        {
            if (obj is null || GetType() != obj.GetType())
                return false;

            var other = (ClientLastNameVO)obj;
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString() => Value;

        public static bool operator ==(ClientLastNameVO left, ClientLastNameVO right)
        {
            if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
            return left.Equals(right);
        }

        public static bool operator !=(ClientLastNameVO left, ClientLastNameVO right)
        {
            return !(left == right);
        }
    }
}