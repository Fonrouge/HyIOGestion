using Domain.Exceptions;
using System;

namespace Domain.Infrastructure.Audit
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class BitacoraAttributes : Attribute
    {       
        public string DefaultMessage { get; set; }
        public BitacoraTypeEnum Type { get; set; }
        public SeverityEnum DefaultSeverity { get; set; }
        public bool Success { get; set; }

        // Constructor para los datos obligatorios (si es que quiero datos obligatorios)
        public BitacoraAttributes() { }
    }

}
