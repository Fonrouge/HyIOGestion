using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.DTOs.Mappers;
using BLL.Infrastructure.Errors;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.Infrastructure.Permisos.Concrete;
using Domain.Repositories;
using Shared;
using Shared.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Sales
{
    public class UCGetAllSales : IUCGetAllSales
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;
        private readonly ISessionProvider _sessionProvider;

        private readonly string _tableNameSale;

        public UCGetAllSales
        (
            IUnitOfWork uow,
            IApplicationSettings appSettings,
            IErrorsFactory errorsFactory,
            IErrorsRepository errorsRepository,
                   ISessionProvider sessionProvider
        )
        {
            _uow = uow;
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _errorsFactory = errorsFactory ?? throw new ArgumentNullException(nameof(errorsFactory));
            _errorsRepository = errorsRepository ?? throw new ArgumentNullException(nameof(errorsRepository));
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
        }

        public async Task<(IEnumerable<SaleDTO>, OperationResult<SaleDTO>)> ExecuteAsync()
        {
            var result = new OperationResult<SaleDTO>();
            var salesListDto = new List<SaleDTO>();

            try
            {
                // 1. Validar Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.SALE_VIEW.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());
                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameSale)));
                    return (salesListDto, result);
                }

                // Setear conexión y empezar transacción
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                var sales = await _uow.SaleRepo.GetAllAsync();

                salesListDto = sales.Select(s => SaleMapper.ToDto(s)).ToList();

                foreach (var sale in salesListDto)
                {
                    sale.Employee = EmployeeMapper.ToDto(await _uow.EmployeeRepo.GetByIdAsync(sale.EmployeeId));
                    sale.Client = ClientMapper.ToDto(await _uow.ClientRepo.GetByIdAsync(sale.ClientId));
                }

                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction)
                {
                    await _uow.RollbackAsync();
                }

                // 1. Log técnico interno (Stacktrace real y excepción para soporte)
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _appSettings.SaleTableName ?? "Sales";

                try
                {
                    await _errorsRepository.CreateAsync(dbError);
                }
                catch { /* Falla silenciosa si no se puede escribir el log */ }

                // 2. Usamos el catálogo para armar la respuesta amigable
                var uiError = _errorsFactory.Create(ErrorCatalogEnum.DataLoadError, _appSettings.SaleTableName);

                // 3. Mapeamos a DTO y le inyectamos el ID de seguimiento real
                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id; // ¡Clave para soporte técnico!
                errorDto.InformativeMessage = $"Falla técnica. Reference ID: {dbError.Id}"; // Opcional

                result.Errors.Add(errorDto);

            }

            return (salesListDto, result);
        }
    }
}