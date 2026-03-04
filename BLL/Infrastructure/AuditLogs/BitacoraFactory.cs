using BLL.Infrastructure.AuditLogs;
using Domain.Exceptions;
using Domain.Exceptions.Base;
using Domain.Infrastructure.Audit;
using Shared;
using System;
using System.Reflection;

namespace BLL.AuditLogs
{
    public class BitacoraFactory: IBitacoraFactory
    {
        private readonly IApplicationSettings _appSettings;

        public BitacoraFactory(IApplicationSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public Bitacora Create(BitacoraCatalogEnum entry, string user, string tableName, string extraInfo = null, Exception ex = null)
        {
            var fieldInfo = entry.GetType().GetField(entry.ToString());
            var config = fieldInfo.GetCustomAttribute<BitacoraAttributes>();

            return new Bitacora
            {
                Timestamp = DateTime.UtcNow,
                User = user ?? "System",
                Message = string.IsNullOrEmpty(extraInfo)
                          ? config.DefaultMessage
                          : $"{config.DefaultMessage} - Detalle: {extraInfo}",

                BitacoraType = config.Type,
                Severity = ex != null ? SeverityEnum.CRITICAL : config.DefaultSeverity,

                TableName = tableName,
                Success = (ex == null),
                ExceptionType = ex?.GetType().Name ?? "N/A",
                StackTrace = ex?.StackTrace ?? "N/A"
            };
        }

    }
}
