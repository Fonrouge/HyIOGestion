using Domain.Contracts;
using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ICategoryRepository: ICrudl<Category>
    {
        Task<Category> GetByNameAsync(string name);
    }
}
