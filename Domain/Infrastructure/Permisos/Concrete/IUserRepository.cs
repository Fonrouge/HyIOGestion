using Domain.Entities.Permisos.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUserRepository: ICrudl<Usuario>
    {
        Task<IEnumerable<Usuario>> GetByUsernameAsync(string username);
    }
}
