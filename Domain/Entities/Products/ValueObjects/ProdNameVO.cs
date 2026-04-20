using System;
using System.Text.RegularExpressions;

namespace Domain.Entities.Products.ValueObjects
{
    public class ProdNameVO : IValueObject
    {
        public object Value { get; }

        private ProdNameVO(string value) => Value = value;

        public static ProdNameVO Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(name));

            // Trim para evitar espacios accidentales al inicio/final
            string cleanName = name.Trim();

            // Validación de longitud (ej: entre 3 y 100 caracteres)
            if (cleanName.Length < 3 || cleanName.Length > 100)
                throw new ArgumentException("El nombre debe tener entre 3 y 100 caracteres.", nameof(name));

            // Opcional: Evitar caracteres de control o scripts (XSS básico)
            if (Regex.IsMatch(cleanName, @"[<>/{}[\]]"))
                throw new ArgumentException("El nombre contiene caracteres no permitidos.", nameof(name));

            return new ProdNameVO(cleanName.Trim().ToUpper());
        }

        public override string ToString() => Value.ToString();
    }
}