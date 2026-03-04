using BLL.LogicLayers;
using BLL.LogicLayers.Payments;
using BLL.LogicLayers.Products;
using BLL.DTOs;
using Presenter.ForEmployee;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.LogicLayers.Products.Categories.UseCases;

namespace Presenter.ForProducts
{
    public class CreateProductPresenter : IPresenter
    {
        private readonly ICreateProductView _view;
        private readonly IUCCreateProduct _useCaseCreate;
        private readonly IUCGetAllCategories _useCaseGetAllCat;

        //private readonly IUCGetAllCategories _useCaseCreate;

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

        private async Task WireViewEvents()
        {
            _view.CreateProductRequested += (sender, e) => OnCreateRequested(e);
            _view.ListAllCategoriesRequested += (sender, e) => OnListAllCategoriesRequested();
        }

        private async Task OnListAllCategoriesRequested()
        {
            var operationResult = await _useCaseGetAllCat.ExecuteAsync();

            var categories = operationResult.Item1;
            var opResult = operationResult.Item2;

            _view.CachingCategoriesList(categories);
        }



        private async Task OnCreateRequested(ProductDTO data)
        {
            try
            {
                var opRes = await _useCaseCreate.Execute(data);
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
    }
}
