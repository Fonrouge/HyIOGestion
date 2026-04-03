using BLL.DTOs;
using BLL.LogicLayers;
using System;
using System.Collections.Generic;

namespace Presenter.Presenters.ForProducts
{
    public interface IUpdateProductView
    {
        event EventHandler<ProductDTO> UpdateProductRequested;
        event EventHandler MinimizeRequested;
        event EventHandler CloseRequested;
        event EventHandler ListAllCategoriesRequested;
        void ShowOperationResult(OperationResult<ProductDTO> opRes);
        void ShowCategoriesOperationResult(OperationResult<CategoryDTO> opRes);
        void CachingCategoriesList(IEnumerable<CategoryDTO> categoriesList);

        void MinimizeView();
        void CloseView();
        void SetProductData(ProductDTO dto);
    }
}
