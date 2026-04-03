using Domain.Contracts;
using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IClientRepository : ICrudl<Client>
    {
        Task<Client> GetByDocNumberAsync(string taxId);
    }
}