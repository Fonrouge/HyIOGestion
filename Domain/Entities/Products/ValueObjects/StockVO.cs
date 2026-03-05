using System;
using System.Globalization;

namespace Domain.Entities.Products.ValueObjects
{
    public class StockVO
    {
        public decimal Value { get; }

        // Constructor privado para garantizar que solo se cree mediante validación
        private StockVO(decimal value) => Value = value;

        /// <summary>
        /// Crea el VO desde un decimal. Ideal para Reconstitución desde DB o cálculos internos.
        /// </summary>
        public static StockVO Create(decimal stockInput)
        {
            // Regla de Oro: El stock no puede ser negativo en este sistema
            if (stockInput < 0)
                throw new ArgumentOutOfRangeException(nameof(stockInput), "El stock no puede ser inferior a 0.");

            // Opcional: Podés limitar la cantidad de decimales (ej: 3 para gramos/mililitros)
            decimal normalizedStock = Math.Round(stockInput, 3, MidpointRounding.AwayFromZero);

            return new StockVO(normalizedStock);
        }

        /// <summary>
        /// Crea el VO desde un string. Ideal para capturar datos desde un TextBox en WinForms.
        /// </summary>
        public static StockVO Create(string stockInput)
        {
            // 1. Validación de Nulidad/Vacío
            if (string.IsNullOrWhiteSpace(stockInput))
                throw new ArgumentNullException(nameof(stockInput), "El valor de stock es requerido.");

            // 2. Normalización de cultura (reemplazo de coma por punto)
            string cleanedInput = stockInput.Trim().Replace(",", ".");

            // 3. Conversión segura usando cultura invariante
            if (!decimal.TryParse(cleanedInput, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue))
                throw new ArgumentException("El formato de stock es inválido. Use números (ej: 10 o 10.5).", nameof(stockInput));

            // 4. Delegamos a la validación de negocio
            return Create(parsedValue);
        }

        // Para mostrarlo de forma amigable en la UI
        public override string ToString() => Value.ToString("N3", CultureInfo.CurrentCulture);
    }
}