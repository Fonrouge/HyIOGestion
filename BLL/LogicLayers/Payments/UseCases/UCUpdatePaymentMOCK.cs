using BLL.DTOs;
using Domain.Infrastructure;
using System;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Payments
{
    public class UCUpdatePaymentMOCK: IUCUpdatePayment
    {
        private readonly IUnitOfWork _uow;

        public UCUpdatePaymentMOCK
        (
            IUnitOfWork uow
        )
        {
            _uow = uow ?? throw new ArgumentNullException($"{nameof(uow)} cannot be null");
        }

        public async Task<OperationResult<PaymentDTO>> Execute(PaymentDTO dto)
        {
            return new OperationResult<PaymentDTO>();
        }
    }
}
