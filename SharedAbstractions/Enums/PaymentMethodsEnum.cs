using SharedAbstractions.Enums.Attributes;

public enum PaymentMethodsEnum
{
    [PaymentsMethods("01", "Efectivo")] Efectivo,
    [PaymentsMethods("02", "Tarjeta de Crédito")] Tarjeta_de_Crédito,
    [PaymentsMethods("03", "Tarjeta de Débito")] Tarjeta_de_Débito,
    [PaymentsMethods("04", "Cheque")] Cheque,
    [PaymentsMethods("05", "Mercado Pago")] Transferencia_MercadoPago,
    [PaymentsMethods("06", "Modo")] Transferencia_Modo,
    [PaymentsMethods("07", "Transferencia Bancaria")] Transferencia_Bancaria,
    [PaymentsMethods("08", "Otra Transferencia")] Otra_Transferencia
}