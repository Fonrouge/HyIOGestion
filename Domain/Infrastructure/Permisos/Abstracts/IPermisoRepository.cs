using Domain.Entities.Permisos.Abstracts;
using Domain.Entities.Permisos.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IPermisoRepository
    {
        void SetTransaction(object transaction);
        Task<List<PermisoComponente>> GetPermissionsByUserAsync(Guid userId);
        Task<IEnumerable<PermisoComponente>> GetAllAsync();
        Task<List<PermisoRelacionDTO>> GetAllPermisoPermisoAsync();
    }
}
