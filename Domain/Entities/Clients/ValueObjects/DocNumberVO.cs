using System;
using System.Text.RegularExpressions;

namespace Domain.Entities.Clients.ValueObjects
{
    public sealed class DocNumberVO : IValueObject
    {
        public string Value { get; private set; }

        private DocNumberVO(string value)
        {
            Value = value;
        }

        public static DocNumberVO Create(string docNumber)
        {
            if (string.IsNullOrWhiteSpace(docNumber))
                throw new ArgumentException("El número de documento no puede estar vacío o ser nulo.", nameof(docNumber));

            docNumber = docNumber.Trim();

            // 1. Manejo estricto de obligatoriedad
            if (docNumber == "N/I")
                throw new ArgumentException("El número de documento es un dato obligatorio.", nameof(docNumber));

            // 2. Sanitización: Quitamos puntos y guiones que suelen venir en DNIs o CUITs/CUILs
            docNumber = docNumber.Replace(".", "").Replace("-", "");

            // 3. Validación de Longitud (DNI: 7-8 dígitos, CUIT/CUIL: 11 dígitos)
            if (docNumber.Length < 7 || docNumber.Length > 11)
                throw new ArgumentException("El número de documento debe tener entre 7 y 11 dígitos válidos.", nameof(docNumber));

            // 4. Validación de Caracteres (Solo números)
            if (!Regex.IsMatch(docNumber, @"^[0-9]+$"))
                throw new ArgumentException("El número de documento contiene caracteres inválidos. Solo se permiten números.", nameof(docNumber));

            return new DocNumberVO(docNumber);
        }

        // --- COMPORTAMIENTO DE VALUE OBJECT ---

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (DocNumberVO)obj;
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}