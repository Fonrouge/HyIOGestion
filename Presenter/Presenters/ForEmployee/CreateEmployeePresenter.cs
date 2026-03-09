using BLL.LogicLayers.Employees;
using BLL.DTOs;
using SharedAbstractions.ArchitecturalMarkers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presenter.ForEmployee
{
    public class CreateEmployeePresenter: IPresenter
    {

        private readonly ICreateEmployeeView _view;
        private readonly IUCCreateEmployee _useCaseCreate;

        public CreateEmployeePresenter
        (
            ICreateEmployeeView view,
            IUCCreateEmployee useCaseCreate
        )
        {
            _view = view;
            _useCaseCreate = useCaseCreate;

            WireViewEvents();
        }

        private void WireViewEvents()
        {
            _view.CreateEmployeeRequested += (sender, e) => OnCreateRequested(e);
        }


        private async Task OnCreateRequested(EmployeeDTO data)
        {
            try
            {
                var opRes = await _useCaseCreate.ExecuteAsync(data);
                _view.ShowOperationResult(opRes);
            }
            catch
            {
                var inCaseOfUncoveredException = new OperationResult<EmployeeDTO>
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
