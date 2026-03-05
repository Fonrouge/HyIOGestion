using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Products.Categories.UseCases
{
    public interface IUCCreateCategory
    {
        Task<OperationResult<CategoryDTO>> ExecuteAsync(CategoryDTO dto);
    }
}
