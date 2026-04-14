using BLL.DTOs;
using BLL.LogicLayers.Employees;
using Presenter.Messaging;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Threading.Tasks;

namespace Presenter.ForEmployee
{
    public class EmployeePresenter : IPresenter, IDisposable
    {
        private readonly IEmployeeView _view;
        private readonly IUCGetAllEmployees _ucGetAll;
        private readonly IUCDeleteEmployee _ucDelete;
        private readonly IMessenger _messenger;

        public EmployeePresenter
        (
            IEmployeeView view,
            IUCGetAllEmployees ucGetAll,
            IUCDeleteEmployee ucDelete,
            IMessenger messenger
        )
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _ucGetAll = ucGetAll ?? throw new ArgumentNullException(nameof(ucGetAll));
            _ucDelete = ucDelete ?? throw new ArgumentNullException(nameof(ucDelete));
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));

            WireViewEvents();
            ApplyDarkTheme();
            SubscribeRelistMessage();
        }

        
        // =========================================================
        // Suscripción a mensajería global
        // =========================================================
        private void SubscribeRelistMessage() => _messenger.Subscribe<EmployeesRelistRequestMessage>(OnGetAllRequested);
        private void CloseHostFormMessage(object viewId) => _messenger.Send(new HostFormCloseRequestMessage((Guid)viewId, this));


        // =========================================================
        // Estética general
        // =========================================================
        private void ApplyDarkTheme() => _view.ThemingNotifiedByConfigurationsModule();


        // =========================================================
        // Mapeo de eventos de View
        // =========================================================
        private void WireViewEvents()
        {
            _view.CreateRequested += HandleCreateRequested;
            _view.UpdateRequested += HandleUpdateRequested;
            _view.DeleteRequested += HandleDeleteRequested;
            _view.ListAllRequested += HandleListAllRequested;
            _view.CloseRequested += HandleCloseRequested;
        }

        private void HandleCreateRequested(object sender, EventArgs e) => _view.OpenCreationView();
        private void HandleUpdateRequested(object sender, EmployeeDTO e) => OnUpdateRequested(e);
        private async void HandleDeleteRequested(object sender, EmployeeDTO e) => await OnDeleteRequested(e);
        private async void HandleListAllRequested(object sender, EventArgs e) => await OnGetAllRequested();
        private void HandleCloseRequested(object sender, EventArgs e)
        {
            CloseHostFormMessage((Guid)sender);
            Dispose();
        }


        // =========================================================
        // Lógica de Casos de Uso (Task-based)
        // =========================================================
        private void OnUpdateRequested(EmployeeDTO employee) => _view.OpenUpdateView();

        private async Task OnDeleteRequested(EmployeeDTO employee)
        {
            var opRes = await _ucDelete.ExecuteAsync(employee);
            _view.ShowOperationResult(opRes);

            if (opRes.Success) await OnGetAllRequested();
        }

        private async void OnGetAllRequested(EmployeesRelistRequestMessage message) => await OnGetAllRequested();

        private async Task OnGetAllRequested()
        {
            var (clients, opResult) = await _ucGetAll.ExecuteAsync();

            if (opResult.Success)
            {
                _view.CachingList(clients);
                _view.FillDGV();
            }
            else
            {
                _view.ShowOperationResult(opResult);
            }
        }


        // =========================================================
        // Limpieza de RAM
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

            _view.Dispose();
        }
    }
}