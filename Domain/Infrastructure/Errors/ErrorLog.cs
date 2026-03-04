using System;

namespace Domain.Exceptions.Base
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ErrorLog : Attribute
    {
     public Guid Id { get; set; } = Guid.NewGuid();
        public string Code { get; set; }
        public string Message { get; set; }
        public string RecommendedAction { get; set; }
        public string InformativeMessage { get; set; }
        public string Table { get; set; }
        public SeverityEnum Severity { get; set; }
        public string StackTrace { get; set; }
    }
}