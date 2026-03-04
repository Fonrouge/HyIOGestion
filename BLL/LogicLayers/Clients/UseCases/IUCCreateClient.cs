using BLL.DTOs;
using Domain.BaseContracts;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Clients
{
    public interface IUCCreateClient
    {
        Task<OperationResult<ClientDTO>> ExecuteAsync(ClientDTO dto);
    } 
}
