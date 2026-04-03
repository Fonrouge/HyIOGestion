using Domain.Contracts;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public interface  ISupplierRepository: ICrudl<Supplier>
    {
        Task<Supplier> GetByTaxNumberAsync(string taxNumber);
    }
}
