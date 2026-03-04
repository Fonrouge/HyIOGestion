using Presenter.ForClient;
using Presenter.ForEmployee;
using Presenter.ForPayments;
using Presenter.ForProducts;
using Presenter.ForSale;
using Presenter.ForSupplier;
using Shared.ArchitecturalMarkers;

namespace WinformsUI.Infrastructure.Factories
{
    public interface IFormsFactory
    {
        IHostFormActions CreateHFA(string Title, IAppEnvironment Environment);
        T CreateGeneric<T>(params object[] parameters) where T : class;


        //For client
        T ClientForm<T>() where T : IClientView;
        T ClientCreationForm<T>(IClientView view) where T : ICreateClientView;


        //For employees
        T EmployeeForm<T>() where T : IEmployeeView;
        ICreateEmployeeView EmployeeCreationForm();


        //For Payments
        T PaymentForm<T>() where T : IPaymentView;
        ICreatePaymentView PaymentCreationForm(IPaymentView view);


        //For Sales
        T SaleForm<T>() where T : ISaleView;
        ICreateSaleView SaleCreationForm(ISaleView view);


        //For Products
        T ProductForm<T>() where T : IProductView;
        ICreateProductView ProductCreationForm(IProductView view);

        //For Supplier
        T SupplierForm<T>() where T : ISupplierView;
        ICreateSupplierView SupplierCreationForm(ISupplierView view);
    }
}
