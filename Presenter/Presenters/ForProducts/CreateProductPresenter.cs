using BLL.LogicLayers;
using BLL.LogicLayers.Products;
using BLL.DTOs;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.LogicLayers.Products.Categories.UseCases;

namespace Presenter.ForProducts
{
    public class CreateProductPresenter : IPresenter, IDisposable
    {
        private readonly ICreateProductView _view;
        private readonly IUCCreateProduct _useCaseCreate;
        private readonly IUCGetAllCategories _useCaseGetAllCat;

        public CreateProductPresenter
        (
            ICreateProductView view,
            IUCCreateProduct useCaseCreate,
            IUCGetAllCategories useCaseGetAllCat
        )
        {
            _view = view;
            _useCaseCreate = useCaseCreate;
            _useCaseGetAllCat = useCaseGetAllCat;

            WireViewEvents();
            ApplyDarkTheme();
        }

        private void ApplyDarkTheme() => _view.ApplyGlobalPalette();

        private void WireViewEvents()
        {
            // Suscripciones explícitas sin lambdas para permitir la desuscripción
            _view.CreateProductRequested += HandleCreateProductRequested;
            _view.ListAllCategoriesRequested += HandleListAllCategoriesRequested;
            _view.CloseRequested += HandleCloseRequested;
        }

        // ===================================================================
        // Event Handlers
        // ===================================================================
        private async void HandleCreateProductRequested(object sender, ProductDTO e)
        {
            await OnCreateRequested(e);
        }

        private async void HandleListAllCategoriesRequested(object sender, EventArgs e)
        {
            await OnListAllCategoriesRequested();
        }
        private void HandleCloseRequested(object sender, EventArgs e)
        {
            Dispose();
        }
        // ===================================================================
        // Lógica de Casos de Uso
        // ===================================================================
        private async Task OnListAllCategoriesRequested()
        {
            try
            {
                var operationResult = await _useCaseGetAllCat.ExecuteAsync();

                var categories = operationResult.Item1;
                var opResult = operationResult.Item2;

                _view.CachingCategoriesList(categories);
            }
            catch
            {
                _view.CachingCategoriesList(new List<CategoryDTO>());
            }
        }

        private async Task OnCreateRequested(ProductDTO data)
        {
            try
            {
                var opRes = await _useCaseCreate.ExecuteAsync(data);
                _view.ShowOperationResult(opRes);
            }
            catch
            {
                var inCaseOfUncoveredException = new OperationResult<ProductDTO>
                {
                    Errors = new List<ErrorLogDTO>
                    {
                        new ErrorLogDTO
                        {
                            Code = "EXCEPTION",
                            Message = "An unexpected error occurred while creating the entity."
                        }
                    }
                };

                _view.ShowOperationResult(inCaseOfUncoveredException);
            }
        }

        // ===================================================================
        // IDisposable (Prevención de Fugas de Memoria)
        // ===================================================================
        public void Dispose()
        {
            if (_view != null)
            {
                _view.CreateProductRequested -= HandleCreateProductRequested;
                _view.ListAllCategoriesRequested -= HandleListAllCategoriesRequested;
                _view.CloseRequested -= HandleCloseRequested;                
            }
        }
    }
}