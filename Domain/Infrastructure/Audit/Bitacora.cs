using Domain.Infrastructure.Audit;
using System;

namespace Domain.Exceptions.Base
{
    public class Bitacora //Ex AuditLog
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string           User { get; set; }
        public string           Message { get; set; }
        public string           ExceptionType { get; set; }
        public string           TableName { get; set; }
        public string           StackTrace { get; set; }
        public BitacoraTypeEnum BitacoraType { get; set; }
        public SeverityEnum     Severity { get; set; }
        public bool             Success { get; set; }



    }
}
