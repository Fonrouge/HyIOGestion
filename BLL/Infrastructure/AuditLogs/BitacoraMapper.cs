using Domain.Exceptions.Base;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Infrastructure.AuditLogs
{
    public static class BitacoraMapper
    {
        public static BitacoraDTO ToDto(Bitacora entity)
        {
            if (entity == null) return null;
            return new BitacoraDTO
            {
                Id = entity.Id,
                Timestamp = entity.Timestamp,
                User = entity.User,
                Message = entity.Message,
                BitacoraType = entity.BitacoraType,
                Severity = entity.Severity,
                Success = entity.Success,
                ExceptionType = entity.ExceptionType,
                TableName = entity.TableName,
                StackTrace = entity.StackTrace,
                DVH = entity.DVH?.Value
            };
        }

        public static Bitacora ToEntity(BitacoraDTO dto)
        {
            if (dto == null) return null;

            if (dto.Id == 0)
            {
                return Bitacora.Create
                (
                    dto.User,
                    dto.Message,
                    dto.BitacoraType,
                    dto.Severity,
                    dto.Success,
                    dto.TableName,
                    dto.ExceptionType,
                    dto.StackTrace
                );
            }

            return Bitacora.Reconstitute
            (
                dto.Id,
                dto.Timestamp,
                dto.User,
                dto.Message,
                (int)dto.BitacoraType,
                (int)dto.Severity,
                dto.Success,
                dto.TableName,
                dto.ExceptionType,
                dto.StackTrace,
                dto.DVH
            );
        }

        public static List<BitacoraDTO> ToListDto(IEnumerable<Bitacora> entities) => entities?.Select(ToDto).ToList() ?? new List<BitacoraDTO>();
    }
}