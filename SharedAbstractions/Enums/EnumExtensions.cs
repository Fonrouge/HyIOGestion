using SharedAbstractions.Enums.Attributes;
using System;
namespace SharedAbstractions.Enums
{
    public static class EnumExtensions
    {
        public static DocTypeInfoAttribute GetDocInfo(this DocTypes val)
        {
            var field = val.GetType().GetField(val.ToString());
            return (DocTypeInfoAttribute)Attribute.GetCustomAttribute(field, typeof(DocTypeInfoAttribute));
        }

        public static CountryInfoAttribute GetCountriesInfo(this Countries val)
        {
            var field = val.GetType().GetField(val.ToString());
            return (CountryInfoAttribute)Attribute.GetCustomAttribute(field, typeof(CountryInfoAttribute));
        }
    }
}
