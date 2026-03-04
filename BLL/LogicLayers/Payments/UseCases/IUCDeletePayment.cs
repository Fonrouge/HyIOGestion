using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Payments
{
    public interface IUCDeletePayment
    {
        Task<OperationResult<PaymentDTO>> Execute(PaymentDTO dto);
    }
}
