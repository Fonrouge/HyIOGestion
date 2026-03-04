using System;
using System.Text.RegularExpressions;

namespace Domain.Entities.Clients.ValueObjects
{
    public sealed class ClientTaxIdVO : IValueObject
    {
        public string Value { get; private set; }

        private ClientTaxIdVO(string value)
        {
            Value = value;
        }

        public static ClientTaxIdVO Create(string taxId)
        {
            if (string.IsNullOrWhiteSpace(taxId))
                throw new ArgumentException("La categoría fiscal no puede estar vacía o ser nula.", nameof(taxId));

            taxId = taxId.Trim();

            // 1. Manejo estricto de obligatoriedad
            if (taxId == "N/I")
                throw new ArgumentException("La categoría fiscal es un dato obligatorio.", nameof(taxId));

            // 2. Validación de Longitud (ej: "Exento" tiene 6, "Responsable Inscripto" tiene 21)
            if (taxId.Length < 3 || taxId.Length > 50)
                throw new ArgumentException("La categoría fiscal debe tener entre 3 y 50 caracteres.", nameof(taxId));

            // 3. Validación de Caracteres (Solo letras, espacios y tildes/eñe)
            if (!Regex.IsMatch(taxId, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                throw new ArgumentException("La categoría fiscal contiene caracteres inválidos. Solo se permiten letras y espacios.", nameof(taxId));

            return new ClientTaxIdVO(taxId);
        }

        // --- COMPORTAMIENTO DE VALUE OBJECT ---

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (ClientTaxIdVO)obj;
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}