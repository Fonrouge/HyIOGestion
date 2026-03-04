using BLL.DTOs;
using Domain.Entities;
using Domain.Infrastructure;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Payments
{
    public class UCCreatePaymentMOCK : IUCCreatePayment
    {

        private readonly IUnitOfWork _uow;

        public UCCreatePaymentMOCK
        (
            IUnitOfWork uow
        )
        {
            _uow = uow;
        }

        public async Task<OperationResult<PaymentDTO>> Execute(PaymentDTO dto)
        {
            //_uow.ClientRepo.Create();
            return new OperationResult<PaymentDTO>();
        }
    }
}
