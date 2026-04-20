using Domain.Exceptions;

namespace Domain.Infrastructure.Audit
{
    public enum BitacoraCatalogEnum
    {

        //========================================================
        //                      CREATE
        //========================================================
        [BitacoraAttributes(
            DefaultMessage = "Se ha persistido una entidad en BBDD.",//(*) Modificar mensaje concatenando número de intentos, usuario, o número de IP/PC
            Type = BitacoraTypeEnum.Activity,
            DefaultSeverity = SeverityEnum.WARNING)]

        CreateOnBD,


        //========================================================
        //                      UPDATE
        //========================================================
        [BitacoraAttributes(
                        DefaultMessage = "Se ha actualizado una entidad en BBDD.",
                        Type = BitacoraTypeEnum.Activity,
                        DefaultSeverity = SeverityEnum.WARNING)]

        UpdateOnBD,


        //========================================================
        //                      HARD DELETE
        //========================================================
        [BitacoraAttributes(
                     DefaultMessage = "Se ha eliminado una entidad en BBDD de forma permanente.",
                     Type = BitacoraTypeEnum.Activity,
                     DefaultSeverity = SeverityEnum.WARNING)]

        HardDeleteOnBD,


        //========================================================
        //                      SOFT-DELETE
        //========================================================
        [BitacoraAttributes(
                     DefaultMessage = "Se ha eliminado una entidad en BBDD de forma reversible.",
                     Type = BitacoraTypeEnum.Activity,
                     DefaultSeverity = SeverityEnum.WARNING)]

        SoftDeleteOnBD,


        //========================================================
        //                      GET ALL
        //========================================================
        [BitacoraAttributes(
                     DefaultMessage = "Se ha solicitado listar las entidades de BBDD.",
                     Type = BitacoraTypeEnum.Activity,
                     DefaultSeverity = SeverityEnum.INFO)]

        GetAllFromDB,



        //========================================================
        //                      GET BY ID
        //========================================================


        [BitacoraAttributes(
                    DefaultMessage = "Se ha consultado una entidad por ID en BBDD.",
                    Type = BitacoraTypeEnum.Activity,
                    DefaultSeverity = SeverityEnum.INFO)]
        GetByIdFromDB,



        //========================================================
        //                      LOG-IN
        //========================================================
        [BitacoraAttributes(
                     DefaultMessage = "Intento de inicio de sesión detectado.",
                     Type = BitacoraTypeEnum.Security,
                     DefaultSeverity = SeverityEnum.INFO)]
        LoginAttempt,



        //========================================================
        //                      LOG-OUT
        //========================================================
        [BitacoraAttributes(
                     DefaultMessage = "Usuario cerró sesión correctamente.",
                     Type = BitacoraTypeEnum.Security,
                     DefaultSeverity = SeverityEnum.INFO)]
        Logout,


        //========================================================
        //                      SEGURIDAD (AUTH)
        //========================================================
        [BitacoraAttributes(
                    DefaultMessage = "Inicio de sesión exitoso.",
                    Type = BitacoraTypeEnum.Security,
                    DefaultSeverity = SeverityEnum.INFO)]
        LoginSuccess,

        [BitacoraAttributes(
                    DefaultMessage = "Fallo en el intento de inicio de sesión.",
                    Type = BitacoraTypeEnum.Security,
                    DefaultSeverity = SeverityEnum.WARNING)]
        LoginFailed,

        [BitacoraAttributes(
                    DefaultMessage = "Se ha bloqueado una cuenta por intentos fallidos.",
                    Type = BitacoraTypeEnum.Security,
                    DefaultSeverity = SeverityEnum.CRITICAL)]
        AccountBlocked,

        [BitacoraAttributes(
                    DefaultMessage = "Cambio de contraseña realizado por el usuario.",
                    Type = BitacoraTypeEnum.Security,
                    DefaultSeverity = SeverityEnum.INFO)]
        PasswordChange,


        //========================================================
        //                INTEGRIDAD DE DATOS (DVH/DVV)
        //========================================================
        [BitacoraAttributes(
                    DefaultMessage = "Falla de integridad detectada (Dígito Verificador Horizontal).",
                    Type = BitacoraTypeEnum.Data,
                    DefaultSeverity = SeverityEnum.CRITICAL)]
        DVH_Failure,

        [BitacoraAttributes(
                    DefaultMessage = "Falla de integridad detectada (Dígito Verificador Vertical).",
                    Type = BitacoraTypeEnum.Data,
                    DefaultSeverity = SeverityEnum.CRITICAL)]
        DVV_Failure,

        [BitacoraAttributes(
                    DefaultMessage = "Recalculo de dígitos verificadores completado exitosamente.",
                    Type = BitacoraTypeEnum.Data,
                    DefaultSeverity = SeverityEnum.INFO)]
        DV_Recalculate,



        //========================================================
        //                      SISTEMA / TÉCNICO
        //========================================================
        [BitacoraAttributes(
                    DefaultMessage = "Error inesperado en el servidor.",
                    Type = BitacoraTypeEnum.Technical,
                    DefaultSeverity = SeverityEnum.ERROR)]
        UnhandledException,


        [BitacoraAttributes(
                    DefaultMessage = "Acceso denegado a recurso restringido (Permisos insuficientes).",
                    Type = BitacoraTypeEnum.Security,
                    DefaultSeverity = SeverityEnum.WARNING)]
        AccessDenied,



        //========================================================
        //                     ADMINISTRACIÓN
        //========================================================
        [BitacoraAttributes(
                     DefaultMessage = "Se ha realizado un Backup de la base de datos.",
                     Type = BitacoraTypeEnum.Data,
                     DefaultSeverity = SeverityEnum.INFO)]
        BackupCreated,


        [BitacoraAttributes(
                    DefaultMessage = "Restauración de base de datos ejecutada.",
                    Type = BitacoraTypeEnum.Data,
                    DefaultSeverity = SeverityEnum.CRITICAL)]
        RestoreExecuted



    }
}

/*
 
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
 

namespace Domain.Infrastructure.Audit
{
    public enum BitacoraTypeEnum
    {
        Technical, //Excepciones

        
        Security, //Seguridad


        Activity, //Trazabilidad


        Data //Integridad
    }
}




 
 */


