using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Products
{
    public interface IUCDeleteProduct
    {
        Task<OperationResult<ProductDTO>> ExecuteAsync(ProductDTO dto);
    }
}
