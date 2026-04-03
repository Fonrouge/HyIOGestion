using Domain.Contracts;
using Domain.Entities;
using Domain.Infrastructure.Audit;
using System;

namespace Domain.Exceptions.Base
{
    public class Bitacora
    {
        // En la DB es INT (PK)
        public int Id { get; private set; }
        public DateTime Timestamp { get; private set; } = DateTime.UtcNow;
        public string User { get; private set; }
        public string Message { get; private set; }
        public string ExceptionType { get; private set; }
        public string TableName { get; private set; }
        public string StackTrace { get; private set; }
        public BitacoraTypeEnum BitacoraType { get; private set; }
        public SeverityEnum Severity { get; private set; }
        public bool Success { get; private set; }

        // Integridad Horizontal
        public DvhVo DVH { get; private set; }

        private Bitacora() { }

        /// <summary>
        /// Crea un nuevo registro de bitácora (para ser persistido)
        /// </summary>
        public static Bitacora Create
        (
            string user,
            string message,
            BitacoraTypeEnum type,
            SeverityEnum severity,
            bool success,
            string tableName = null,
            string exceptionType = null,
            string stackTrace = null
        )
        {
            return new Bitacora
            {
                Timestamp = DateTime.UtcNow,
                User = user,
                Message = message,
                BitacoraType = type,
                Severity = severity,
                Success = success,
                TableName = tableName,
                ExceptionType = exceptionType,
                StackTrace = stackTrace,
                DVH = DvhVo.Create("")
            };
        }

        /// <summary>
        /// Reconstituye un registro desde la DB
        /// </summary>
        public static Bitacora Reconstitute
        (
            int id,
            DateTime timestamp,
            string user,
            string message,
            int type,
            int severity,
            bool success,
            string tableName,
            string exceptionType,
            string stackTrace,
            string dvh
        )
        {
            return new Bitacora
            {
                Id = id,
                Timestamp = timestamp,
                User = user,
                Message = message,
                BitacoraType = (BitacoraTypeEnum)type,
                Severity = (SeverityEnum)severity,
                Success = success,
                TableName = tableName,
                ExceptionType = exceptionType,
                StackTrace = stackTrace,
                DVH = !string.IsNullOrEmpty(dvh) ? DvhVo.Create(dvh) : null
            };
        }
    }
}