using BLL.LogicLayers;
using BLL.LogicLayers.Sales;
using BLL.DTOs;
using System;

namespace Presenter.ForSale
{
    public class SalePresenter
    {
        private readonly ISaleView _view;
        private readonly IUCGetAllSales _ucGetAll;
        private readonly IUCUpdateSale _ucUpdate;
        private readonly IUCDeleteSale _ucDelete;

        public SalePresenter
        (
            ISaleView view,
            IUCGetAllSales ucGetAll,
            IUCUpdateSale ucUpdate,
            IUCDeleteSale ucDelete
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
            _view.CreateRequested += (s, e) => OnOpenCreationForm();
            _view.UpdateRequested += (s, e) => UpdateSale(e);
            _view.DeleteRequested += (s, e) => DeleteSale(e);

            _view.ListAllRequested += (s, e) => GetAllSales();
        }

        private void OnOpenCreationForm() => _view.OpenCreationForm();

        private async void UpdateSale(SaleDTO e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var opRes = await _ucUpdate.ExecuteAsync(e);
            ShowResult(opRes);

            // Refrescar lista si salió bien
            if (!opRes.Success) GetAllSales();
        }

        private async void DeleteSale(SaleDTO e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var opRes = await _ucDelete.ExecuteAsync(e);
            ShowResult(opRes);

            // Refrescar lista si salió bien
            if (!opRes.Success) GetAllSales();
        }

        private async void GetAllSales()
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

        private void ShowResult(OperationResult<SaleDTO> opRes)
            => _view.ShowOperationResult(opRes);
    }
}
