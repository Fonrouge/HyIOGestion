using Domain.Exceptions;
using Domain.Exceptions.Base;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace BLL.Infrastructure.Errors
{
    public class ErrorsFactory : IErrorsFactory
    {
        // CACHÉ: Guarda los atributos ya leídos para no usar Reflection cada vez que ocurre un error.
        // ConcurrentDictionary es Thread-Safe, ideal para aplicaciones web o concurrentes.
        private static readonly ConcurrentDictionary<ErrorCatalogEnum, ErrorDescriptorAttribute> _attributesCache = new ConcurrentDictionary<ErrorCatalogEnum, ErrorDescriptorAttribute>();

        public ErrorLog Create(ErrorCatalogEnum error, string table = null)
        {
            // 1. Buscamos en la caché primero. Si no existe, usamos Reflection y lo guardamos.
            var metaAttr = _attributesCache.GetOrAdd(error, GetAttributeFromEnum);

            // 2. Construimos la entidad
            return new ErrorLog
            {
                Code = metaAttr?.Code ?? "UNKNOWN_ERR",
                Message = metaAttr?.Message ?? $"Error no catalogado: {error}",
                RecommendedAction = metaAttr?.RecommendedAction ?? "Contacte a soporte.",
                InformativeMessage = metaAttr?.InformativeMessage ?? "Sin detalles técnicos.",
                Table = table ?? metaAttr?.Table ?? "System",
                Severity = metaAttr?.Severity ?? SeverityEnum.NOTDETERMINED
            };
        }

        public ErrorLog CreateFromException(Exception ex)
        {
            // Extraemos el tipo de error más específico disponible
            var errorType = ex.InnerException?.GetType().Name ?? ex.GetType().Name;

            return new ErrorLog
            {
                Code = $"EXC-{errorType.ToUpper()}",
                Message = "Se produjo un error técnico inesperado en el servidor.",
                RecommendedAction = "Por favor, intente la operación más tarde o contacte a soporte.",
                InformativeMessage = $"Mensaje: {ex.Message} | Inner: {(ex.InnerException?.Message ?? "N/A")}",
                Table = "System",
                Severity = SeverityEnum.CRITICAL,
                StackTrace = ex.StackTrace // Se queda en la entidad, pero el Mapper lo ignora para la UI
            };
        }

        // Método auxiliar privado que hace el trabajo pesado de Reflection
        private ErrorDescriptorAttribute GetAttributeFromEnum(ErrorCatalogEnum error)
        {
            Type type = error.GetType();
            FieldInfo fieldInfo = type.GetField(error.ToString());
            return fieldInfo?.GetCustomAttribute<ErrorDescriptorAttribute>();
        }
    }
}