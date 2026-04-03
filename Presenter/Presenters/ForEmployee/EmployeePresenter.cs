using BLL.LogicLayers.Employees;
using BLL.DTOs;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Presenter.ForEmployee
{
    public class EmployeePresenter : IPresenter, IDisposable
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
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _ucGetAll = ucGetAll ?? throw new ArgumentNullException(nameof(ucGetAll));
            _ucUpdate = ucUpdate ?? throw new ArgumentNullException(nameof(ucUpdate));
            _ucDelete = ucDelete ?? throw new ArgumentNullException(nameof(ucDelete));

            WireViewEvents();
            ApplyDarkTheme();
        }

        private void ApplyDarkTheme() => _view.ApplyGlobalPalette();

        private void WireViewEvents()
        {
            // Usamos nombres explícitos de métodos para poder desuscribir en Dispose
            _view.CreateRequested += HandleCreateRequested;
            _view.UpdateRequested += HandleUpdateRequested;
            _view.DeleteRequested += HandleDeleteRequested;
            _view.ListAllRequested += HandleListAllRequested;
            _view.CloseRequested += HandleCloseRequested;
        }


        // =========================================================
        // Event Handlers (Orquestación de UI)
        // =========================================================
        private void HandleCreateRequested(object sender, EventArgs e) => _view.OpenCreationView();
        private async void HandleUpdateRequested(object sender, EmployeeDTO e) => await OnUpdateRequested(e);
        private async void HandleDeleteRequested(object sender, EmployeeDTO e) => await OnDeleteRequested(e);
        private async void HandleListAllRequested(object sender, EventArgs e) => await OnGetAllRequested();
        private void HandleCloseRequested(object sender, EventArgs e) => Dispose();


        // =========================================================
        // Lógica de Casos de Uso (Task-based)
        // =========================================================
        private async Task OnUpdateRequested(EmployeeDTO employee) => await _view.OpenUpdateView();

        private async Task OnDeleteRequested(EmployeeDTO employee)
        {
            var opRes = await _ucDelete.ExecuteAsync(employee);
            _view.ShowOperationResult(opRes);

            if (opRes.Success) await OnGetAllRequested();
        }

        private async Task OnGetAllRequested()
        {
            // Deconstrucción de tuplas (C# 7.0+)
            var (employees, opResult) = await _ucGetAll.ExecuteAsync();

            if (opResult.Success)
            {
                _view.CachingList(employees);
                _view.FillDGV();
            }
            else
            {
                _view.ShowOperationResult(opResult);
            }
        }


        // =========================================================
        // IDisposable (Limpieza de "Alumbrado")
        // =========================================================
        public void Dispose()
        {
            if (_view != null)
            {
                _view.CreateRequested -= HandleCreateRequested;
                _view.UpdateRequested -= HandleUpdateRequested;
                _view.DeleteRequested -= HandleDeleteRequested;
                _view.ListAllRequested -= HandleListAllRequested;
                _view.CloseRequested -= HandleCloseRequested;
            }
        }
    }
}