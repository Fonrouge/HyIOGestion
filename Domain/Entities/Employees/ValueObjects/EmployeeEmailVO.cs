using System;

namespace Domain.Entities.Employees.ValueObjects
{
    public sealed class EmployeeEmailVO : IValueObject
    {
        public string Value { get; }

        private EmployeeEmailVO(string value) => Value = value;

        public static EmployeeEmailVO Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío");

            if (email.Length > 100 || !email.Contains("@"))
                throw new ArgumentException("El email no tiene un formato válido");

            return new EmployeeEmailVO(email.Trim().ToUpper());
        }
    }
}
