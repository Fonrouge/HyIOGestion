using Domain.Exceptions.Base;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IErrorsRepository
    {
        Task CreateAsync(ErrorLog exception);
        void SetTransaction(object transaction);
    }
}