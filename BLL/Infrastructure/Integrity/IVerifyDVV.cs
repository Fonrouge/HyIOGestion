using System.Threading.Tasks;

namespace BLL.LogicLayer
{
    public interface IVerifyDVV
    {
        Task<bool> ExecuteAsync(string tabla, string connectionString);
    }
}