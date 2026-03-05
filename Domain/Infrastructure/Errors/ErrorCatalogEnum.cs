using Domain.Exceptions.Base;
using System;

namespace Domain.Exceptions
{
    public enum ErrorCatalogEnum
    {
        // --- INTEGRIDAD DE DATOS (CRITICAL) ---
        [ErrorDescriptor(Code = "DATA_001",
                         Message = "CORRUPCIÓN DE FILA (DVH).",
                         RecommendedAction = "El registro ha sido alterado externamente. Realice un 'Restore' o contacte a soporte.",
                         InformativeMessage = "La validación del Dígito Verificador Horizontal ha fallado para el registro solicitado.",
                         Table = "",
                         Severity = SeverityEnum.CRITICAL)]
        InconsistentRowIntegrity,

        [ErrorDescriptor(Code = "DATA_002",
                         Message = "CORRUPCIÓN DE TABLA (DVV).",
                         RecommendedAction = "Se detectaron registros faltantes o agregados ilegalmente. Restaure la base de datos.",
                         InformativeMessage = "La validación del Dígito Verificador Vertical no coincide con el conteo de hashes de la tabla.",
                         Table = "",
                         Severity = SeverityEnum.CRITICAL)]
        InconsistentTableIntegrity,

        // --- SEGURIDAD Y ACCESO (WARNING / INFO) ---
        [ErrorDescriptor(Code = "AUTH_001",
                         Message = "Credenciales inválidas.",
                         RecommendedAction = "Verifique su usuario y contraseña e intente nuevamente.",
                         InformativeMessage = "Intento de login fallido: usuario inexistente o password incorrecto.",
                         Table = "Users",
                         Severity = SeverityEnum.WARNING)]
        InvalidCredentials,

        [ErrorDescriptor(Code = "AUTH_002",
                         Message = "Acceso denegado.",
                         RecommendedAction = "Solicite al administrador que le asigne los permisos (patentes) necesarios.",
                         InformativeMessage = "El usuario no posee los permisos requeridos para ejecutar esta acción.",
                         Table = "",
                         Severity = SeverityEnum.WARNING)]
        InsufficientPermissions,

        [ErrorDescriptor(Code = "AUTH_003",
                         Message = "Sesión expirada.",
                         RecommendedAction = "Cierre el programa y vuelva a iniciar sesión.",
                         InformativeMessage = "El token de sesión actual ya no es válido o ha caducado.",
                         Table = "",
                         Severity = SeverityEnum.INFO)]
        SessionExpired,

        // --- LÓGICA DE NEGOCIO (INFO) ---
        [ErrorDescriptor(Code = "BUSI_001",
                         Message = "Entidad inactiva.",
                         RecommendedAction = "Cambie el estado del registro a 'Activo' desde el panel de administración.",
                         InformativeMessage = "La operación fue rechazada porque el flag 'IsActive' es false.",
                         Table = "",
                         Severity = SeverityEnum.INFO)]
        EntityInactive,

        [ErrorDescriptor(Code = "BUSI_002",
                         Message = "Registro duplicado.",
                         RecommendedAction = "Verifique que el dato (DNI, CUIT o Email) no haya sido cargado previamente.",
                         InformativeMessage = "Violación de restricción de unicidad en la base de datos.",
                         Table = "",
                         Severity = SeverityEnum.INFO)]
        DuplicateEntry,

        [ErrorDescriptor(Code = "BUSI_003",
                         Message = "No se puede eliminar el registro.",
                         RecommendedAction = "El registro tiene datos asociados (ej. ventas o turnos). Desactívelo en su lugar.",
                         InformativeMessage = "Falla de restricción de clave foránea (Foreign Key Constraint).",
                         Table = "",
                         Severity = SeverityEnum.WARNING)]
        DeleteRestriction,

        // --- SISTEMA E INFRAESTRUCTURA (CRITICAL / WARNING) ---
        [ErrorDescriptor(Code = "SYST_001",
                         Message = "Base de datos no disponible.",
                         RecommendedAction = "Verifique su conexión a la red o el estado del servidor SQL.",
                         InformativeMessage = "No se pudo establecer conexión con el motor de base de datos.",
                         Table = "",
                         Severity = SeverityEnum.CRITICAL)]
        DatabaseUnavailable,

        [ErrorDescriptor(Code = "SYST_002",
                         Message = "Error interno del sistema.",
                         RecommendedAction = "Reinicie la aplicación. Si el problema persiste, envíe el log de errores a soporte.",
                         InformativeMessage = "Error no controlado (Exception Catch-all).",
                         Table = "",
                         Severity = SeverityEnum.CRITICAL)]
        InternalError,

        [ErrorDescriptor(Code = "SYST_003",
                         Message = "Error al cargar datos.",
                         RecommendedAction = "Intente realizar la búsqueda nuevamente.",
                         InformativeMessage = "Falla en el mapeo de la entidad o timeout en la consulta.",
                         Table = "",
                         Severity = SeverityEnum.WARNING)]
        DataLoadError,
        
        [ErrorDescriptor(Code = "BUSI_004",
                         Message = "Registro no encontrado.",
                         RecommendedAction = "Verifique si el identificador es correcto o si el registro fue eliminado por otro usuario.",
                         InformativeMessage = "La consulta no devolvió ningún resultado para los criterios especificados.",
                         Table = "",
                         Severity = SeverityEnum.INFO)]
        NotFound,
    }
}