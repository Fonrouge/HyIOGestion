using System;
using System.Text.RegularExpressions;

namespace Domain.Entities.Clients.ValueObjects
{
    public sealed class ClientNameVO : IValueObject
    {
        // Propiedad de solo lectura
        public object Value { get; private set; }

        // Constructor privado para forzar el uso del Factory Method
        private ClientNameVO(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Factory Method para crear y validar el Value Object.
        /// </summary>
        public static ClientNameVO Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del cliente no puede estar vacío o ser nulo.", nameof(name));

            name = name.Trim();

            // 1. Manejo del valor por defecto (Not Initialized)
            if (name == "N/I" || string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del cliente es un dato obligatorio.", nameof(name));

            // 2. Validación de Longitud (Mínimo 2, Máximo 50 caracteres)
            if (name.Length < 2 || name.Length > 50)
                throw new ArgumentException("El nombre del cliente debe tener entre 2 y 50 caracteres.", nameof(name));

            // 3. Validación de Caracteres (Solo letras, acentos, ñ, espacios y guiones)
            // Evita números y símbolos raros como @, !, $, etc.
            if (!Regex.IsMatch(name, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-]+$"))
                throw new ArgumentException("El nombre del cliente contiene caracteres inválidos. Sólo se permiten letras.", nameof(name));

            return new ClientNameVO(name.ToUpper());
        }

        // --- COMPORTAMIENTO DE VALUE OBJECT (Comparación por valor) ---

        public override bool Equals(object obj)
        {
            if (obj is null || GetType() != obj.GetType())
                return false;

            var other = (ClientNameVO)obj;
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString() => Value.ToString();

        // Sobrecarga de operadores opcional pero recomendada para VOs
        public static bool operator ==(ClientNameVO left, ClientNameVO right)
        {
            if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
            return left.Equals(right);
        }

        public static bool operator !=(ClientNameVO left, ClientNameVO right)
        {
            return !(left == right);
        }
    }
}