using BLL.DTOs;
using BLL.LogicLayers;
using BLL.LogicLayers.Suppliers;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Presenter.ForSupplier
{
    public class SupplierPresenter : IPresenter
    {

        private readonly ISupplierView _view;
        private readonly IUCGetAllSuppliers _ucGetAll;
        private readonly IUCUpdateSupplier _ucUpdate;
        private readonly IUCDeleteSupplier _ucDelete;

        public SupplierPresenter
        (
            ISupplierView view,
            IUCGetAllSuppliers ucGetAll,
            IUCUpdateSupplier ucUpdate,
            IUCDeleteSupplier ucDelete
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
            // Mapeo de eventos GENÉRICOS (ICrudView<SupplierDTO>)
            _view.CreateRequested += (s, e) => OnOpenCreationForm();
            _view.UpdateRequested += (s, e) => UpdateSupplier(e);
            _view.DeleteRequested += (s, e) => DeleteSupplier(e);

            // Evento para listar (renombrado de CachingAll... a ListAll...)
            _view.ListAllRequested += (s, e) => GetAllSuppliers();
        }

        private void OnOpenCreationForm() => _view.OpenCreationForm();

        private async Task UpdateSupplier(SupplierDTO e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var opRes = await _ucUpdate.Execute(e);
            ShowResult(opRes);

            // Refrescar lista si salió bien
            if (!opRes.Errors.Any()) GetAllSuppliers();
        }

        private async Task DeleteSupplier(SupplierDTO e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var opRes = await _ucDelete.ExecuteAsync(e);
            ShowResult(opRes);

            // Refrescar lista si salió bien
            if (!opRes.Errors.Any()) GetAllSuppliers();
        }

        private async Task GetAllSuppliers()
        {
            var tuple = await _ucGetAll.ExecuteAsync();

            var supplierList = tuple.Item1;
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
                    _view.CachingList(supplierList);
                }
                finally
                {
                    _view.FillDGV();
                }
            }
        }

        private void ShowResult(OperationResult<SupplierDTO> opRes)
            => _view.ShowOperationResult(opRes);
    }
}

