using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Products
{
    public interface IUCUpdateProduct
    {
        Task<OperationResult<ProductDTO>> Execute(ProductDTO dto);
    }
}
