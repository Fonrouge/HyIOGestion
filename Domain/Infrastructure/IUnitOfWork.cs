using Domain.Entities;
using Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace Domain.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRepository EmployeeRepo { get; }
        IIntegrityVerifierRepository IntegrityRepo { get; }
        IPermisoRepository PermisoRepo { get; }
        IUserRepository UserRepo { get; }
        IBitacoraRepository BitacoraRepo { get; }
        IErrorsRepository LogEntryRepo { get; }
        IClientRepository ClientRepo { get; }
        IPaymentRepository PaymentRepo { get; }
        IProductRepository ProductRepo { get; }
        ICategoryRepository CategoryRepo { get; } // Añadido
        ISaleRepository SaleRepo { get; }
        ISaleDetailRepository SaleDetailRepo { get; } // Añadido
        ISupplierRepository SupplierRepo { get; }

        bool HasActiveTransaction { get; }
        void SetConnectionString(string connectionString);
        string GetEntitiesConnectionString();

        // Pasamos la orquestación a métodos asíncronos
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}