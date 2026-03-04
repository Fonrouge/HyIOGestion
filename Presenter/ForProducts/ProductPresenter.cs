using BLL.LogicLayers;
using BLL.LogicLayers.Products;
using BLL.DTOs;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using BLL.LogicLayers.Products.Categories.UseCases;

namespace Presenter.ForProducts
{
    public class ProductPresenter
    {

        private readonly IProductView _view;
        private readonly IUCGetAllProducts _ucGetAll;
        private readonly IUCUpdateProduct _ucUpdate;
        private readonly IUCDeleteProduct _ucDelete;
        private readonly IUCGetAllCategories _uCGetAllCategories;

        public ProductPresenter
        (
            IProductView view,
            IUCGetAllProducts ucGetAll,
            IUCUpdateProduct ucUpdate,
            IUCDeleteProduct ucDelete,
            IUCGetAllCategories uCGetAllCategories
        )
        {
            _view = view;
            _ucGetAll = ucGetAll;
            _ucUpdate = ucUpdate;
            _ucDelete = ucDelete;
            _uCGetAllCategories = uCGetAllCategories;

            WireEvents();
        }

        private void WireEvents()
        {
            // Mapeo de eventos GENÉRICOS (ICrudView<ProductDTO>)
            _view.CreateRequested += (s, e) => OnOpenCreationForm();
            _view.UpdateRequested += (s, e) => UpdateProduct(e);
            _view.DeleteRequested += (s, e) => DeleteProddct(e);

            // Evento para listar (renombrado de CachingAll... a ListAll...)
            _view.ListAllRequested += (s, e) => GetAllProducts();

        }

        private void OnOpenCreationForm() => _view.OpenCreationForm();

        private async void UpdateProduct(ProductDTO e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var opRes = await _ucUpdate.Execute(e);
            ShowResult(opRes);

            // Refrescar lista si salió bien
            if (!opRes.Errors.Any()) GetAllProducts();
        }

        private async void DeleteProddct(ProductDTO e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var opRes = await _ucDelete.Execute(e);
            ShowResult(opRes);

            if (!opRes.Errors.Any())
            {
                GetAllProducts();
                Debugger.Break();
            }
        }

        private async void GetAllProducts()
        {
            var tuple = await _ucGetAll.ExecuteAsync();

            var productList = tuple.Item1;
            var opResult = tuple.Item2;


            var categTuple = await _uCGetAllCategories.ExecuteAsync();

            var categories = categTuple.Item1;

            _view.SetSearchFilters(categories);

            if (!opResult.Success)
            {
                ShowResult(opResult);
            }
            else
            {
                try
                {
                    _view.CachingList(productList);
                }
                finally
                {
                    _view.FillDGV();
                }
            }
        }

        private void ShowResult(OperationResult<ProductDTO> opRes) => _view.ShowOperationResult(opRes);
    }
}
