using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions; 

namespace BLL.DTOs
{
    public class OperationResult<T>
    {
        public T Value { get; set; }
        public bool Success => Errors == null || !Errors.Any();
        public List<ErrorLogDTO> Errors { get; set; } = new List<ErrorLogDTO>();
        public string Metadata { get; set; }
        public Guid? SessionID { get; set; }

        public static OperationResult<T> Ok(T value, Guid? sessionId = null)
        {
            return new OperationResult<T>
            {
                Value = value,
                SessionID = sessionId
            };
        }

        public static OperationResult<T> Failure(string message, Guid? sessionId = null)
        {
            var result = new OperationResult<T> { SessionID = sessionId };

            result.Errors.Add(new ErrorLogDTO
            {
                Message = message,
                Severity = SeverityEnum.WARNING, // Default para fallos controlados
                Timestamp = DateTime.UtcNow,
                LogId = Guid.NewGuid()
            });
            return result;
        }


        public static OperationResult<T> FromException(Exception ex, Guid? sessionId = null)
        {
            return new OperationResult<T>
            {
                Value = default,
                SessionID = sessionId,
                Metadata = "Excepción no controlada detectada.",
                Errors = new List<ErrorLogDTO>
                {
                    new ErrorLogDTO
                    {
                        LogId = Guid.NewGuid(),
                        Timestamp = DateTime.UtcNow,
                        Message = "Ocurrió un error interno en el servidor. Por favor reinicie la app. o contacte a soporte",
                        InformativeMessage = ex.Message,
                        StackTrace = ex.StackTrace,
                        Severity = SeverityEnum.CRITICAL
                    }
                }
            };
        }
    }
}