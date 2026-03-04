using BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Payments
{
    public interface IUCGetAllPayments
    {
        Task<(IEnumerable<PaymentDTO>, OperationResult<PaymentDTO>)> ExecuteAsync();
    }
}