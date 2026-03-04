using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.UseCases
{
    public interface IUCCreateUser
    {
        Task<OperationResult<UsuarioDTO>> CreateAsync(UsuarioDTO userDto);
    }
}