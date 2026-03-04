using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Sales
{
    public interface IUCUpdateSale
    {
        Task<OperationResult<SaleDTO>> ExecuteAsync(SaleDTO dto);
    }
}
