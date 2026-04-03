using BLL.LogicLayers.Products;
using BLL.DTOs;
using System;
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
            ApplyDarkTheme(); 
        }

        private void ApplyDarkTheme() => _view.ApplyGlobalPalette();

        private void WireEvents()
        {
            _view.CreateRequested += (s, e) => OnOpenCreationForm();
            _view.UpdateRequested += (s, e) => UpdateProduct(e);
            _view.DeleteRequested += (s, e) => DeleteProduct(e);
            _view.ListAllRequested += (s, e) => GetAllProducts();
        }

        private void OnOpenCreationForm() => _view.OpenCreationView();

        private async void UpdateProduct(ProductDTO e) => await _view.OpenUpdateView();
        private async void DeleteProduct(ProductDTO e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var opRes = await _ucDelete.ExecuteAsync(e);
            ShowResult(opRes);

            if (opRes.Success)
            {
                GetAllProducts();
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

            if (opResult.Success)
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
            else
            {
                ShowResult(opResult);
            }
        }

        private void ShowResult(OperationResult<ProductDTO> opRes) => _view.ShowOperationResult(opRes);
    }
}
