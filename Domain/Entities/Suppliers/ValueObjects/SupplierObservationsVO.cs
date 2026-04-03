using System;

namespace Domain.Entities.Suppliers.ValueObjects
{
    public sealed class SupplierObservationsVO : IValueObject
    {
        public string Value { get; }

        private SupplierObservationsVO(string value) => Value = value;

        public static SupplierObservationsVO Create(string observations)
        {            
            var cleaned = observations?.Trim() ?? string.Empty;
            
            if (cleaned.Length > 500)
                throw new ArgumentException("Las observaciones no pueden superar los 500 caracteres.");

            return new SupplierObservationsVO(cleaned.Trim().ToUpper());
        }
    }
}