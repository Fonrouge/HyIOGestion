using System;
using System.Linq;

namespace Domain.Entities.Payments.ValueObjects
{
    public sealed class PaymentMethodVO : IValueObject
    {
        public object Value { get; }


        private PaymentMethodVO(string value)
        {
            Value = value;
        }

        public static PaymentMethodVO Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El método de pago no puede estar vacío.", nameof(value));

            // Normalizamos a mayúsculas y quitamos espacios para evitar inconsistencias
            var sanitizedValue = value.Trim().ToUpperInvariant();

            return new PaymentMethodVO(sanitizedValue.ToUpper());
        }

        // --- COMPORTAMIENTO DE VALUE OBJECT ---

        public override bool Equals(object obj)
        {
            if (obj is null || obj.GetType() != GetType())
                return false;

            var other = (PaymentMethodVO)obj;
            return string.Equals(Value.ToString(), other.Value.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Value != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Value) : 0;
        }

        public static bool operator ==(PaymentMethodVO left, PaymentMethodVO right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null)) return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) return false;
            return left.Equals(right);
        }

        public static bool operator !=(PaymentMethodVO left, PaymentMethodVO right)
            => !(left == right);

        public override string ToString() => Value.ToString();
    }
}