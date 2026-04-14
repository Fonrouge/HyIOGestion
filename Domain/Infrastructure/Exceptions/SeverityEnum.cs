namespace Domain.Exceptions
{
    public enum SeverityEnum
    {
        // Información de auditoría (ej: "Usuario inició sesión").
        INFO = 0,

        // El sistema detectó algo inusual pero puede seguir operando 
        // (ej: "Intento de inicio de sesión fallido").
        WARNING = 1,

        // Una operación falló pero el sistema es estable 
        // (ej: "No se pudo emitir la factura").
        ERROR = 2,

        // El nivel más alto. Implica que la seguridad o la integridad de los datos 
        // se vio comprometida (ej: "Falla de integridad vertical DVV en tabla User").
        CRITICAL = 3,

        // Para errores donde no se puede determinar niveles de criticidad
        NOTDETERMINED = 4
    }
}