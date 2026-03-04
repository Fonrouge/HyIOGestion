using System.Threading.Tasks;

namespace BLL.UseCases
{
    public interface IVerifyDVH
    {
        Task<bool> ExecuteAsync();
    }
}
