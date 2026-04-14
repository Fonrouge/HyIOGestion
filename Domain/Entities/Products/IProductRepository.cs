using Domain.Contracts;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProductRepository: ICrudl<Product>
    {
        Task<Product> GetByNameAsync(string name);
        Task<IEnumerable<ProductCategoryRelacionDTO>> GetAllProductCategoryAsync();


    }
}
