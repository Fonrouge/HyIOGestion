using Domain.Entities;
using Domain.Infrastructure;
using Domain.Repositories;
using Shared;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private SqlConnection _connection;
        private SqlTransaction _transaction;
        private string _connectionString;
        private bool _initialized = false;
        private IApplicationSettings _appSettings { get; }

        public IEmployeeRepository EmployeeRepo { get; }
        public IIntegrityVerifierRepository IntegrityRepo { get; }
        public IPermisoRepository PermisoRepo { get; }
        public IUserRepository UserRepo { get; }
        public IBitacoraRepository BitacoraRepo { get; }
        public IErrorsRepository LogEntryRepo { get; }
        public IClientRepository ClientRepo { get; }
        public IPaymentRepository PaymentRepo { get; }
        public IProductRepository ProductRepo { get; }
        public ICategoryRepository CategoryRepo { get; }
        public ISaleRepository SaleRepo { get; }
        public ISaleDetailRepository SaleDetailRepo { get; }
        public ISupplierRepository SupplierRepo { get; }

        public UnitOfWork
        (
            IClientRepository clientRepo,
            IEmployeeRepository employeeRepo,
            IPaymentRepository paymentRepo,
            IProductRepository productRepo,
            ICategoryRepository categoryRepo,
            ISaleRepository saleRepo,
            ISaleDetailRepository saleDetailRepo,
            ISupplierRepository supplierRepo,
            IIntegrityVerifierRepository integrityRepo,
            IPermisoRepository permisoRepo,
            IUserRepository userRepo,
            IBitacoraRepository bitacoraRepo,
            IErrorsRepository logEntryRepo,
            IApplicationSettings appsettings
        )
        {
            ClientRepo = clientRepo ?? throw new ArgumentNullException(nameof(clientRepo));
            EmployeeRepo = employeeRepo ?? throw new ArgumentNullException(nameof(employeeRepo));
            PaymentRepo = paymentRepo ?? throw new ArgumentNullException(nameof(paymentRepo));
            ProductRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));
            CategoryRepo = categoryRepo ?? throw new ArgumentNullException(nameof(categoryRepo));
            SaleRepo = saleRepo ?? throw new ArgumentNullException(nameof(saleRepo));
            SaleDetailRepo = saleDetailRepo ?? throw new ArgumentNullException(nameof(saleDetailRepo));
            SupplierRepo = supplierRepo ?? throw new ArgumentNullException(nameof(supplierRepo));
            IntegrityRepo = integrityRepo ?? throw new ArgumentNullException(nameof(integrityRepo));
            PermisoRepo = permisoRepo ?? throw new ArgumentNullException(nameof(permisoRepo));
            UserRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            BitacoraRepo = bitacoraRepo ?? throw new ArgumentNullException(nameof(bitacoraRepo));
            LogEntryRepo = logEntryRepo ?? throw new ArgumentNullException(nameof(logEntryRepo));
            _appSettings = appsettings ?? throw new ArgumentNullException(nameof(appsettings));
        }

        public bool HasActiveTransaction
        {
            get
            {
                // Excelente validación del estado de la transacción
                return _transaction != null && _transaction.Connection != null;
            }
        }

        public void SetConnectionString(string connectionString)
        {
            _initialized = true;
            _connectionString = connectionString;
        }

        public async Task BeginTransactionAsync()
        {
            if (!_initialized)
                throw new InvalidOperationException("ConnectionString no configurada. Use el método 'SetConnectionString' primero.");

            _connection = new SqlConnection(_connectionString);

            // ASINCRONISMO REAL AL ABRIR LA CONEXIÓN
            await _connection.OpenAsync();
            _transaction = _connection.BeginTransaction();

            // Inyectamos la transacción a todos los repositorios
            ClientRepo.SetTransaction(_transaction);
            EmployeeRepo.SetTransaction(_transaction);
            PaymentRepo.SetTransaction(_transaction);
            ProductRepo.SetTransaction(_transaction);
            CategoryRepo.SetTransaction(_transaction);
            SaleRepo.SetTransaction(_transaction);
            SaleDetailRepo.SetTransaction(_transaction);
            SupplierRepo.SetTransaction(_transaction);
            BitacoraRepo.SetTransaction(_transaction);
            IntegrityRepo.SetTransaction(_transaction);
            PermisoRepo.SetTransaction(_transaction);
            UserRepo.SetTransaction(_transaction);
            LogEntryRepo.SetTransaction(_transaction);
        }

        public async Task CommitAsync()
        {
            if (!_initialized)
                throw new InvalidOperationException("ConnectionString no configurada. Use el método 'SetConnectionString' primero.");

            if (HasActiveTransaction)
            {
                await Task.Run(() => _transaction.Commit());
            }
        }

        public async Task RollbackAsync()
        {
            if (!_initialized)
                throw new InvalidOperationException("ConnectionString no configurada. Use el método 'SetConnectionString' primero.");

            if (HasActiveTransaction)
            {
                await Task.Run(() => _transaction.Rollback());
            }
        }

        public string GetEntitiesConnectionString() => _appSettings.EntitiesConnection;

        public void Dispose()
        {
            _transaction?.Dispose();

            if (_connection?.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }

            _connection?.Dispose();
        }
    }
}