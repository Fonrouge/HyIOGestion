using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProductRepository: ICrudl<Product>
    {
        Task<Product> GetByNameAsync(string name);
        Task<IEnumerable<ProductCategoryDTO>> GetAllProductCategoryAsync();

        Task UpdateRelationDVHAsync(Guid productId, Guid categoryId, string newDvh);
        Task UpdateAsync(Product entity, IEnumerable<ProductCategoryDTO> relations);
        Task CreateAsync(Product entity, IEnumerable<ProductCategoryDTO> relations);
    }
}
