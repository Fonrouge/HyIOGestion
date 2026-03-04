using BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Suppliers
{
    public interface IUCGetAllSuppliers
    {
        Task<(IEnumerable<SupplierDTO>, OperationResult<SupplierDTO>)> ExecuteAsync();
    }
}