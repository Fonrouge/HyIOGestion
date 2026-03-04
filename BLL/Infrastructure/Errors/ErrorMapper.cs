using Domain.Exceptions.Base;

namespace BLL.DTOs.Errors
{
    public static class ErrorMapper
    {        
        public static ErrorLogDTO ToDTO(ErrorLog errorLog)
        {
            return new ErrorLogDTO
            {
                Code = errorLog.Code ?? "ERR-UNKNOWN",
                Message = errorLog.Message ?? "Ocurrió un error inesperado.",
                RecommendedAction = errorLog.RecommendedAction ?? "Contacte al administrador.",
                InformativeMessage = errorLog.InformativeMessage ?? "Sin detalles adicionales.",
                Table = errorLog.Table ?? "N/A",
                Severity = errorLog.Severity,
                //StackTrace information shouldn't be exposed to the UI
            };
        }
    }
}