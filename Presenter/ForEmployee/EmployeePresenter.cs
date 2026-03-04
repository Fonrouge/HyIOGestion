using BLL.LogicLayers;
using BLL.LogicLayers.Employees;
using BLL.DTOs;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Linq;

namespace Presenter.ForEmployee
{
    public class EmployeePresenter : IPresenter
    {
        private readonly IEmployeeView _view;
        private readonly IUCGetAllEmployees _ucGetAll;        
        private readonly IUCUpdateEmployee _ucUpdate;
        private readonly IUCDeleteEmployee _ucDelete;

        public EmployeePresenter
        (
            IEmployeeView view,
            IUCGetAllEmployees ucGetAll,
            IUCUpdateEmployee ucUpdate,
            IUCDeleteEmployee ucDelete
        )
        {
            _view = view;
            _ucGetAll = ucGetAll;
            _ucUpdate = ucUpdate;
            _ucDelete = ucDelete;

            WireEvents();
            ApplyDarkTheme();
        }

        private void ApplyDarkTheme() => _view.ApplyGlobalPalette();

        private void WireEvents()
        {
            // Mapeo de eventos GENÉRICOS (ICrudView<EmployeeDTO>)
            _view.CreateRequested += (s, e) => OnOpenCreationForm();
            _view.UpdateRequested += (s, e) => UpdateEmployee(e);
            _view.DeleteRequested += (s, e) => DeleteEmployee(e);

            // Evento para listar (renombrado de CachingAll... a ListAll...)
            _view.ListAllRequested += (s, e) => GetAllEmployees();
            
        }

        private void OnOpenCreationForm() => _view.OpenCreationForm();

        private async void UpdateEmployee(EmployeeDTO e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var opRes = await _ucUpdate.Execute(e);
            ShowResult(opRes);

            // Refrescar lista si salió bien
            if (!opRes.Errors.Any()) GetAllEmployees();
        }

        private async void DeleteEmployee(EmployeeDTO e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var opRes = await _ucDelete.Execute(e);
            ShowResult(opRes);

            // Refrescar lista si salió bien
            if (!opRes.Errors.Any()) GetAllEmployees();
        }

        private async void GetAllEmployees()
        {
            var tuple = await _ucGetAll.ExecuteAsync();

            var employeeList = tuple.Item1;
            var opResult = tuple.Item2;    


            if (!opResult.Success)
            {
                ShowResult(opResult);
            }
            else
            {
                try
                {
                    // Método genérico de ICrudView
                    _view.CachingList(employeeList);
                }
                finally
                {
                    _view.FillDGV();
                }
            }
        }

        private void ShowResult(OperationResult<EmployeeDTO> opRes)
            => _view.ShowOperationResult(opRes);

       
    }
}