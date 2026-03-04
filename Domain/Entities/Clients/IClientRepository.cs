using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IClientRepository : ICrudl<Client>
    {
        Task<Client> GetByTaxIdAsync(string taxId);
    }
}