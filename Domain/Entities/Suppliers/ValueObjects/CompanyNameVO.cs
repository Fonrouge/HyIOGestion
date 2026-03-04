using System;
using System.Text.RegularExpressions;

namespace Domain.Entities.Suppliers.ValueObjects
{
    public sealed class CompanyNameVO : IValueObject
    {
        // Propiedad de solo lectura
        public string Value { get; private set; }

        // Constructor privado para forzar el uso del Factory Method
        private CompanyNameVO(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Factory Method para crear y validar el Value Object.
        /// </summary>
        public static CompanyNameVO Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del cliente no puede estar vacío o ser nulo.", nameof(name));

            name = name.Trim();

            // 1. Manejo del valor por defecto (Not Initialized)
            if (name == "N/I" || string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre de la empresa es un dato obligatorio.", nameof(name));

            
            // 2. Validación de Longitud (Mínimo 2, Máximo 50 caracteres)
            if (name.Length < 2 || name.Length > 50)
                throw new ArgumentException("El nombre de la empresa debe tener entre 2 y 50 caracteres.", nameof(name));

            
            // 3. Validación de Caracteres (Solo letras, acentos, ñ, espacios y guiones)
            // Evita números y símbolos raros como @, !, $, etc.
            if (!Regex.IsMatch(name, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-]+$"))
                throw new ArgumentException("El nombre de la empresa contiene caracteres inválidos. Solo se permiten letras.", nameof(name));

            return new CompanyNameVO(name);
        }

        // --- COMPORTAMIENTO DE VALUE OBJECT (Comparación por valor) ---

        public override bool Equals(object obj)
        {
            if (obj is null || GetType() != obj.GetType())
                return false;

            var other = (CompanyNameVO)obj;
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString() => Value;


        // Sobrecarga de operadores opcional pero recomendada para VOs
        public static bool operator ==(CompanyNameVO left, CompanyNameVO right)
        {
            if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
            return left.Equals(right);
        }

        public static bool operator !=(CompanyNameVO left, CompanyNameVO right)
        {
            return !(left == right);
        }
    }
}