using BLL.DTOs;
using BLL.LogicLayers;
using System;
using System.Collections.Generic;

namespace Presenter.ForSale
{
    public interface ICreateSaleView
    {
        // ==================== EVENTOS EXISTENTES ====================
        event EventHandler CreateSaleRequested;
        event EventHandler OnLoadRequested;
        event EventHandler OnCloseRequested;
        event EventHandler UpdateSubTotalRequested;

        // ==================== NUEVOS EVENTOS (para botones) ====================
        event EventHandler AddProductRequested;
        event EventHandler RemoveProductRequested;

        // ==================== MÉTODOS EXISTENTES ====================
        void InitializeClientGrid(List<ClientDTO> allClientsList);
        void InitializeProductsGrid(List<ProductDTO> allProductsList);
        void AddViewTranslatables();
        void ShowOperationResultClients(OperationResult<ClientDTO> opRes);
        void ShowOperationResultProducts(OperationResult<ProductDTO> opRes);
        void ShowOperationResultSales(OperationResult<SaleDTO> opRes);
        void UpdateSubTotal(string subTotal);
        void ShowWarning(string message);
        SaleDTO GetMappedSaleDTO();

        // ==================== NUEVOS MÉTODOS PARA EL LISTBOX ====================
        ProductDTO GetSelectedProduct();      // lee dgvSelectProduct
        decimal GetSelectedQuantity();        // lee tbQuantity.Text

        void AddSaleDetailToList(SaleDetailDTO detail);
        void RemoveSelectedSaleDetailFromList();
        List<SaleDetailDTO> GetCurrentSaleDetails();   // devuelve la BindingList como List
        void ClearSaleDetailsList();                   // opcional

        // Para que el Presenter pueda recalcular subtotal
        void RefreshSubTotal(decimal total);
    }
}