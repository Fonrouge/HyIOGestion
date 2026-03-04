using Domain.Exceptions.Base;

namespace BLL.Infrastructure.AuditLogs
{
    public interface IBitacoraMapper
    {
        BitacoraDTO ToEntity(Bitacora entity);
        Bitacora ToEntity(BitacoraDTO dto);
    }
}
