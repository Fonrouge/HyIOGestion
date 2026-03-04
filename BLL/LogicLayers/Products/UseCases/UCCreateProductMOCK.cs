using BLL.DTOs;
using Domain.Entities;
using Domain.Infrastructure;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Products
{
    public class UCCreateProductMOCK : IUCCreateProduct
    {

        private readonly IUnitOfWork _uow;

        public UCCreateProductMOCK
        (
            IUnitOfWork uow
        )
        {
            _uow = uow;
        }

        public async Task<OperationResult<ProductDTO>> Execute(ProductDTO dto)
        {



            return new OperationResult<ProductDTO>();
            
        }
    }
}
