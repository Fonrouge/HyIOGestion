using BLL.LogicLayers.Products;
using BLL.LogicLayers.Products.Categories.UseCases;
using Presenter.Messaging;
using System;

namespace Presenter.Presenters.ForProducts
{
    public class UpdateProductPresenter
    {

        private readonly IUpdateProductView _view;
        private readonly IUCUpdateProduct _ucUpdate;
        private readonly IUCGetAllCategories _ucGetAllCategories;
        private readonly IMessenger _messenger;


        public UpdateProductPresenter
        (
            IUpdateProductView view,
            IUCUpdateProduct ucUpdate,
            IUCGetAllCategories ucGetAllCategories,
            IMessenger messenger
        )
        {
            _view = view;
            _ucUpdate = ucUpdate;
            _ucGetAllCategories = ucGetAllCategories;
            _messenger = messenger;

            WireViewEvents();
        }

        private void WireViewEvents()
        {
            _view.UpdateProductRequested += OnUpdateRequested;
            _view.MinimizeRequested += OnMinimizeWindowtRequest;
            _view.CloseRequested += OnCloseWindowRequest;
            _view.ListAllCategoriesRequested += OnListAllRequested;
        }

        private async void OnListAllRequested(object sender, EventArgs e)
        {
            var tuple = await _ucGetAllCategories.ExecuteAsync();
            
            var categories = tuple.categories;
            var opRes = tuple.operationResult;

            if (opRes.Success)
            {
                _view.CachingCategoriesList(categories);
            }
            else
            {
                _view.ShowCategoriesOperationResult(opRes);
            }
        }

        private async void OnUpdateRequested(object sender, ProductDTO product)
        {
            var opRes = await _ucUpdate.ExecuteAsync(product);
         
            _view.ShowOperationResult(opRes);

            if (opRes.Success)
            {
                SendRelistAsk();
            }
        }
        private void SendRelistAsk()
        {
            var relistSuppliersAsk = new ProductsRelistRequestMessage(this);
            _messenger.Send(relistSuppliersAsk);
        }

        private void OnMinimizeWindowtRequest(object sender, EventArgs e) => _view.MinimizeView();
        private void OnCloseWindowRequest(object sender, EventArgs e) => _view.CloseView();

    }
}
