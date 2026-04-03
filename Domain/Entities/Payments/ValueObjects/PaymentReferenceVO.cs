using System;
using System.Text.RegularExpressions;

namespace Domain.Entities.Payments.ValueObjects
{
    public sealed class PaymentReferenceVO : IValueObject
    {
        public string Value { get; }

        // Constructor privado: la única forma de instanciarlo es mediante Create()
        private PaymentReferenceVO(string value)
        {
            Value = value;
        }

        public static PaymentReferenceVO Create(string value)
        {
            // 1. Regla: No debe ser null, ni estar vacío, ni ser puros espacios.
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("La referencia de pago no puede ser nula ni estar vacía.", nameof(value));

            var sanitizedValue = value.Trim();

            // 2. Regla: Longitud entre 2 y 20 caracteres.
            if (sanitizedValue.Length < 2 || sanitizedValue.Length > 20)
                throw new ArgumentException($"La referencia de pago debe tener entre 2 y 20 caracteres. Longitud actual: {sanitizedValue.Length}.");

            // 3. Regla: Solo letras, números, guiones (-), puntos (.) y numeral (#).
            if (!Regex.IsMatch(sanitizedValue, @"^[a-zA-Z0-9\-.#]+$"))
                throw new ArgumentException("La referencia de pago contiene caracteres inválidos. Solo se permiten letras, números, guiones (-), puntos (.) y numeral (#).");

            return new PaymentReferenceVO(sanitizedValue.Trim().ToUpper());
        }

        // --- COMPORTAMIENTO DE VALUE OBJECT (Igualdad basada en el valor) ---

        public override bool Equals(object obj)
        {
            if (obj is null || obj.GetType() != GetType())
                return false;

            var other = (PaymentReferenceVO)obj;
            return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Value != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Value) : 0;
        }

        public static bool operator ==(PaymentReferenceVO left, PaymentReferenceVO right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null)) return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) return false;
            return left.Equals(right);
        }

        public static bool operator !=(PaymentReferenceVO left, PaymentReferenceVO right)
            => !(left == right);

        public override string ToString() => Value;
    }
}