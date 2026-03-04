using BLL.DTOs;
using Domain.BaseContracts;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Clients
{
    public interface IUCDeleteClient
    {
        Task<OperationResult<ClientDTO>> ExecuteAsync(ClientDTO dto);
    }
}
