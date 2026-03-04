using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Payments
{
    public interface IUCUpdatePayment
    {
        Task<OperationResult<PaymentDTO>> Execute(PaymentDTO dto);
    }
}
