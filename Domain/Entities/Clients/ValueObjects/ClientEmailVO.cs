using Domain.Entities.Employees.ValueObjects;
using System;

namespace Domain.Entities.Clients.ValueObjects
{
    public sealed class ClientEmailVO: IValueObject
    {
        public string Value { get; }

        private ClientEmailVO(string value) => Value = value;

        public static ClientEmailVO Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío");

            if (email.Length > 100 || !email.Contains("@"))
                throw new ArgumentException("El email no tiene un formato válido");

            return new ClientEmailVO(email.Trim().ToLowerInvariant());
        }
    }
}
