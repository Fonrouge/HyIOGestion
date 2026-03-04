using BLL.AuditLogs;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using BLL.LogicLayer;
using BLL.LogicLayers;
using BLL.LogicLayers.Clients;
using BLL.LogicLayers.Employees;
using BLL.LogicLayers.Payments;
using BLL.LogicLayers.Products;
using BLL.LogicLayers.Products.Categories.UseCases;
using BLL.LogicLayers.Sales;
using BLL.LogicLayers.Suppliers;
using BLL.SessionInfo;
using BLL.UseCases;
using DAL.Persistence;
using DAL.Persistence.MicrosoftSQL;
using Domain.Entities;
using Domain.Infrastructure;
using Domain.Repositories;
using Infrastructure.Persistence.Local;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using Shared.Services;
using Shared.Sessions;

namespace Bootstrapper
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            FromShared(services);
            FromDomainAndDal(services);
            FromBLL(services);

            return services;
        }


        private static void FromDomainAndDal(IServiceCollection services)
        {
            //===================================
            //   FOR ENTITIES (Interface from Domain - Implementation from DAL)
            //===================================

            //Client
            services.AddTransient<IClientRepository, ClientRepository>();

            //Employee
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();

            //Payment
            services.AddTransient<IPaymentRepository, PaymentRepository>();

            //Product
            services.AddTransient<IProductRepository, ProductRepository>();

            //Sale
            services.AddTransient<ISaleRepository, SaleRepository>();
            services.AddTransient<ISaleDetailRepository, SaleDetailRepository>();

            //Supplier
            services.AddTransient<ISupplierRepository, SupplierRepository>();

            //Category
            services.AddTransient<ICategoryRepository, CategoryRepository>();


            //===================================
            //         INFRASTRUCTURE
            //===================================

            //INFRA - Security
            services.AddTransient<IErrorsFactory, ErrorsFactory>();
            services.AddTransient<IErrorsRepository, ErrorsRepository>();

            //Infra - Errors
            services.AddTransient<IBitacoraRepository, BitacoraRepository>();
            services.AddTransient<IIntegrityVerifierRepository, IntegrityVerifierRepository>();

            //Infra - Users
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPermisoRepository, PermisoRepository>();


            //===================================
            //         FROM DAL
            //===================================

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }



        private static void FromBLL(IServiceCollection services)
        {

            //===================================
            //         INFRASTRUCTURE
            //===================================

            //INFRA - Sessions
            services.AddScoped<ISession, Session>();
            services.AddSingleton<ISessionFactory, SessionFactory>();
            services.AddSingleton<ISessionManager, SessionManager>();
            services.AddSingleton<ISessionProvider, GlobalSessionProvider>();

            //INFRA - Security
            services.AddTransient<IVerifyDVV, VerifyDVV>();
            services.AddTransient<IVerifyDVH, VerifyDVH>();

            //INFRA - Registers
            services.AddSingleton<IBitacoraMapper, BitacoraMapper>();
            services.AddSingleton<IBitacoraFactory, BitacoraFactory>();

            //Login (User) 
            services.AddTransient<IUCLogin, UCLogin>();
            services.AddTransient<IUCModifyUser, UCModifyUser>();
            services.AddTransient<IUCCreateUser, UCCreateUser>();


            //===================================
            //          FOR ENTITIES
            //===================================

            //Client
            services.AddTransient<IUCGetAllClients, UCGetAllClients>();
            services.AddTransient<IUCCreateClient, UCCreateClient>();
            services.AddTransient<IUCUpdateClient, UCUpdateClient>();
            services.AddTransient<IUCDeleteClient, UCDeleteClient>();


            //Employee
            services.AddTransient<IUCGetAllEmployees, UCGetAllEmployees>();
            services.AddTransient<IUCCreateEmployee, UCCreateEmployee>();
            services.AddTransient<IUCUpdateEmployee, UCUpdateEmployeeMOCK>();
            services.AddTransient<IUCDeleteEmployee, UCDeleteEmployeeMOCK>();


            //Payment
            services.AddTransient<IUCGetAllPayments, UCGetAllPayments>();
            services.AddTransient<IUCCreatePayment, UCCreatePaymentMOCK>();
            services.AddTransient<IUCUpdatePayment, UCUpdatePaymentMOCK>();
            services.AddTransient<IUCDeletePayment, UCDeletePaymentMOCK>();


            //Products + it's categories
            services.AddTransient<IUCGetAllProducts, UCGetAllProducts>();
            services.AddTransient<IUCCreateProduct, UCCreateProductMOCK>();
            services.AddTransient<IUCUpdateProduct, UCUpdateProductMOCK>();
            services.AddTransient<IUCDeleteProduct, UCDeleteProductMOCK>();

            services.AddTransient<IUCGetAllCategories, UCGetAllCategories>();

            //Sale
            services.AddTransient<IUCGetAllSales, UCGetAllSales>();
            services.AddTransient<IUCCreateSale, UCCreateSale>();
            services.AddTransient<IUCUpdateSale, UCUpdateSale>();
            services.AddTransient<IUCDeleteSale, UCDeleteSale>();

            //Suppliers
            services.AddTransient<IUCGetAllSuppliers, UCGetAllSuppliers>();
            services.AddTransient<IUCCreateSupplier, UCCreateSupplier>();
            services.AddTransient<IUCUpdateSupplier, UCUpdateSupplierMOCK>();
            services.AddTransient<IUCDeleteSupplier, UCDeleteSupplierMOCK>();
        }


        private static void FromShared(IServiceCollection services)
        {
            //Cross-Cutting services
            services.AddTransient<IApplicationSettings, ApplicationSettings>();
            services.AddTransient<IAesEncryptionService, AesEncryptionService>();
            services.AddTransient<IHashEncryptionService, HashEncryptionService>();
        }



    }
}
