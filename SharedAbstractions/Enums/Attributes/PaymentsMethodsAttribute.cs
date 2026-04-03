using System;

namespace SharedAbstractions.Enums.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class PaymentsMethodsAttribute : Attribute
    {
        public string Id { get; }
        public string Description { get; }

        public PaymentsMethodsAttribute(string id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}

