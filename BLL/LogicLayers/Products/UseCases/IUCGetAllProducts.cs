using BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Products
{
    public interface IUCGetAllProducts
    {
        Task<(IEnumerable<ProductDTO>, OperationResult<ProductDTO>)> ExecuteAsync();
    }
}
