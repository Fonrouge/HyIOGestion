using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IEmployeeRepository: ICrudl<Employee>
    {
        Task<Employee> GetByFileNumberAsync(string fileNumber);
        Task<Employee> GetByTaxIdAsync(string fileNumber);
    }
}
