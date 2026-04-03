using SharedAbstractions.Enums.Attributes;
using System;
namespace SharedAbstractions.Enums
{
    public static class EnumExtensions
    {
        public static DocTypeInfoAttribute GetDocInfo(this DocTypesEnum val)
        {
            var field = val.GetType().GetField(val.ToString());
            return (DocTypeInfoAttribute)Attribute.GetCustomAttribute(field, typeof(DocTypeInfoAttribute));
        }

        public static CountryInfoAttribute GetCountriesInfo(this CountriesEnum val)
        {
            var field = val.GetType().GetField(val.ToString());
            return (CountryInfoAttribute)Attribute.GetCustomAttribute(field, typeof(CountryInfoAttribute));
        }

        public static PaymentsMethodsAttribute GetPaymentMethodsInfo(this PaymentMethodsEnum val)
        {
            var field = val.GetType().GetField(val.ToString());
            return (PaymentsMethodsAttribute)Attribute.GetCustomAttribute(field, typeof(PaymentsMethodsAttribute));
        }


    }
}
