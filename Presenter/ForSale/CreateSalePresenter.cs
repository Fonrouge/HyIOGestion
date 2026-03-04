using BLL.DTOs;
using BLL.LogicLayers;
using BLL.LogicLayers.Sales;
using SharedAbstractions.ArchitecturalMarkers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presenter.ForSale
{
    public class CreateSalePresenter: IPresenter
    {

        private readonly ICreateSaleView _view;
        private readonly IUCCreateSale _useCaseCreate;

        public CreateSalePresenter
        (
            ICreateSaleView view,
            IUCCreateSale useCaseCreate
        )
        {
            _view = view;
            _useCaseCreate = useCaseCreate;

            WireViewEvents();
        }

        private void WireViewEvents()
        {
            _view.CreateSaleRequested += (sender, e) => OnCreateSaleRequested(e);
        }


        private async Task OnCreateSaleRequested(SaleDTO clientData)
        {
            try
            {
                var opRes = await _useCaseCreate.ExecuteAsync(clientData);

                _view.ShowOperationResult(opRes);
            }
            catch
            {
                var inCaseOfUncoveredException = new OperationResult<SaleDTO>
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
