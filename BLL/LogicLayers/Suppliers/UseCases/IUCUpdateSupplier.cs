using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Suppliers
{
    public interface IUCUpdateSupplier
    {
        Task<OperationResult<SupplierDTO>> ExecuteAsync(SupplierDTO dto);
    }
}
