using BLL.AuditLogs;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using BLL.LogicLayer;
using BLL.LogicLayers;
using BLL.LogicLayers.Categories;
using BLL.LogicLayers.Clients;
using BLL.LogicLayers.Clients.UseCases;
using BLL.LogicLayers.Employees;
using BLL.LogicLayers.Payments;
using BLL.LogicLayers.Products;
using BLL.LogicLayers.Products.Categories.UseCases;
using BLL.LogicLayers.Sales;
using BLL.LogicLayers.Suppliers;
using BLL.LogicLayers.User.UseCases;
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
            services.AddScoped<IClientRepository, ClientRepository>();

            //Employee
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            //Payment
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            //Product
            services.AddScoped<IProductRepository, ProductRepository>();

            //Sale
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<ISaleDetailRepository, SaleDetailRepository>();

            //Supplier
            services.AddScoped<ISupplierRepository, SupplierRepository>();

            //Category
            services.AddScoped<ICategoryRepository, CategoryRepository>();


            //===================================
            //         INFRASTRUCTURE
            //===================================

            //INFRA - Security
            services.AddScoped<IErrorsFactory, ErrorsFactory>();
            services.AddScoped<IErrorsRepository, ErrorsRepository>();

            //Infra - Errors
            services.AddScoped<IBitacoraRepository, BitacoraRepository>();
            services.AddScoped<IIntegrityVerifierRepository, IntegrityVerifierRepository>();

            //Infra - Users
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPermisoRepository, PermisoRepository>();


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
            services.AddScoped<IVerifyDVV, VerifyDVV>();
            services.AddScoped<IVerifyDVH, VerifyDVH>();

            //INFRA - Registers
//            services.AddSingleton<IBitacoraMapper, BitacoraMapper>();
            services.AddSingleton<IBitacoraFactory, BitacoraFactory>();

            //Login (User) 
            services.AddScoped<IUCLogin, UCLogin>();
            services.AddScoped<IUCUpdateUser, UCUpdateUser>();
            services.AddScoped<IUCCreateUser, UCCreateUser>();
            services.AddScoped<IUCGetUserById, UCGetUserById>();


            //===================================
            //          FOR ENTITIES
            //===================================

            //Client
            services.AddScoped<IUCGetAllClients, UCGetAllClients>();
            services.AddScoped<IUCCreateClient, UCCreateClient>();
            services.AddScoped<IUCUpdateClient, UCUpdateClient>();
            services.AddScoped<IUCDeleteClient, UCDeleteClient>();
            services.AddScoped<IUCGetClientById, UCGetClientById>();
        

            //Employee
            services.AddScoped<IUCGetAllEmployees, UCGetAllEmployees>();
            services.AddScoped<IUCCreateEmployee, UCCreateEmployee>();
            services.AddScoped<IUCUpdateEmployee, UCUpdateEmployee>();
            services.AddScoped<IUCDeleteEmployee, UCDeleteEmployee>();


            //Payment
            services.AddScoped<IUCGetAllPayments, UCGetAllPayments>();
            services.AddScoped<IUCCreatePayment, UCCreatePayment>();
            services.AddScoped<IUCUpdatePayment, UCUpdatePayment>();
            services.AddScoped<IUCDeletePayment, UCDeletePayment>();


            //Products + it's categories
            services.AddScoped<IUCGetAllProducts, UCGetAllProducts>();
            services.AddScoped<IUCCreateProduct, UCCreateProduct>();
            services.AddScoped<IUCUpdateProduct, UCUpdateProduct>();
            services.AddScoped<IUCDeleteProduct, UCDeleteProduct>();
            
            services.AddScoped<IUCCreateCategory, UCCreateCategory>();
            services.AddScoped<IUCGetAllCategories, UCGetAllCategories>();
            services.AddScoped<IUCDeleteCategory, UCDeleteCategory>();

            //Sale
            services.AddScoped<IUCGetAllSales, UCGetAllSales>();
            services.AddScoped<IUCCreateSale, UCCreateSale>();
            services.AddScoped<IUCUpdateSale, UCUpdateSale>();
            services.AddScoped<IUCDeleteSale, UCDeleteSale>();

            //Suppliers
            services.AddScoped<IUCGetAllSuppliers, UCGetAllSuppliers>();
            services.AddScoped<IUCCreateSupplier, UCCreateSupplier>();
            services.AddScoped<IUCUpdateSupplier, UCUpdateSupplier>();
            services.AddScoped<IUCDeleteSupplier, UCDeleteSupplier>();
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
