using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Payments
{
    public interface IUCCreatePayment
    {
        Task<OperationResult<PaymentDTO>> ExecuteAsync(PaymentDTO dto);
    } 
}
