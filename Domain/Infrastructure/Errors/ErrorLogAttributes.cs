using System;

namespace Domain.Exceptions.Base
{
    // El atributo debe ser una clase de primer nivel
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ErrorDescriptorAttribute : Attribute
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string RecommendedAction { get; set; }
        public string InformativeMessage { get; set; }
        public string Table { get; set; }
        public SeverityEnum Severity { get; set; }

        public ErrorDescriptorAttribute() { }
    }
}