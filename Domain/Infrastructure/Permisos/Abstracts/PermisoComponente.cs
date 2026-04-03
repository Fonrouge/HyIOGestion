using Domain.Contracts;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Permisos.Abstracts
{
    public abstract class PermisoComponente : EntityBase
    {
        // Corresponde a 'Nombre' en la DB (ej: "Gestión de Usuarios")
        public string Nombre { get; set; }

        // Corresponde a 'Permiso' en la DB (ej: "USR_CREATE")
        public string PermisoCode { get; set; }

        // Integridad Horizontal
        public DvhVo DVH { get; set; }

        public abstract IList<PermisoComponente> Hijos { get; }

        public abstract void AgregarHijo(PermisoComponente c);
        public abstract void VaciarHijos();

        public override string ToString() => Nombre;
    }
}