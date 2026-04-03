using System;

namespace Domain.Entities.Products.ValueObjects
{
    public class DescriptionVO : IValueObject
    {
        public string Value { get; }

        private DescriptionVO(string value) => Value = value;

        public static DescriptionVO Create(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La descripción no puede ser nula o vacía.", nameof(description));

            string cleanDescription = description.Trim();

            // Límite razonable (ej: 500 caracteres)
            if (cleanDescription.Length > 500)
                throw new ArgumentException("La descripción no puede superar los 500 caracteres.", nameof(description));

            return new DescriptionVO(cleanDescription.Trim().ToUpper());
        }

    }
}