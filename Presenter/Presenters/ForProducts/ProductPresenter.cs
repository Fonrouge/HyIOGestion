using BLL.DTOs;
using BLL.LogicLayers.Products;
using BLL.LogicLayers.Products.Categories.UseCases;
using Presenter.Messaging;
using SharedAbstractions.Enums;
using System;
using System.Threading.Tasks;

namespace Presenter.ForProducts
{
    public class ProductPresenter
    {
        private readonly IProductView _view;
        private readonly IUCGetAllProducts _ucGetAll;
        private readonly IUCDeleteProduct _ucDelete;
        private readonly IUCGetAllCategories _uCGetAllCategories;
        private readonly IMessenger _messenger;

        public ProductPresenter
        (
            IProductView view,
            IUCGetAllProducts ucGetAll,
            IUCDeleteProduct ucDelete,
            IUCGetAllCategories uCGetAllCategories,
            IMessenger messenger
        )
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _ucGetAll = ucGetAll ?? throw new ArgumentNullException(nameof(ucGetAll));
            _ucDelete = ucDelete ?? throw new ArgumentNullException(nameof(ucDelete));
            _uCGetAllCategories = uCGetAllCategories ?? throw new ArgumentNullException(nameof(uCGetAllCategories));
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));

            WireViewEvents();
            ApplyDarkTheme();
            SubscribeMessages();
        }


        // =========================================================
        // Estética general
        // =========================================================
        private void ApplyDarkTheme() => _view.ThemingNotifiedByConfigurationsModule();


        // =========================================================
        // Suscripción a mensajería global
        // =========================================================
        private void SubscribeMessages()
        {
            _messenger.Subscribe<ProductsRelistRequestMessage>(OnGetAllRequested);
            _messenger.Subscribe<ViewContainerGotFocusNotificationMessage>(GotFocusHostForm);
            //Ready for more...
        }
        private void CloseHostFormMessage(object viewId) => _messenger.Send(new ViewContainerCloseRequestMessage((Guid)viewId, this));
        private async void GotFocusHostForm(ViewContainerGotFocusNotificationMessage message)
        {
            if (message.Payload == _view.ViewId)
            {
                _view.ChangeActivationStateFeedbackBar(true);
                await Task.WhenAll(_view.SetFeedbackState(FeedbackState.Idle));
            }
            else
            {
                _view.ChangeActivationStateFeedbackBar(false);
            }
        }


        // =========================================================
        // Estética general
        // =========================================================
        private void WireViewEvents()
        {
            _view.CreateRequested += HandleCreateRequested;
            _view.UpdateRequested += HandleUpdateRequested;
            _view.DeleteRequested += HandleDeleteRequested;
            _view.ListAllRequested += HandleListAllRequested;
            _view.CloseRequested += HandleCloseRequested;
            _view.OnceLoadedAdvice += HandleLoadedOperations;
        }


        // =========================================================
        // Event Handlers (Orquestación de UI)
        // =========================================================
        private void HandleCreateRequested(object sender, EventArgs e) => _view.OpenCreationView();
        private void HandleUpdateRequested(object sender, ProductDTO e) => OnUpdateRequested(e);
        private async void HandleDeleteRequested(object sender, ProductDTO e) => await OnDeleteRequested(e);
        private async void HandleListAllRequested(object sender, EventArgs e) => await OnGetAllRequested();        
        private void HandleCloseRequested(object sender, EventArgs e)
        {
            CloseHostFormMessage((Guid)sender);
            Dispose();
        }
        private void HandleLoadedOperations(object sender, EventArgs e) => _view.SelectFirstGridRow();

                
        // =========================================================
        // Lógica de Casos de Uso
        // =========================================================
        private async Task OnDeleteRequested(ProductDTO product)
        {
            var opRes = await _ucDelete.ExecuteAsync(product);
            _view.ShowOperationResult(opRes);

            if (opRes.Success) await OnGetAllRequested();
        }

        private async void OnGetAllRequested(ProductsRelistRequestMessage message) => await OnGetAllRequested();

        private async Task OnGetAllRequested()
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
        private void OnUpdateRequested(ProductDTO product) => _view.OpenUpdateView();


        // =========================================================
        // Aplicación del patrón OperationResult para UI
        // =========================================================
        private void ShowResult(OperationResult<ProductDTO> opRes) => _view.ShowOperationResult(opRes);


        // =========================================================
        // Limpieza de RAM
        // =========================================================
        public void Dispose()
        {
            if (_view != null)
            {
                _view.CreateRequested += HandleCreateRequested;
                _view.UpdateRequested += HandleUpdateRequested;
                _view.DeleteRequested += HandleDeleteRequested;
                _view.ListAllRequested += HandleListAllRequested;
                _view.CloseRequested += HandleCloseRequested;
            }

            _view.Dispose();
        }
    }
}
