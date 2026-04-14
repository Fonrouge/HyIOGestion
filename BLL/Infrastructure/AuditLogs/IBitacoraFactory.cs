using Domain.Exceptions;
using Domain.Exceptions.Base;
using Domain.Infrastructure.Audit;
using System;

namespace BLL.Infrastructure.AuditLogs
{
    public interface IBitacoraFactory
    {
        Bitacora Create
        (
            BitacoraCatalogEnum entry,
            string user,
            string tableName,
            Guid sessionId,
            Guid correlationId,
            string extraInfo = null,
            Exception ex = null
        );

    }
}
