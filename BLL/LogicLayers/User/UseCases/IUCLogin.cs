using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.UseCases
{
    public interface IUCLogin
    {
        Task<OperationResult<UsuarioDTO>> ExecuteAsync(string userName, string pass);

    }
}
