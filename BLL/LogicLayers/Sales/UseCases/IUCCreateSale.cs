using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Sales
{
    public interface IUCCreateSale
    {
        Task<OperationResult<SaleDTO>> ExecuteAsync(SaleDTO dto);

    } 
}
