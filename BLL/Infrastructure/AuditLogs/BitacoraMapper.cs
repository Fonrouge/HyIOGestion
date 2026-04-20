using Domain.Exceptions.Base;
using System;
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
                SessionId = entity.SessionId,
                CorrelationId = entity.CorrelationId,
                HostName = entity.HostName,
                Timestamp = entity.Timestamp,
                User = entity.User,
                Message = entity.Message,
                BitacoraType = entity.BitacoraType,
                Severity = entity.Severity,
                Success = entity.Success,
                ExceptionType = entity.ExceptionType,
                TableName = entity.TableName,
                StackTrace = entity.StackTrace,
                DVH = (string)(entity.DVH?.Value)
            };
        }

        public static Bitacora ToEntity(BitacoraDTO dto)
        {
            if (dto == null) return null;

            // Si el Id es vacío, asumimos que es un registro nuevo para persistir
            if (dto.Id == Guid.Empty)
            {
                return Bitacora.Create
                (
                    user: dto.User,
                    message: dto.Message,
                    sessionId: dto.SessionId,
                    correlationId: dto.CorrelationId,
                    type: dto.BitacoraType,
                    severity: dto.Severity,
                    success: dto.Success,
                    hostName: dto.HostName,
                    tableName: dto.TableName,
                    exceptionType: dto.ExceptionType,
                    stackTrace: dto.StackTrace
                );
            }

            // Si tiene Id, lo reconstituimos con los datos exactos de la BBDD
            return Bitacora.Reconstitute
            (
                id: dto.Id,
                timestamp: dto.Timestamp,
                user: dto.User,
                message: dto.Message,
                sessionId: dto.SessionId,
                correlationId: dto.CorrelationId,
                type: (int)dto.BitacoraType,
                severity: (int)dto.Severity,
                hostName: dto.HostName,
                success: dto.Success,
                tableName: dto.TableName,
                exceptionType: dto.ExceptionType,
                stackTrace: dto.StackTrace,
                dvh: dto.DVH
            );
        }

        public static List<BitacoraDTO> ToListDto(IEnumerable<Bitacora> entities)
            => entities?.Select(ToDto).ToList() ?? new List<BitacoraDTO>();
    }
}