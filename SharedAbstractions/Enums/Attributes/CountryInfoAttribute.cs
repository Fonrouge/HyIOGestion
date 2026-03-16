using System;

namespace SharedAbstractions.Enums.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class CountryInfoAttribute:Attribute
    {
        public string Id { get; }
        public string Description { get; }

        public CountryInfoAttribute(string id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}


