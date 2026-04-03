using BLL.DTOs;
using Presenter.ForClient;
using Presenter.ForEmployee;
using Presenter.ForPayments;
using Presenter.ForProducts;
using Presenter.ForSale;
using Presenter.ForSupplier;
using Presenter.Presenters.ForClient;
using Presenter.Presenters.ForEmployee;
using Presenter.Presenters.ForProducts;
using Presenter.Presenters.ForSupplier;
using Shared.ArchitecturalMarkers;

namespace WinformsUI.Infrastructure.Factories
{
    public interface IFormsFactory
    {
        IHostFormActions CreateHFA(string Title, IAppEnvironment Environment);
        T CreateGeneric<T>(params object[] parameters) where T : class;


        //For client
        T ClientForm<T>() where T : IClientView;
        T ClientCreationForm<T>() where T : ICreateClientView;
        T ClientUpdateForm<T>() where T : IUpdateClientView;


        //For employees
        T EmployeeForm<T>() where T : IEmployeeView;
        ICreateEmployeeView EmployeeCreationForm();
        T EmployeeUpdateForm<T>() where T : IUpdateEmployeeView;


        //For Payments
        T PaymentForm<T>() where T : IPaymentView;
        ICreatePaymentView PaymentCreationForm();


        //For Sales
        T SaleForm<T>() where T : ISaleView;
        ICreateSaleView SaleCreationForm();


        //For Products
        T ProductForm<T>() where T : IProductView;
        ICreateProductView ProductCreationForm();
        T ProductUpdateForm<T>() where T : IUpdateProductView;

        //For Supplier
        T SupplierForm<T>() where T : ISupplierView;
        ICreateSupplierView SupplierCreationForm();
        T SupplierUpdateForm<T>() where T : IUpdateSupplierView;
    }
}
