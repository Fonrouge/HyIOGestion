using BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Clients
{
    public interface IUCGetAllClients
    {
        Task<(IEnumerable<ClientDTO>, OperationResult<ClientDTO>)> ExecuteAsync();
    }
}