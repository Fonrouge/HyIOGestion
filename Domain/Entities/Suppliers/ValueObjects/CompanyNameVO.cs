using System;
using System.Text.RegularExpressions;

namespace Domain.Entities.Suppliers.ValueObjects
{
    public sealed class CompanyNameVO : IValueObject
    {
        // Propiedad de solo lectura
        public object Value { get; private set; }

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
           

            return new CompanyNameVO(name.Trim().ToUpper());
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

        public override string ToString() => Value.ToString();


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