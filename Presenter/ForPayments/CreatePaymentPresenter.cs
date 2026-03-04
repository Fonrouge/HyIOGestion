using BLL.LogicLayers;
using BLL.LogicLayers.Payments;
using BLL.DTOs;
using SharedAbstractions.ArchitecturalMarkers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Presenter.ForPayments;

namespace Presenter.ForEmployee
{
    public class CreatePaymentPresenter: IPresenter
    {
        private readonly IPaymentView _view;
        private readonly IUCCreatePayment _useCaseCreate;
               
        public CreatePaymentPresenter
        (
            IPaymentView view,
            IUCCreatePayment useCaseCreate
        )
        {
            _view = view;
            _useCaseCreate = useCaseCreate;

            WireViewEvents();
        }

        private void WireViewEvents()
        {
   //         _view.CreatePaymentRequested += (sender, e) => OnCreateRequested(e);
        }


        private async Task OnCreateRequested(PaymentDTO data)
        {
            try
            {
                var opRes = await _useCaseCreate.Execute(data);
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
