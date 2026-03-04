using Domain.Entities.Clients.ValueObjects;
using System;

namespace Domain.Entities.Suppliers.ValueObjects
{
    public sealed class ContactNameVO : IValueObject
    {
        public string Value { get; private set; }

        private ContactNameVO(string value)
        {
            Value = value;
        }

        public static ContactNameVO Create(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("El nombre de contacto del cliente no puede estar vacío o ser nulo.", nameof(lastName));

            lastName = lastName.Trim();

            // 1. Manejo estricto de obligatoriedad (Rechaza el valor por defecto)
            if (lastName == "N/I")
                throw new ArgumentException("El nombre de contacto del cliente es un dato obligatorio.", nameof(lastName));

            // 2. Validación de Longitud (Mínimo 2, Máximo 50 caracteres)
            if (lastName.Length < 2 || lastName.Length > 50)
                throw new ArgumentException("El nombre de contacto del cliente debe tener entre 2 y 50 caracteres.", nameof(lastName));


            return new ContactNameVO(lastName);
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

   
    }
}