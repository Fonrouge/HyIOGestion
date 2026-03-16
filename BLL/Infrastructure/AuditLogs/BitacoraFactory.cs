using BLL.Infrastructure.AuditLogs;
using Domain.Exceptions;
using Domain.Exceptions.Base;
using Domain.Infrastructure.Audit;
using Shared;
using System;
using System.Reflection;

namespace BLL.AuditLogs
{
    public class BitacoraFactory : IBitacoraFactory
    {
        private readonly IApplicationSettings _appSettings;

        public BitacoraFactory(IApplicationSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public Bitacora Create(BitacoraCatalogEnum entry, string user, string tableName, string extraInfo = null, Exception ex = null)
        {
            // 1. Extraemos los metadatos del Enum usando Reflection
            var fieldInfo = entry.GetType().GetField(entry.ToString());
            var config = fieldInfo.GetCustomAttribute<BitacoraAttributes>();

            if (config == null)
            {
                throw new InvalidOperationException($"El evento {entry} no tiene definidos los atributos de Bitacora.");
            }

            // 2. Lógica de construcción del mensaje
            string finalMessage = string.IsNullOrEmpty(extraInfo) ? config.DefaultMessage : $"{config.DefaultMessage} - Detalle: {extraInfo}";

            
            SeverityEnum finalSeverity = (ex != null) ? SeverityEnum.CRITICAL : config.DefaultSeverity;

            return Bitacora.Create
            (
                user: user ?? "System",
                message: finalMessage,
                type: config.Type,
                severity: finalSeverity,
                success: (ex == null),
                tableName: tableName,
                exceptionType: ex?.GetType().Name,
                stackTrace: ex?.StackTrace
            );
        }
    }
}