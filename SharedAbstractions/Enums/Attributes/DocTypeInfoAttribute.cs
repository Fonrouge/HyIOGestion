using System;

namespace SharedAbstractions.Enums.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class DocTypeInfoAttribute : Attribute
    {
        public string Id { get; }
        public string Description { get; }

        public DocTypeInfoAttribute(string id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}