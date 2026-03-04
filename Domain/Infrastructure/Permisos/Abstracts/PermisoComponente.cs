using System;
using System.Collections.Generic;

namespace Domain.Entities.Permisos.Abstracts
{
    public abstract class PermisoComponente
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Permiso { get; set; } //(ej: "USR_CREATE")
        public abstract IList<PermisoComponente> Hijos { get; }
        
        
        public abstract void AgregarHijo(PermisoComponente c);
        public abstract void VaciarHijos();
    }
}
