using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.Linq;

// OJO: Idealmente, no deberías referenciar Domain.Entities aquí.
using Domain.Entities.Permisos.Abstracts;

namespace BLL.DTOs
{
    public class UsuarioDTO : IDto
    {
        // 1. Propiedad plana. El Factory/Entidad es quien crea los IDs nuevos, no el DTO.
        public Guid Id { get; set; } = Guid.Empty;

        public string Username { get; set; }
        public string Password { get; set; }
        public string Language { get; set; }
        public string DVH { get; set; }

        // 2. Aquí está tu magia: El objeto rico para la Vista
        public EmployeeDTO EmployeeDTO { get; set; }

        public Guid SessionId { get; set; }

        // 3. (A futuro, podrías cambiar esto a List<string> con los códigos de permisos)
        public List<PermisoComponente> Permisos { get; set; } = new List<PermisoComponente>();

        public override string ToString() => Username;

        // Mantenemos tu lógica si la necesitas urgente para la UI, 
        // pero recuerda que el verdadero "HasPermission" de seguridad ocurre en tu BLL con la Entidad.
        public bool HasPermission(string codigo) => Permisos.Any(p => CheckRecursivo(p, codigo));

        private bool CheckRecursivo(PermisoComponente componente, string codigo)
        {
            if (componente.Permiso == codigo) return true;
            return componente.Hijos.Any(hijo => CheckRecursivo(hijo, codigo));
        }
    }
}