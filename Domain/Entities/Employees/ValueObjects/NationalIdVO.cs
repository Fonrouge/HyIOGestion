using System;
using System.Linq;

namespace Domain.Entities.Employees.ValueObjects
{
    public sealed class NationalIdVO : IValueObject
    {
        public object Value { get; }

        private NationalIdVO(string value) => Value = value;

        public static NationalIdVO Create(string nationalId)
        {
            if (string.IsNullOrWhiteSpace(nationalId))
                throw new ArgumentException("El DNI no puede estar vacío");

            if (nationalId.Length != 8)
                throw new ArgumentException("El DNI debe tener exactamente 8 dígitos numéricos");

            return new NationalIdVO(nationalId.Trim().ToUpper());
        }
    }
}
