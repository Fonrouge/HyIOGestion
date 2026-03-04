using Domain.Exceptions;
using System;

namespace BLL.Infrastructure.AuditLogs
{
    public class BitacoraDTO
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string User { get; set; }
        public string Message { get; set; }
        public SeverityEnum Severity { get; set; }
        public string ExceptionType { get; set; }
        public string TableName { get; set; }
        public string StackTrace { get; set; }

    }
}
