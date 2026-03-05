using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Domain.Entities.Products.ValueObjects
{
    public class PriceVO
    {
        public decimal Value { get; }

        // Constructor privado: la única forma de existir es pasando por las validaciones
        private PriceVO(decimal value) => Value = value;

        /// <summary>
        /// Crea el VO desde un decimal (Uso interno del Dominio/Mappers)
        /// </summary>
        public static PriceVO Create(decimal priceInput)
        {
            // 1. Validaciones de Negocio puras
            if (priceInput < 0)
                throw new ArgumentOutOfRangeException(nameof(priceInput), "El precio no puede ser negativo.");

            // Validamos el límite de 8 cifras enteras (puedes ajustar el número según tu DB)
            if (Math.Truncate(priceInput) > 99_999_999)
                throw new ArgumentException("El precio supera el límite permitido del sistema.", nameof(priceInput));

            // 2. Redondeo automático a 2 decimales (opcional, pero recomendado en finanzas)
            decimal roundedPrice = Math.Round(priceInput, 2, MidpointRounding.AwayFromZero);

            return new PriceVO(roundedPrice);
        }

        /// <summary>
        /// Crea el VO desde un string (Uso para capturar datos de la UI)
        /// </summary>
        public static PriceVO Create(string priceInput)
        {
            // 1. Validación de Nulidad
            if (string.IsNullOrWhiteSpace(priceInput))
                throw new ArgumentNullException(nameof(priceInput), "El precio no puede estar vacío.");

            // 2. Limpieza y Normalización
            // Reemplazamos coma por punto para que el TryParse no se maree con la cultura
            string normalizedInput = priceInput.Trim().Replace(",", ".");

            // 3. Validación de formato vía Regex (Acepta solo números y un separador decimal)
            if (!Regex.IsMatch(normalizedInput, @"^[0-9]+(\.[0-9]{1,2})?$"))
                throw new ArgumentException("Formato de precio inválido. Use hasta 2 decimales.");

            // 4. Conversión segura
            if (!decimal.TryParse(normalizedInput, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedPrice))
                throw new FormatException("No se pudo procesar el valor numérico.");

            // 5. Delegamos a la validación de negocio
            return Create(parsedPrice);
        }

        // Para que el objeto se dibuje lindo en la UI automáticamente
        public override string ToString() => Value.ToString("C2", CultureInfo.CurrentCulture);
    }
}