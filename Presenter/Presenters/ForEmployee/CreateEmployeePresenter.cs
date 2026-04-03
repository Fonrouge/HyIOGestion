using BLL.DTOs;
using BLL.LogicLayers.Employees;
using SharedAbstractions.ArchitecturalMarkers;
using SharedAbstractions.Enums;
using System;
using System.Linq;

namespace Presenter.ForEmployee
{
    public class CreateEmployeePresenter : IPresenter
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
            .Select(d => new
            {
                Id = d.GetDocInfo().Id,
                Display = d.GetDocInfo().Description
            }).ToList();

            _view.FillCountries(datasourceDocs);
        }

        private void WireViewEvents()
        {
            _view.CreateEmployeeRequested += OnCreateRequested;
        }

        private async void OnCreateRequested(object sender, EmployeeDTO e)
        {
            var opRes = await _useCaseCreate.ExecuteAsync(e);
            _view.ShowOperationResult(opRes);
        }
    }
}
