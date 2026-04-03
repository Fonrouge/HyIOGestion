using System;

namespace Domain.Entities.Suppliers.ValueObjects
{
    public sealed class SupplierAddressVO : IValueObject
    {
        public string Value { get; }

        private SupplierAddressVO(string value) => Value = value;

        public static SupplierAddressVO Create(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("La dirección no puede estar vacía.");

            var cleaned = address.Trim();

            // Longitud razonable para una dirección física
            if (cleaned.Length < 5 || cleaned.Length > 150)
                throw new ArgumentException("La dirección debe tener entre 5 y 150 caracteres.");

            // Retornamos normalizado (lo pasamos a mayúsculas si tu sistema prefiere ese estándar, 
            // o lo dejamos con su case original limpio)
            return new SupplierAddressVO(cleaned.Trim().ToUpper());
        }
    }
}