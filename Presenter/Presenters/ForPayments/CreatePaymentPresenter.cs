using BLL.DTOs;
using BLL.LogicLayers;
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
            .Cast<PaymentMethodsEnum>()
            .Select(d =>
            {
                var attr = d.GetPaymentMethodsInfo();
                return new
                {
                    Id = attr?.Id ?? "0",
                    Display = attr?.Description ?? d.ToString()
                };

            }).ToList();

            _view.FillPaymentMethods(datasourcePayMeth);
        }

        private async Task OnListAllClientsRequested()
        {
            var result = await _uCGetAllSales.ExecuteAsync();

            var allSales = result.Item1.ToList();
            var opResult = result.Item2;

            if (!(allSales.Count > 0))
                _view.InitializeGrid(new List<SaleDTO>());
            else
                _view.InitializeGrid(allSales);

            _view.ShowOperationResult(opResult);
        }

        private void OnCloseRequested() => _view.Dispose();

        private async Task OnCreateRequested(PaymentDTO data)
        {
            var opRes = await _useCaseCreate.ExecuteAsync(data);
            _view.ShowOperationResult(opRes);
        }
    }
}
