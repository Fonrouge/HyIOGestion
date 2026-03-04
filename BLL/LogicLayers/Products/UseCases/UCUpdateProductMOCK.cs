using BLL.DTOs;
using Domain.Infrastructure;
using System;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Products
{
    public class UCUpdateProductMOCK : IUCUpdateProduct
    {
        private readonly IUnitOfWork _uow;

        public UCUpdateProductMOCK
        (
            IUnitOfWork uow
        )
        {
            _uow = uow ?? throw new ArgumentNullException($"{nameof(uow)} cannot be null");
        }

        public async Task<OperationResult<ProductDTO>> Execute(ProductDTO dto)
        {
            return new OperationResult<ProductDTO>();
        }
    }
}
