using BLL.DTOs;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;

namespace Presenter.ForClient
{
    public interface ICreateClientView: IView
    {
        event EventHandler<ClientDTO> CreateClientRequested;
        void ShowOperationResult(OperationResult<ClientDTO> opRes);
        void FillClientDocTypes(IEnumerable<object> docTypes);
        void FillCountries(IEnumerable<object> countries);



    }
}
