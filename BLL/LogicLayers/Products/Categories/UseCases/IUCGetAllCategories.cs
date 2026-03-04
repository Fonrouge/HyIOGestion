using BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Products.Categories.UseCases
{
    public interface IUCGetAllCategories
    {
        Task<(IEnumerable<CategoryDTO> categories, OperationResult<CategoryDTO> operationResult)> ExecuteAsync();
    }
}
