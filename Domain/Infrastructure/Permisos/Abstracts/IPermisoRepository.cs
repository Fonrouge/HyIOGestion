using Domain.Entities.Permisos.Abstracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IPermisoRepository
    {
        void SetTransaction(object transaction);
        Task<List<PermisoComponente>> GetPermissionsByUserAsync(Guid userId);
    }
}
