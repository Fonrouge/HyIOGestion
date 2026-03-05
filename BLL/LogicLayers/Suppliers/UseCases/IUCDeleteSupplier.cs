using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Suppliers
{
    public interface IUCDeleteSupplier
    {
        Task<OperationResult<SupplierDTO>> ExecuteAsync(SupplierDTO dto);
    }
}
