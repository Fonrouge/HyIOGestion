using Domain.Exceptions;
using System;

namespace BLL.DTOs
{
    public class ErrorLogDTO
    {
        // Info para el Usuario (viene del catálogo)
        public string Code { get; set; }
        public string Message { get; set; }
        public string RecommendedAction { get; set; }
        public SeverityEnum Severity { get; set; }


        // Info Técnica (para el Log)
        public string InformativeMessage { get; set; }
        public string Table { get; set; }
        public string StackTrace { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public Guid LogId { get; set; } = Guid.NewGuid();

        public override string ToString() => $"[{Code}] {Message} ({Severity})";
    }
}