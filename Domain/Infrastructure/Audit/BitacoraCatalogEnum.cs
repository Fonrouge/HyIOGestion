using Domain.Exceptions;
using System;

namespace Domain.Infrastructure.Audit
{
    public enum BitacoraCatalogEnum
    {
        //To determine in Factory - BLL > AuditoLogs > BitacoraFactory.cs. Search for fields with '(*)'

        [BitacoraAttributes(
                        DefaultMessage = "Se ha persistido una entidad en BBDD.",//(*) Modificar mensaje concatenando número de intentos, usuario, o número de IP/PC
                        Type = BitacoraTypeEnum.Activity,
                        DefaultSeverity = SeverityEnum.INFO)]
        CreateOnBD,


        [BitacoraAttributes(
                        DefaultMessage = "Se ha actualizado una entidad en BBDD.",//(*) Modificar mensaje concatenando número de intentos, usuario, o número de IP/PC
                        Type = BitacoraTypeEnum.Activity,
                        DefaultSeverity = SeverityEnum.INFO)]

        UpdateOnBD,



        [BitacoraAttributes(
                     DefaultMessage = "Se ha eliminado una entidad en BBDD.",//(*) Modificar mensaje concatenando número de intentos, usuario, o número de IP/PC
                     Type = BitacoraTypeEnum.Activity,
                     DefaultSeverity = SeverityEnum.INFO)]
        DeleteOnBD,



        [BitacoraAttributes(
                   DefaultMessage = "Se ha solicitado listas las entidades de BBDD.",//(*) Modificar mensaje concatenando número de intentos, usuario, o número de IP/PC
                   Type = BitacoraTypeEnum.Activity,
                   DefaultSeverity = SeverityEnum.INFO)]

        GetFromDB,



        [BitacoraAttributes(
                   DefaultMessage = "Se ha solicitado ingresar al sistema .", //(*) Modificar mensaje concatenando número de intentos, usuario, o número de IP/PC
                   Type = BitacoraTypeEnum.Activity,
                   DefaultSeverity = SeverityEnum.INFO)]
        LoginAttempt





    }
}
