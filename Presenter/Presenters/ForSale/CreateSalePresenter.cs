using BLL.DTOs;
using BLL.LogicLayers;
using BLL.LogicLayers.Clients;
using BLL.LogicLayers.Products;
using BLL.LogicLayers.Sales;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Presenter.ForSale
{
    public class CreateSalePresenter : IPresenter
    {
        private readonly ICreateSaleView _view;
        private readonly IUCCreateSale _useCaseCreate;
        private readonly IUCGetAllClients _useCaseAllClients;
        private readonly IUCGetAllProducts _useCaseAllProducts;

        public CreateSalePresenter
        (
            ICreateSaleView view,
            IUCCreateSale useCaseCreate,
            IUCGetAllClients useCaseAllClients,
            IUCGetAllProducts useCaseAllProducts
        )
        {
            _view = view;
            _useCaseCreate = useCaseCreate;
            _useCaseAllClients = useCaseAllClients;
            _useCaseAllProducts = useCaseAllProducts;

            WireViewEvents();
        }

        private void WireViewEvents()
        {
            _view.CreateSaleRequested += async (sender, e) => await OnCreateSaleRequested();
            _view.OnLoadRequested += OnLoadFlow;
            _view.AddProductRequested += OnAddProductRequested;
            _view.RemoveProductRequested += OnRemoveProductRequested;
            _view.UpdateSubTotalRequested += (sender, e) => { }; // por compatibilidad
        }

        private async void OnLoadFlow(object sender, EventArgs e)
        {
            await GetAllProducts();
            await GetAllClients();
            _view.AddViewTranslatables();
        }

        private async Task GetAllClients()
        {
            try
            {
                var tuple = await _useCaseAllClients.ExecuteAsync();
                _view.InitializeClientGrid(tuple.Item1.ToList());
            }
            catch
            {
                var opResult = new OperationResult<ClientDTO>();
                opResult.Errors.Add(new ErrorLogDTO { Message = "Error desconocido al cargar clientes. Por favor intente nuevamente o reinicie el programa" });
                _view.ShowOperationResultClients(opResult);
            }
        }

        private async Task GetAllProducts()
        {
            try
            {
                var tuple = await _useCaseAllProducts.ExecuteAsync();
                _view.InitializeProductsGrid(tuple.Item1.ToList());
            }
            catch
            {
                var opResult = new OperationResult<ProductDTO>();
                opResult.Errors.Add(new ErrorLogDTO { Message = "Error desconocido al cargar productos. Por favor intente nuevamente o reinicie el programa" });
                _view.ShowOperationResultProducts(opResult);
            }
        }

        // ==================== LÓGICA AGREGAR (limpia) ====================
        private void OnAddProductRequested(object sender, EventArgs e)
        {
            var product = _view.GetSelectedProduct();
            var quantity = _view.GetSelectedQuantity();

            if (product == null)
            {
                _view.ShowWarning("Debe seleccionar un producto.");
                return;
            }

            if (quantity <= 0)
            {
                _view.ShowWarning("La cantidad debe ser mayor a cero.");
                return;
            }

            var newDetail = new SaleDetailDTO
            {
                ProductId = product.Id,
                Quantity = quantity,
                UnitPrice = product.Price,
                Subtotal = product.Price * quantity
            };

            _view.AddSaleDetailToList(newDetail);
            RefreshSubTotal();
        }

        // ==================== LÓGICA QUITAR ====================
        private void OnRemoveProductRequested(object sender, EventArgs e)
        {
            _view.RemoveSelectedSaleDetailFromList();
            RefreshSubTotal();
        }

        private void RefreshSubTotal()
        {
            var total = _view.GetCurrentSaleDetails().Sum(d => d.Subtotal);
            _view.RefreshSubTotal(total);
        }

        // ==================== CREACIÓN DE VENTA ====================
        private async Task OnCreateSaleRequested()
        {
            try
            {
                var newSale = _view.GetMappedSaleDTO();

                if (newSale.Items == null || !newSale.Items.Any())
                {
                    _view.ShowOperationResultSales(CreateError<SaleDTO>("INCOMPLETO", "Debe ingresar al menos un producto"));
                    return;
                }

                foreach (var item in newSale.Items)
                    item.SaleId = newSale.Id;

                var operationResult = await _useCaseCreate.ExecuteAsync(newSale);
                _view.ShowOperationResultSales(operationResult);

                if (operationResult.Errors.Count == 0)
                    _view.ClearSaleDetailsList();
            }
            catch
            {
                _view.ShowOperationResultSales(new OperationResult<SaleDTO>
                {
                    Errors = new List<ErrorLogDTO>
                    {
                        new ErrorLogDTO { Code = "EXCEPTION", Message = "Un error inesperado surgió durante la creación de la venta. Se sugiere reiniciar el programa." }
                    }
                });
            }
        }

        private OperationResult<T> CreateError<T>(string code, string message) where T : IDto
        {
            return new OperationResult<T>
            {
                Errors = new List<ErrorLogDTO> { new ErrorLogDTO { Code = code, Message = message } }
            };
        }
    }
}