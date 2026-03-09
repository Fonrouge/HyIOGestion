using BLL.LogicLayers.Clients;
using BLL.DTOs;
using SharedAbstractions.ArchitecturalMarkers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presenter.ForClient
{
    public class CreateClientPresenter: IPresenter
    {

        private readonly ICreateClientView _view;
        private readonly IUCCreateClient _useCaseCreate;

        public CreateClientPresenter
        (
            ICreateClientView view,
            IUCCreateClient useCaseCreate
        )
        {
            _view = view;
            _useCaseCreate = useCaseCreate;

            WireViewEvents();
            ApplyDarkTheme();
        }

        private void WireViewEvents()
        {
            _view.CreateClientRequested += (sender, e) => OnCreateClientRequested(e);
        }

        private void ApplyDarkTheme() => _view.ApplyGlobalPalette();
        private async Task OnCreateClientRequested(ClientDTO clientData)
        {
            try
            {
                var opRes = await _useCaseCreate.ExecuteAsync(clientData);

                _view.ShowOperationResult(opRes);
            }
            catch
            {
                var inCaseOfUncoveredException = new OperationResult<ClientDTO>
                {
                    Errors = new List<ErrorLogDTO>
                    {
                        new ErrorLogDTO
                        {
                            Code = "EXCEPTION",
                            Message = "An unexpected error occurred while creating the client."
                        }
                    }
                };

                _view.ShowOperationResult(inCaseOfUncoveredException);
            }
        }
    }


}
