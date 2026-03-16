using Domain.BaseContracts;
using Domain.Entities.Permisos.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities.Permisos.Concrete
{
    public class Usuario : EntityBase, ISoftDeletable
    {
        public string Username { get; private set; }
        public string Password { get; private set; } // Hash
        public string Language { get; private set; }
        public DvhVo DVH { get; private set; }
        public Guid EmployeeId { get; private set; }
        public bool IsDeleted { get; private set; }
        public bool IsActive { get; private set; }

        public List<PermisoComponente> Permisos { get; private set; } = new List<PermisoComponente>();

        private Usuario() { }

        // Factory para usuario NUEVO
        public static Usuario Create(string username, string password, string language, Guid employeeId)
        {
            return new Usuario
            {
                Username = username,
                Password = password, // Aquí ya debería venir hasheada de la BLL
                Language = language,
                EmployeeId = employeeId,
                IsDeleted = false
            };
        }

        public void UpdateEmployeeId(Guid employeeId)
        {
            EmployeeId = employeeId;
        }

        public void UpdatePassword(string pass)
        {
            Password = pass;
        }

        // Factory para RECONSTITUCIÓN desde DB
        public static Usuario Reconstitute(Guid id, string username, string password, string language, string dvh, Guid employeeId, bool isDeleted)
        {
            return new Usuario
            {
                Id = id,
                Username = username,
                Password = password,
                Language = language,
                DVH = !string.IsNullOrEmpty(dvh) ? DvhVo.Create(dvh) : null,
                EmployeeId = employeeId,
                IsDeleted = isDeleted
            };
        }

        // --- LÓGICA DE PERMISOS ---
        public void AddPermiso(PermisoComponente componente) => Permisos.Add(componente);
        public void ClearPermisos() => Permisos.Clear();

        public bool HasPermission(string codigo) => Permisos.Any(p => CheckRecursivo(p, codigo));

        private bool CheckRecursivo(PermisoComponente componente, string codigo)
        {
            if (componente.Permiso == codigo) return true;
            return componente.Hijos.Any(hijo => CheckRecursivo(hijo, codigo));
        }

        public void UpdateDVH(string dvh) => DVH = DvhVo.Create(dvh);
    }
}