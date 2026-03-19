using SharedAbstractions.Enums.Attributes;

namespace SharedAbstractions.Enums
{
    public enum PaymentMethodsEnum
    {
      [CountryInfo("01", "Efectivo")]   Efectivo,
      [CountryInfo("02", "Tarjeta_de_Crédito")]   Tarjeta_de_Crédito,
      [CountryInfo("03", "Tarjeta_de_Débito")]   Tarjeta_de_Débito,
      [CountryInfo("04", "Cheque")]   Cheque,
      [CountryInfo("05", "Transferencia_MercadoPago")]   Transferencia_MercadoPago,
      [CountryInfo("06", "Transferencia_Modo")]   Transferencia_Modo,
      [CountryInfo("07", "Transferencia_Bancaria")]   Transferencia_Bancaria,
      [CountryInfo("08", "Otra_Transferencia")] Otra_Transferencia
    }
}
