using BLL.DTOs;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Clients
{
    public interface IUCCreateClient
    {
        Task<OperationResult<ClientDTO>> ExecuteAsync(ClientDTO dto);
    } 
}
