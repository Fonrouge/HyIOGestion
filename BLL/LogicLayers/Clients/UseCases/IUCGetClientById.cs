using BLL.DTOs;
using System;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Clients.UseCases
{
    public interface IUCGetClientById
    {
        Task<(ClientDTO, OperationResult<ClientDTO>)> ExecuteAsync(Guid id);
    }
}
