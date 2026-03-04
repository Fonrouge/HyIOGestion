using Microsoft.Extensions.DependencyInjection;
using Presenter.ForClient;
using Presenter.ForEmployee;
using Presenter.ForProducts;
using Presenter.ForSale;
using Shared.ArchitecturalMarkers; // Asumiendo que aquí están las interfaces de View
using System;
using Presenter.ForPayments;
using Presenter.ForSupplier;

namespace WinformsUI.Infrastructure.Factories
{
    public class FormsFactory : IFormsFactory
    {
        private readonly IServiceProvider _sp;

        public FormsFactory(IServiceProvider sp)
        {
            _sp = sp ?? throw new ArgumentNullException(nameof(sp));
        }

        public IHostFormActions CreateHFA(string title, IAppEnvironment environment)
        {
            var hf = _sp.GetRequiredService<IHostFormActions>();
            hf.SetTitle(title);
            hf.Initialize(environment);
            return hf;
        }

        public T CreateGeneric<T>(params object[] parameters) where T : class
        {
            if (parameters == null || parameters.Length == 0)
            {
                return _sp.GetRequiredService<T>();
            }

            return ActivatorUtilities.CreateInstance<T>(_sp, parameters);
        }

        // ==============================================================
        // MÓDULO CLIENTES
        // ==============================================================
        public T ClientForm<T>() where T : IClientView
        {
            // 1. Obtenemos la Vista desde el DI (ya que Form suele registrarse)
            var view = _sp.GetRequiredService<T>();

            ActivatorUtilities.CreateInstance<ClientPresenter>(_sp, view);

            return view;
        }


        public T ClientCreationForm<T>(IClientView parentView) where T : ICreateClientView
        {
            var creationView = _sp.GetRequiredService<T>();

            ActivatorUtilities.CreateInstance<CreateClientPresenter>(_sp, creationView);

            return creationView;
        }

        // ==============================================================
        // MÓDULO EMPLEADOS
        // ==============================================================
        public T EmployeeForm<T>() where T : IEmployeeView
        {
            var view = _sp.GetRequiredService<T>();

            ActivatorUtilities.CreateInstance<EmployeePresenter>(_sp, view);

            return view;
        }

        public ICreateEmployeeView EmployeeCreationForm()
        {
            var newCreationForm = _sp.GetRequiredService<ICreateEmployeeView>();
            ActivatorUtilities.CreateInstance<CreateEmployeePresenter>(_sp, newCreationForm);

            return newCreationForm;
        }



        // ==============================================================
        // MÓDULO VENTAS
        // ==============================================================
        public T SaleForm<T>() where T : ISaleView
        {
            // 1. Obtenemos la Vista desde el DI (ya que Form suele registrarse)
            var view = _sp.GetRequiredService<T>();

            ActivatorUtilities.CreateInstance<SalePresenter>(_sp, view);

            return view;
        }

        public ICreateSaleView SaleCreationForm(ISaleView view)
        {
            var newCreationForm = _sp.GetRequiredService<ICreateSaleView>();

            ActivatorUtilities.CreateInstance<CreateSalePresenter>(_sp, newCreationForm);

            return newCreationForm;
        }



        // ==============================================================
        // MÓDULO PAGOS
        // ==============================================================
        public ICreatePaymentView PaymentCreationForm(IPaymentView view)
        {
            var newCreationForm = _sp.GetRequiredService<ICreatePaymentView>();

            ActivatorUtilities.CreateInstance<CreatePaymentPresenter>(_sp, view);

            return newCreationForm;
        }

        public T PaymentForm<T>() where T : IPaymentView
        {
            var view = _sp.GetRequiredService<T>();

            ActivatorUtilities.CreateInstance<PaymentPresenter>(_sp, view);

            return view;
        }



        // ==============================================================
        // MÓDULO PRODUCTOS
        // ==============================================================
        public ICreateProductView ProductCreationForm(IProductView view)
        {
            var newCreationForm = _sp.GetRequiredService<ICreateProductView>();

            ActivatorUtilities.CreateInstance<CreateProductPresenter>(_sp, newCreationForm);

            return newCreationForm;
        }

        public T ProductForm<T>() where T : IProductView
        {
            var view = _sp.GetRequiredService<T>();

            ActivatorUtilities.CreateInstance<ProductPresenter>(_sp, view);

            return view;
        }



        // ==============================================================
        // MÓDULO PROVEEDORES
        // ==============================================================
        public T SupplierForm<T>() where T : ISupplierView
        {
            var view = _sp.GetRequiredService<T>();

            ActivatorUtilities.CreateInstance<SupplierPresenter>(_sp, view);

            return view;
        }

        public ICreateSupplierView SupplierCreationForm(ISupplierView view)
        {
            var newCreationForm = _sp.GetRequiredService<ICreateSupplierView>();
            
            ActivatorUtilities.CreateInstance<CreateSupplierPresenter>(_sp, newCreationForm);

            return newCreationForm;
        }


    }
}