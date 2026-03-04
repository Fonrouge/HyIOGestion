using System;
using System.Text.RegularExpressions;

namespace Domain.Entities.Clients.ValueObjects
{
    public class WarehouseAddressVO : IValueObject
    {
        public string Value { get; private set; }

        private WarehouseAddressVO(string value)
        {
            Value = value;
        }

        public static WarehouseAddressVO Create(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("La dirección de envío no puede estar vacía o ser nula.", nameof(address));

            address = address.Trim();

            
          // if (address == "N/I")
          //     throw new ArgumentException("La dirección de envío es un dato obligatorio.", nameof(address));

            // 2. Validación de Longitud (Mínimo 2, Máximo 150 caracteres)
            if (address.Length < 2 || address.Length > 150)
                throw new ArgumentException("La dirección de envío debe tener entre 2 y 150 caracteres.", nameof(address));

            // 3. Validación de Caracteres (Letras, números, acentos, ñ, espacios, comas, puntos, guiones y símbolos de número/piso)
            if (!Regex.IsMatch(address, @"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\-\.,#ºª]+$"))
                throw new ArgumentException("La dirección de envío contiene caracteres inválidos.", nameof(address));

            return new WarehouseAddressVO(address);
        }

        // --- COMPORTAMIENTO DE VALUE OBJECT (Comparación por valor) ---

        public override bool Equals(object obj)
        {
            if (obj is null || GetType() != obj.GetType())
                return false;

            var other = (ShipAddressVO)obj;
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString() => Value;


    }
}