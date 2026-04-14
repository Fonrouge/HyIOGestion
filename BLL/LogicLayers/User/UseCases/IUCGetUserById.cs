using BLL.DTOs;
using System;
using System.Threading.Tasks;

namespace BLL.LogicLayers.User.UseCases
{
    public interface IUCGetUserById
    {
        Task<(UsuarioDTO, OperationResult<UsuarioDTO>)> ExecuteAsync(Guid id);
    }
}
