using BLL.Infrastructure.AuditLogs;
using Domain.Exceptions;
using Domain.Exceptions.Base;
using Domain.Infrastructure.Audit;
using Shared.Services;
using System;
using System.Reflection;

namespace BLL.AuditLogs
{
    public class BitacoraFactory : IBitacoraFactory
    {

        public Bitacora Create
        (
            BitacoraCatalogEnum entry,
            string user,
            string tableName,
            Guid sessionId,
            Guid correlationId,
            string extraInfo = null,
            Exception ex = null
        )
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

                
            //3. Cualquier exxcepcipón se cataloga como error crítico
            SeverityEnum finalSeverity = (ex != null) ? SeverityEnum.CRITICAL : config.DefaultSeverity;


            //4. Finalmente se crea la bitácora usando la factory estática del rich domain
            var newLog = Bitacora.Create
            (
                user: user ?? "System",
                message: finalMessage,
                sessionId: sessionId,
                correlationId: correlationId,
                type: config.Type,
                severity: config.DefaultSeverity,
                success: (ex == null),
                hostName: Environment.MachineName,
                tableName: tableName,
                exceptionType: ex?.GetType().Name,
                stackTrace: ex?.StackTrace
            );

            newLog.UpdateDVH(IntegrityService.GetIntegrityHash(newLog.GetDvhSerialization()));

            return newLog;
        }
    }
}