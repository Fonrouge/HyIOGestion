using BLL.LogicLayers;
using BLL.LogicLayers.Payments;
using BLL.DTOs;
using SharedAbstractions.ArchitecturalMarkers;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.LogicLayers.Clients;
using System.Linq;
using BLL.Infrastructure.Errors;

namespace Presenter.ForEmployee
{
    public class CreatePaymentPresenter : IPresenter
    {
        private readonly ICreatePaymentView _view;
        private readonly IUCCreatePayment _useCaseCreate;
        private readonly IUCGetAllClients _uCGetAllClients;

        public CreatePaymentPresenter
        (
            ICreatePaymentView view,
            IUCCreatePayment useCaseCreate,
            IUCGetAllClients uCGetAllClients
        )
        {
            _view = view;
            _useCaseCreate = useCaseCreate;
            _uCGetAllClients = uCGetAllClients;

            WireViewEvents();
        }

        private void WireViewEvents()
        {
            _view.CreatePaymentRequested += (sender, e) => OnCreateRequested(e);
            _view.GetAllClientsRequested += (sender, e) => OnListAllClientsRequested();
            _view.CloseRequested += (sender, e) => OnCloseRequested();

        }

        private async Task OnListAllClientsRequested()
        {
            OperationResult<ClientDTO> opResult;

            try
            {
                var result = await _uCGetAllClients.ExecuteAsync();

                List<ClientDTO> allClients = result.Item1.ToList();
                opResult = result.Item2;

                _view.CachingClientList(allClients);

            }

            catch //La información de errores proviene de capas superiores
            {
                opResult = new OperationResult<ClientDTO>();

                if (opResult.Errors.Count == 0)
                {
                    var newError = new ErrorLogDTO() { Message = "Error desconocido. Por favor intente nuevamente o reinicie el programa" };
                    opResult.Errors.Add(newError);

                    _view.ShowOperationResult(opResult);
                }
            }
        }

        private void OnCloseRequested() => _view.Dispose();


        private async Task OnCreateRequested(PaymentDTO data)
        {
            try
            {
                var opRes = await _useCaseCreate.ExecuteAsync(data);
                _view.ShowOperationResult(opRes);
            }
            catch
            {
                var inCaseOfUncoveredException = new OperationResult<PaymentDTO>
                {
                    Errors = new List<ErrorLogDTO>
                    {
                        new ErrorLogDTO
                        {
                            Code = "EXCEPTION",
                            Message = "An unexpected error occurred while creating the entity."
                        }
                    }
                };

                _view.ShowOperationResult(inCaseOfUncoveredException);
            }
        }
    }
}
