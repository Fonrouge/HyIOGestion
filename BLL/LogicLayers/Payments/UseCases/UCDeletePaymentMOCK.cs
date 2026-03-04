using BLL.DTOs;
using Domain.Entities;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Payments
{
    public class UCDeletePaymentMOCK : IUCDeletePayment
    {
        public async Task<OperationResult<PaymentDTO>> Execute(PaymentDTO dto)
        {
            return new OperationResult<PaymentDTO>();
        }
    }
}
