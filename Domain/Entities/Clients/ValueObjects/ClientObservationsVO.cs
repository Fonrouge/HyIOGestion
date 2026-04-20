using System;

namespace Domain.Entities.Clients.ValueObjects
{
    public sealed class ClientObservationsVO : IValueObject
    {
        public object Value { get; }

        private ClientObservationsVO(string value) => Value = value;

        public static ClientObservationsVO Create(string observations)
        {
            var cleaned = observations?.Trim() ?? string.Empty;

            if (cleaned.Length > 500)
                throw new ArgumentException("Las observaciones no pueden superar los 500 caracteres.");

            return new ClientObservationsVO(cleaned.Trim().ToUpper());
        }
    }
}