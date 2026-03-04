using Domain.Exceptions.Base;
using System;

namespace Domain.Exceptions
{
    public class IntegrityException : BaseException
    {
        public string TableName { get; }
        public string EntityId { get; }
        

        // Constructor para fallos de un registro puntual (DVH)
        public IntegrityException(string tableName, string entityId, string message) : base(message, SeverityEnum.CRITICAL)
        {
            TableName = tableName;
            EntityId = entityId;            
        }

        public override string ToString()
        {
            return $"{base.ToString()} + Table: {TableName} + Entity ID: {EntityId}";
        }
    }
}
