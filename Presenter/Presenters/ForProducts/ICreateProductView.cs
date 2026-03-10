using BLL.LogicLayers;
using BLL.DTOs;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;

namespace Presenter.ForProducts
{
    public interface ICreateProductView : IView
    {
        // ==========================================================
        // Eventos (De la Vista hacia el Presenter)
        // ==========================================================
        event EventHandler<ProductDTO> CreateProductRequested;
        event EventHandler ListAllCategoriesRequested;
        event EventHandler CloseRequested;


        // ==========================================================
        // Métodos / Acciones (Del Presenter hacia la Vista)
        // ==========================================================
        void CachingCategoriesList(IEnumerable<CategoryDTO> categoriesList);
        void ShowOperationResult(OperationResult<ProductDTO> opRes);
    }
}