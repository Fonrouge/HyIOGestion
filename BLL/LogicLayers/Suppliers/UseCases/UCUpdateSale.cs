using BLL.DTOs;
using Domain.Infrastructure;
using System;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Suppliers
{
    public class UCUpdateSupplierMOCK : IUCUpdateSupplier
    {
        private readonly IUnitOfWork _uow;

        public UCUpdateSupplierMOCK
        (
            IUnitOfWork uow
        )
        {
            _uow = uow ?? throw new ArgumentNullException($"{nameof(uow)} cannot be null");
        }

        public async Task<OperationResult<SupplierDTO>> Execute(SupplierDTO dto)
        {
            return new OperationResult<SupplierDTO>();
        }
    }
}
