using System;
using System.Text.RegularExpressions;

namespace Domain.Entities.Clients.ValueObjects
{
    public sealed class ZipCodeVO : IValueObject
    {
        public object Value { get; private set; }

        private ZipCodeVO(string value)
        {
            Value = value;
        }

        public static ZipCodeVO Create(string address)
        {
            //1. Se quitan los espacios
            string cleanedValue = Regex.Replace(address, @"\s+", "").ToUpper();

            // 2. Validación de Longitud (4-10 caracteres)
            if (cleanedValue.Length < 4 || cleanedValue.Length > 10)
                throw new ArgumentException("El código postal debe tener entre 4 y 10 caracteres.", nameof(address));

            return new ZipCodeVO(cleanedValue);
        }

        // --- COMPORTAMIENTO DE VALUE OBJECT ---

        public override bool Equals(object obj)
        {
            if (obj is null || GetType() != obj.GetType())
                return false;

            var other = (ZipCodeVO)obj;
            // Importante: al ser 'object', usar Equals() para comparar el contenido de los strings
            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString() => Value.ToString();

        public static bool operator ==(ZipCodeVO left, ZipCodeVO right)
        {
            if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
            return left.Equals(right);
        }

        public static bool operator !=(ZipCodeVO left, ZipCodeVO right)
        {
            return !(left == right);
        }
    }
}