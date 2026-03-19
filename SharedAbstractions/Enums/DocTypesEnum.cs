using SharedAbstractions.Enums.Attributes;

namespace SharedAbstractions.Enums
{
    public enum DocTypesEnum
    {
        [DocTypeInfo("80", "CUIT")]
        CUIT,

        [DocTypeInfo("96", "DNI")]
        DNI,

        [DocTypeInfo("86", "CUIL")]
        CUIL,

        [DocTypeInfo("94", "Pasaporte")]
        PASAPORTE,

        [DocTypeInfo("87", "CDI")]
        CDI,

        [DocTypeInfo("89", "Libreta de Enrolamiento")]
        LE,

        [DocTypeInfo("90", "Libreta Cívica")]
        LC,

        [DocTypeInfo("91", "Cédula Extranjera")]
        CI_EXTRANJERA,

        [DocTypeInfo("99", "Sin Identificar / Consumidor Final")]
        SIN_IDENTIFICAR
    }
}
