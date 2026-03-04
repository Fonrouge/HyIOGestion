using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers
{
    public interface IUCModifyUser
    {
        Task<OperationResult<UsuarioDTO>> ExecuteAsync(UsuarioDTO userDto);
    }
}