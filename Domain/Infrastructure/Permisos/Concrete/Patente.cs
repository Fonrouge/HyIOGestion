using Domain.Entities.Permisos.Abstracts;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Permisos.Concrete
{
    public class Patente : PermisoComponente
    {
        // Una patente nunca tiene hijos por definición de arquitectura
        public override IList<PermisoComponente> Hijos => new List<PermisoComponente>();

        public override void AgregarHijo(PermisoComponente c)
        {
            throw new InvalidOperationException("No se pueden agregar hijos a una Patente individual.");
        }

        public override void VaciarHijos() { /* No hace nada, ya está vacía */ }
    }
}