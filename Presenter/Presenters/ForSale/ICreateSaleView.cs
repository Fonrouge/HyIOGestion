using BLL.DTOs;
using BLL.LogicLayers;
using System;

namespace Presenter.ForSale
{
    public interface ICreateSaleView
    {            
        event EventHandler<SaleDTO> CreateSaleRequested;
        void ShowOperationResult(OperationResult<SaleDTO> opRes);
    }
}
