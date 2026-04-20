using System;
using System.Linq;

namespace Domain.Entities.Suppliers.ValueObjects
{
    public sealed class SupplierCityVO : IValueObject
    {
        public object Value { get; }

        private SupplierCityVO(string value) => Value = value;

        public static SupplierCityVO Create(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("La ciudad no puede estar vacía.");

            var cleaned = city.Trim();

            // Limites razonables para nombres de ciudades
            if (cleaned.Length < 3 || cleaned.Length > 50)
                throw new ArgumentException("La ciudad debe tener entre 3 y 50 caracteres.");

            // Validación de dominio: Las ciudades no tienen números
            if (cleaned.Any(char.IsDigit))
                throw new ArgumentException("El nombre de la ciudad no puede contener números.");

            // Normalizamos
            return new SupplierCityVO(cleaned.Trim().ToUpper());
        }
    }
}