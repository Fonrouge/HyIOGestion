using Domain.Entities.Permisos.Abstracts;
using System.Collections.Generic;

namespace Domain.Entities.Permisos.Concrete
{
    public class Familia : PermisoComponente
    {
        private IList<PermisoComponente> _hijos;

        public Familia()
        {
            _hijos = new List<PermisoComponente>();
        }

        public override IList<PermisoComponente> Hijos => _hijos;

        public override void AgregarHijo(PermisoComponente c) => _hijos.Add(c);

        public override void VaciarHijos() => _hijos.Clear();
    }
}
