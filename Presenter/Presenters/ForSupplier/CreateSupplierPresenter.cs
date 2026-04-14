using BLL.DTOs;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Threading.Tasks;
using BLL.LogicLayers.Suppliers;

namespace Presenter.ForSupplier
{
    public class CreateSupplierPresenter : IPresenter, IDisposable
    {
        private readonly ICreateSupplierView _view;
        private readonly IUCCreateSupplier _useCaseCreate;

        public CreateSupplierPresenter
        (
            ICreateSupplierView view,
            IUCCreateSupplier useCaseCreate
        )
        {
            _view = view;
            _useCaseCreate = useCaseCreate;

            WireViewEvents();
            ApplyDarkTheme();
        }

        private void ApplyDarkTheme() => _view.ThemingNotifiedByConfigurationsModule();

        private void WireViewEvents()
        {
            _view.CreateSupplierRequested += HandleCreateSupplierRequested;
            _view.CloseRequested += HandleCloseRequested;
        }

        // ===================================================================
        // Event Handlers
        // ===================================================================
        private async void HandleCreateSupplierRequested(object sender, SupplierDTO e) => await OnCreateRequested(e);
        private void HandleCloseRequested(object sender, EventArgs e) => Dispose();


        // ===================================================================
        // Lógica de Casos de Uso
        // ===================================================================
        private async Task OnCreateRequested(SupplierDTO data)
        {
            var opRes = await _useCaseCreate.ExecuteAsync(data);
            _view.ShowOperationResult(opRes);
        }

        // ===================================================================
        // IDisposable (Prevención de Fugas de Memoria)
        // ===================================================================
        public void Dispose()
        {
            if (_view != null)
            {
                _view.CreateSupplierRequested -= HandleCreateSupplierRequested;
                _view.CloseRequested -= HandleCloseRequested;
            }
        }
    }
}