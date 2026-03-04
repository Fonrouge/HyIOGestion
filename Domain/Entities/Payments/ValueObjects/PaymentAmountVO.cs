using System;

namespace Domain.Entities.Payments.ValueObjects
{
    public sealed class PaymentAmountVO : IValueObject
    {
        public decimal Value { get; }

        private PaymentAmountVO(decimal value)
        {
            Value = value;
        }

        public static PaymentAmountVO Create(decimal value)
        {
            // 1. Regla: El pago debe ser estrictamente positivo
            if (value <= 0)
                throw new ArgumentException("El monto del pago debe ser mayor a cero.", nameof(value));

            // 2. Regla: Evitar decimales infinitos o fraccionales no soportados por la moneda (máximo 2 decimales).
            // Si al redondear a 2 decimales el valor cambia, significa que venía con basura fraccional.
            if (decimal.Round(value, 2) != value)
                throw new ArgumentException("El monto del pago no puede tener más de dos cifras decimales.", nameof(value));

            return new PaymentAmountVO(value);
        }

        // --- COMPORTAMIENTO DE VALUE OBJECT ---

        public override bool Equals(object obj)
        {
            if (obj is null || obj.GetType() != GetType())
                return false;

            var other = (PaymentAmountVO)obj;
            // Para los decimales, la comparación directa es segura en C#
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(PaymentAmountVO left, PaymentAmountVO right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null)) return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) return false;
            return left.Equals(right);
        }

        public static bool operator !=(PaymentAmountVO left, PaymentAmountVO right)
            => !(left == right);

        // Formateamos la salida a dos decimales por defecto (ej: "150.00")
        public override string ToString() => Value.ToString("F2");
    }
}