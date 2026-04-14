using Domain.Exceptions;
using Domain.Infrastructure.Audit;
using System;

namespace BLL.Infrastructure.AuditLogs
{
    public class BitacoraDTO
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public Guid CorrelationId { get; set; }
        public string HostName { get; set; }
        public DateTime Timestamp { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
        public BitacoraTypeEnum BitacoraType { get; set; }
        public SeverityEnum Severity { get; set; }
        public bool Success { get; set; }
        public string ExceptionType { get; set; }
        public string TableName { get; set; }
        public string StackTrace { get; set; }

        /// <summary>
        /// Valor del Dígito Verificador Horizontal en formato string para transporte o validación.
        /// </summary>
        public string DVH { get; set; }
    }
}