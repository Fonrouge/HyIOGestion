
using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Products.Categories.UseCases
{
    public interface IUCDeleteCategory
    {
        Task<OperationResult<CategoryDTO>> ExecuteAsync(CategoryDTO category);
    }
}
