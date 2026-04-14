using Domain.Contracts;
using Domain.Entities;
using Domain.Infrastructure.Audit;
using System;

namespace Domain.Exceptions.Base
{
    public class Bitacora: IIntegrityCheckable
    {
        // En la DB es INT (PK)
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid SessionId { get; private set; } 
        public Guid CorrelationId { get; private set; }
        public string HostName { get; private set; }
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
            Guid sessionId,
            Guid correlationId,
            BitacoraTypeEnum type,
            SeverityEnum severity,
            bool success,
            string hostName = null,
            string tableName = null,
            string exceptionType = null,
            string stackTrace = null
            //string dvh > Una entidad recién creada no puede tener DVH calculado hasta ingresar en BBDD.
        )
        {
            return new Bitacora
            {
                Timestamp = DateTime.UtcNow,
                User = user,
                Message = message,
                SessionId = sessionId,
                CorrelationId = correlationId, 
                HostName = hostName,       
                BitacoraType = type,
                Severity = severity,
                Success = success,
                TableName = tableName,
                ExceptionType = exceptionType,
                StackTrace = stackTrace
            };
        }

        /// <summary>
        /// Reconstituye un registro desde la DB
        /// </summary>
        public static Bitacora Reconstitute
        (
            Guid id,
            DateTime timestamp,
            string user,
            string message,
            Guid sessionId,
            Guid correlationId,
            int type,
            int severity,
            string hostName,
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
                HostName = hostName,
                TableName = tableName,
                ExceptionType = exceptionType,
                StackTrace = stackTrace,
                DVH =  DvhVo.Create(dvh)
            };
        }


        /// <summary>
        /// Genera la cadena de serialización para el cálculo del Dígito Verificador Horizontal.
        /// Protege la integridad de los datos filiatorios y de contacto del cliente.
        /// </summary>
        public string GetDvhSerialization()
        {
            // Mantenemos la consistencia con cultura invariante para el ID (Guid)
            var culture = System.Globalization.CultureInfo.InvariantCulture;

            return string.Join("|",
                Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff", culture),
                User ?? string.Empty,
                Message ?? string.Empty,                
                ((int)BitacoraType).ToString(culture),
                ((int)Severity).ToString(culture)
            );
        }


        public void UpdateDVH(string dvh) => this.DVH = DvhVo.Create(dvh);

    }
}