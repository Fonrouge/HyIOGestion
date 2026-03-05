using BLL.LogicLayers.Products.Categories.UseCases;
using Microsoft.Extensions.DependencyInjection;
using Presenter.ForClient;
using Presenter.ForEmployee;
using Presenter.ForPayments;
using Presenter.ForProducts;
using Presenter.ForSale;
using Presenter.ForSupplier;
using Presenter.HostFormActions;
using Presenter.LoginScreen;
using Presenter.MainFormNavigation;
using Shared;
using Shared.ArchitecturalMarkers;
using Shared.Factories;
using Shared.Services.Searching;
using WinformsUI.Forms.ClientCRUDL;
using WinformsUI.Forms.ConfigurationsMenu;
using WinformsUI.Forms.EmployeeCRUDL;
using WinformsUI.Forms.Login;
using WinformsUI.Forms.PaymentCRUDL;
using WinformsUI.Forms.ProductCRUDL;
using WinformsUI.Forms.SaleCRUDL;
using WinformsUI.Forms.SupplierCRUDL;
using WinformsUI.Forms.Base;
using WinformsUI.Forms.Main;
using WinformsUI.Infrastructure.Culture;
using WinformsUI.Infrastructure.Factories;
using WinformsUI.Infrastructure.Localization;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.Infrastructure.UserInterface.Windowing;
using WinformsUI.UserControls.CustomDGV;
using WinformsUI.UserControls.Wizard;

namespace WinformsUI.Infrastructure.DI
{
    public static class ServiceCollectionForUI
    {
        public static IServiceCollection AddUILayer(this IServiceCollection services)
        {
            
            services.AddTransient<IAppEnvironment, AppEnvironment>();
            services.AddSingleton<IAppEnvironmentFactory, AppEnvironmentFactory>();

            services.AddSingleton<IFormsFactory, FormsFactory>();
            services.AddSingleton<ILayoutStrategyFactory, LayoutStrategyFactory>();

            services.AddTransient<IHostFormActions, HostForm>();
              
            services.AddTransient<IWizardPanelNavigator, WizardPanelNavigator>();
            

            services.AddSingleton<MainForm>();
            services.AddSingleton<IMainFormNavigation>(provider => provider.GetRequiredService<MainForm>());


            services.AddTransient<LoginFrm>();            
            services.AddTransient<ILoginView, LoginFrm>();
            
            services.AddTransient<ICustomDGVFactory, CustomDGVFactory>();

            
            services.AddTransient<IListFilterSortProvider, ListFilterSortProvider>();
            services.AddSingleton<ITranslatableControlsManager, TranslationsManager>();
            services.AddSingleton<ILocalizationService, LocalizationService>();
            services.AddSingleton<ICultureSwitcher, CultureProfile>(d =>
            {
                var appSettings = d.GetRequiredService<IApplicationSettings>();
                return new CultureProfile(appSettings.DefaultLanguage);                                          
            });


            return services;
        }

        public static IServiceCollection AddFromPresenter(this IServiceCollection services)
        {
            //Infrastructure
            services.AddSingleton<MainFormNavigationPresenter>();
            services.AddSingleton<HostFormActionsPresenter>();


            //Login
            services.AddSingleton<LoginPresenter>();


            //Employees
            services.AddTransient<IEmployeeView, EmployeeForm>();
            services.AddTransient<EmployeePresenter>();

            services.AddTransient<ICreateEmployeeView, CreateEmployeeForm>();
            services.AddTransient<CreateClientPresenter>();


            //Clients
            services.AddTransient<ICreateClientView, CreateClientForm>();
            services.AddTransient<CreateClientPresenter>();

            services.AddTransient<IClientView, ClientForm>();
            services.AddTransient<ClientPresenter>();


            //Sales
            services.AddTransient<ISaleView, SaleForm>();
            services.AddTransient<SalePresenter>();

            services.AddTransient<ICreateSaleView, CreateSaleForm>();
            services.AddTransient<CreateSalePresenter>();


            //Payments
            services.AddTransient<ICreatePaymentView, CreatePaymentForm>();
            services.AddTransient<CreatePaymentPresenter>();

            services.AddTransient<IPaymentView, PaymentForm>();
            services.AddTransient<PaymentPresenter>();


            //Products
            services.AddTransient<ICreateProductView, CreateProductForm>();
            services.AddTransient<CreateProductPresenter>();

            services.AddTransient<IProductView, ProductForm>();           
            services.AddTransient<ProductPresenter>();

            
            //Suppliers
            services.AddTransient<ICreateSupplierView, CreateSupplierForm>();
            services.AddTransient<CreateProductPresenter>();

            services.AddTransient<ISupplierView, SupplierForm>();
            services.AddTransient<SupplierPresenter>();


            //Tools
            services.AddTransient<CustomDGVForm>();
            services.AddTransient<CustomDGVForm>();
            services.AddTransient<ConfigurationsForm>();

            return services;
        }
    }
}
