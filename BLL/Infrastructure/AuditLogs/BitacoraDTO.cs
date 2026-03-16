using Domain.Exceptions;
using Domain.Infrastructure.Audit;
using System;

namespace BLL.Infrastructure.AuditLogs
{
    public class BitacoraDTO
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
        public BitacoraTypeEnum BitacoraType { get; set; }
        public SeverityEnum Severity { get; set; }
        public bool Success { get; set; }
        public string ExceptionType { get; set; }
        public string TableName { get; set; }
        public string StackTrace { get; set; }

        // El DVH suele ser transparente para la UI, pero lo mantenemos para validaciones
        public string DVH { get; set; }
    }
}