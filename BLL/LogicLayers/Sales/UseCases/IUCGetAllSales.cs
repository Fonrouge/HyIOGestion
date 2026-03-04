using BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Sales
{
    public interface IUCGetAllSales
    {
        Task<(IEnumerable<SaleDTO>, OperationResult<SaleDTO>)> ExecuteAsync();
    }
}