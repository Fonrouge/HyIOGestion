using BLL.DTOs;
using BLL.LogicLayers.Employees;
using SharedAbstractions.ArchitecturalMarkers;
using SharedAbstractions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
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
            FillDropDownData();
        }
        
        private void FillDropDownData()
        {
            var datasourceDocs = Enum.GetValues(typeof(DocTypesEnum))
            .Cast<DocTypesEnum>()
            .Select(d => new { Id = d.GetDocInfo().Id, Display = d.GetDocInfo().Description }).ToList();

            _view.FillCountries(datasourceDocs);
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
