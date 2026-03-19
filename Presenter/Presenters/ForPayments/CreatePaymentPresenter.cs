using BLL.DTOs;
using BLL.Infrastructure.Errors;
using BLL.LogicLayers;
using BLL.LogicLayers.Clients;
using BLL.LogicLayers.Payments;
using BLL.LogicLayers.Sales;
using SharedAbstractions.ArchitecturalMarkers;
using SharedAbstractions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presenter.ForEmployee
{
    public class CreatePaymentPresenter : IPresenter
    {
        private readonly ICreatePaymentView _view;
        private readonly IUCCreatePayment _useCaseCreate;
        private readonly IUCGetAllSales _uCGetAllSales;

        public CreatePaymentPresenter
        (
            ICreatePaymentView view,
            IUCCreatePayment useCaseCreate,
            IUCGetAllSales uCGetAllCSales
        )
        {
            _view = view;
            _useCaseCreate = useCaseCreate;
            _uCGetAllSales = uCGetAllCSales;

            WireViewEvents();
            FillDropDownData();
        }

        private void WireViewEvents()
        {
            _view.CreatePaymentRequested += (sender, e) => OnCreateRequested(e);
            _view.GetAllSalesRequested += (sender, e) => OnListAllClientsRequested();
            _view.CloseRequested += (sender, e) => OnCloseRequested();

        }
        private void FillDropDownData()
        {
            var datasourcePayMeth = Enum.GetValues(typeof(PaymentMethodsEnum))
            .Cast<DocTypesEnum>()
            .Select(d => new { Id = d.GetDocInfo().Id, Display = d.GetDocInfo().Description }).ToList();

            _view.FillPaymentMethods(datasourcePayMeth);
        }
        private async Task OnListAllClientsRequested()
        {
            OperationResult<SaleDTO> opResult;

            try
            {
                var result = await _uCGetAllSales.ExecuteAsync();

                List<SaleDTO> allSales = result.Item1.ToList();
                opResult = result.Item2;

                _view.InitializeGrid(allSales);

            }

            catch //La información de errores proviene de capas superiores
            {
                opResult = new OperationResult<SaleDTO>();

                if (opResult.Errors.Count == 0)
                {
                    var newError = new ErrorLogDTO() { Message = "Error desconocido al cargar ventas. Por favor intente nuevamente o reinicie el programa" };
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
