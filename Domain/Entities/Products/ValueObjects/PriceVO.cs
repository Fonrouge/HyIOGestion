using System;
using System.Globalization; // Necesario para InvariantCulture
using System.Text.RegularExpressions;

namespace Domain.Entities.Products.ValueObjects
{
    public class PriceVO
    {
        public decimal Value { get; }

        private PriceVO(decimal value) => Value = value;

        public static PriceVO Create(string priceInput)
        {
            // 1. Validación de Nulidad
            if (string.IsNullOrWhiteSpace(priceInput))
                throw new ArgumentNullException(nameof(priceInput), "El precio no puede estar vacío.");

            // 2. Validación de formato (Acepta punto o coma)
            if (!Regex.IsMatch(priceInput, @"^[0-9]+([.,][0-9]{1,2})?$"))
                throw new ArgumentException("Formato inválido. Use números con hasta 2 decimales (ej: 10.50 o 10,50).", nameof(priceInput));

            // 3. Normalización: Convertimos coma en punto para unificar
            string normalizedInput = priceInput.Replace(",", ".");

            // 4. Conversión usando Cultura Invariante (Punto siempre es decimal)
            if (!decimal.TryParse(normalizedInput, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedPrice))
                throw new FormatException("No se pudo procesar el valor numérico del precio.");

            // 5. Validaciones de Negocio
            if (parsedPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(priceInput), "El precio no puede ser negativo.");

            if (Math.Truncate(parsedPrice) > 99_999_999)
                throw new ArgumentException("El precio supera el límite permitido de 8 cifras enteras.", nameof(priceInput));

            return new PriceVO(parsedPrice);
        }
    }
}