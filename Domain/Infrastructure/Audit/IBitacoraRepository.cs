using Domain.Exceptions.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IBitacoraRepository
    {
        Task CreateAsync(Bitacora logEntry);
        Task<IEnumerable<Bitacora>> GetAllAsync();
        void SetTransaction(object transaction);
    }
}