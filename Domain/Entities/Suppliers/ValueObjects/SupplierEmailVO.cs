using System;

namespace Domain.Entities.Suppliers.ValueObjects
{
    public sealed class SupplierEmailVO : IValueObject
    {
        public object Value { get; }

        private SupplierEmailVO(string value) => Value = value;

        public static SupplierEmailVO Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío");

            if (email.Length > 30 || !email.Contains("@"))
                throw new ArgumentException("El email no tiene un formato válido. Máximo 30 caracteres y debe contener '@'");

            return new SupplierEmailVO(email.Trim().ToUpper());
        }
    }
}
