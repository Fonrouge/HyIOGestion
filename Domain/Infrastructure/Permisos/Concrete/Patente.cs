using Domain.Entities.Permisos.Abstracts;
using System.Collections.Generic;

namespace Domain.Entities.Permisos.Concrete
{
    public class Patente : PermisoComponente
    {        
        public override IList<PermisoComponente> Hijos => new List<PermisoComponente>(); // Una patente no tiene hijos, por lo que devolvemos lista vacía o null
        public override void AgregarHijo(PermisoComponente c) { /* No hace nada */ }
        public override void VaciarHijos() { /* No hace nada */ }
    }
}
