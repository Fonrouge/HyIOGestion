using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Products
{
    public interface IUCCreateProduct
    {
        Task<OperationResult<ProductDTO>> Execute(ProductDTO dto);
    } 
}
