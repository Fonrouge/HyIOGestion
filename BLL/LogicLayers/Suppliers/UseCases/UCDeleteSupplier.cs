using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Suppliers
{
    public class UCDeleteSupplierMOCK : IUCDeleteSupplier
    {
        public async Task<OperationResult<SupplierDTO>> Execute(SupplierDTO dto)
        {
            return new OperationResult<SupplierDTO>();
        }
    }
}
