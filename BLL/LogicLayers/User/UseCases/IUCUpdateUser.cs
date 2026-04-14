using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers
{
    public interface IUCUpdateUser
    {
        Task<OperationResult<UsuarioDTO>> ExecuteAsync(UsuarioDTO userDto);
    }
}