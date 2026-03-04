using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Products
{
    public class UCDeleteProductMOCK : IUCDeleteProduct
    {
        public async Task<OperationResult<ProductDTO>> Execute(ProductDTO dto)
        {
            return new OperationResult<ProductDTO>();
        }
    }
}
