using Domain.Repositories;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public interface  ISupplierRepository: ICrudl<Supplier>
    {
        Task<Supplier> GetByTaxIdAsync(string taxId);
    }
}
