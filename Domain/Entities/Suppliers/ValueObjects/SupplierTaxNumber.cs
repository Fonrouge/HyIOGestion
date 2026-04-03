using System;

namespace Domain.Entities.Suppliers.ValueObjects
{
    public sealed class SupplierTaxNumber : IValueObject
    {
        public string Value { get; private set; }

        private SupplierTaxNumber(string value)
        {
            Value = value;
        }

        public static SupplierTaxNumber Create(string taxNumber)
        {
            if (string.IsNullOrWhiteSpace(taxNumber))
                throw new ArgumentException("El número fiscal no puede estar vacía o ser nula.", nameof(taxNumber));

            taxNumber = taxNumber.Trim();

            // 1. Manejo estricto de obligatoriedad
            if (taxNumber == "N/I")
                throw new ArgumentException("El número fiscal es un dato obligatorio.", nameof(taxNumber));

            // 2. Validación de Longitud (ej: "Exento" tiene 6, "Responsable Inscripto" tiene 21)
            if (taxNumber.Length < 2 || taxNumber.Length > 50)
                throw new ArgumentException("El número fiscal debe tener entre 5 y 20 caracteres.", nameof(taxNumber));


            return new SupplierTaxNumber(taxNumber);
        }

        // --- COMPORTAMIENTO DE VALUE OBJECT ---

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (SupplierTaxNumber)obj;
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