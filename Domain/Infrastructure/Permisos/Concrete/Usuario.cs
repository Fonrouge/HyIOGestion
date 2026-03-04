using Domain.BaseContracts;
using Domain.Entities.Permisos.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities.Permisos.Concrete
{
    public class Usuario : EntityBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Language { get; set; }
        public string DVH { get; set; }
        public Guid EmployeeId { get; set; }
        

        public List<PermisoComponente> Permisos { get; set; } = new List<PermisoComponente>();

        public override string ToString() => Username;

        public bool HasPermission(string codigo) => Permisos.Any(p => CheckRecursivo(p, codigo));

        private bool CheckRecursivo(PermisoComponente componente, string codigo)
        {
            if (componente.Permiso == codigo) return true;
            return componente.Hijos.Any(hijo => CheckRecursivo(hijo, codigo));
        }
    }
}
