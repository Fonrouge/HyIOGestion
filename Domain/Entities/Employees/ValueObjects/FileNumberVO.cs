using System;

namespace Domain.Entities.Employees.ValueObjects
{
    public sealed class FileNumberVO : IValueObject
    {
        public string Value { get; }

        private FileNumberVO(string value) => Value = value;

        public static FileNumberVO Create(string fileNumber)
        {
            if (string.IsNullOrWhiteSpace(fileNumber))
                throw new ArgumentException("El número de legajo no puede estar vacío");

            if (fileNumber.Length < 1 || fileNumber.Length > 10)
                throw new ArgumentException("El número de legajo debe tener entre 1 y 10 caracteres");
            

            return new FileNumberVO(fileNumber.Trim().ToUpper());
        }
    }
}