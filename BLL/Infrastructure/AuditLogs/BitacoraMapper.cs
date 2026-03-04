using Domain.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Infrastructure.AuditLogs
{
    public class BitacoraMapper : IBitacoraMapper
    {

        public BitacoraDTO ToEntity(Bitacora entity)
        {
            if (entity == null) throw new ArgumentNullException($"Entity {nameof(entity)} cannot be null");

            return new BitacoraDTO()
            {
                Timestamp = entity.Timestamp,
                User = entity.User,
                Message = entity.Message,
                Severity = entity.Severity,
                ExceptionType = entity.ExceptionType,
                TableName = entity.TableName,
                StackTrace = entity.StackTrace
            };
        }

        public Bitacora ToEntity(BitacoraDTO dto)
        {
            if (dto == null) throw new ArgumentNullException($"DTO {nameof(dto)} cannot be null");

            return new Bitacora()
            {
                Timestamp = dto.Timestamp,
                User = dto.User,
                Message = dto.Message,
                Severity = dto.Severity,
                ExceptionType = dto.ExceptionType,
                TableName = dto.TableName,
                StackTrace = dto.StackTrace
            };

        }

    }
}
