using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.Infrastructure.Audit;
using Domain.Infrastructure.Permisos.Concrete;
using Domain.Repositories;
using Shared;
using Shared.Services;
using Shared.Sessions;
using System;
using System.Linq;
using System.Threading.Tasks;

// Asumo que tu mapper está en un namespace similar a este:
// using BLL.DTOs.Mappers;

namespace BLL.LogicLayers.Suppliers
{
    public class UCCreateSupplier : IUCCreateSupplier
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;

        private readonly string _tableNameSupplier;

        public UCCreateSupplier(
            IUnitOfWork uow,
            IApplicationSettings appSettings,
            IBitacoraFactory bitacoraFact,
            ISessionProvider sessionProvider,
            IErrorsFactory errorsFactory,
            IErrorsRepository errorsRepository)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _bitacoraFact = bitacoraFact ?? throw new ArgumentNullException(nameof(bitacoraFact));
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
            _errorsFactory = errorsFactory ?? throw new ArgumentNullException(nameof(errorsFactory));
            _errorsRepository = errorsRepository ?? throw new ArgumentNullException(nameof(errorsRepository));

            // Asumimos que tienes esto en tus settings, o usamos un default
            _tableNameSupplier = appSettings.SupplierTableName ?? "Suppliers";
        }

        // Corregido: La interfaz pedía ExecuteAsync
        public async Task<OperationResult<SupplierDTO>> ExecuteAsync(SupplierDTO dto)
        {
            var result = new OperationResult<SupplierDTO>();

            try
            {
                // 1. Validar Sesión Activa (Fail Fast)
                if (_sessionProvider.Current == null)
                {
                    var newError = _errorsFactory.Create(ErrorCatalogEnum.SessionExpired);
                    result.Errors.Add(new ErrorLogDTO
                    {
                        Code = newError.Code,
                        Message = newError.Message,
                        RecommendedAction = newError.RecommendedAction
                    });
                    return result;
                }

                // 2. Configurar conexión y abrir transacción
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                // 3. Validar Usuario y Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.SUPPLIER_CREATE.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());
                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameSupplier)));
                    return result;
                }


                // 4. Validar Reglas de Negocio Básicas
                if (string.IsNullOrWhiteSpace(dto.TaxId) || string.IsNullOrWhiteSpace(dto.CompanyName))
                {
                    result.Errors.Add(new ErrorLogDTO { Message = "La Razón Social (CompanyName) y el CUIT/RUT (TaxId) son obligatorios." });
                    return result;
                }

                // Validación de Duplicado
                var existingSupplier = await _uow.SupplierRepo.GetByTaxIdAsync(dto.TaxId);
                if (existingSupplier != null)
                {
                    var dupError = _errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameSupplier);
                    result.Errors.Add(new ErrorLogDTO
                    {
                        Code = dupError.Code,
                        Message = dupError.Message,
                        RecommendedAction = dupError.RecommendedAction
                    });
                    return result;
                }

                // 5. Mapeo a Entidad (KISS)
                var newSupplierEntity = SupplierMapper.ToEntity(dto);

                // Futura implementación de DVH + DVV
                // newSupplierEntity.DVH = IntegrityService.GetIntegrityHash(newSupplierEntity.Id, newSupplierEntity.TaxId, newSupplierEntity.CompanyName);

                // 6. Persistencia principal
                await _uow.SupplierRepo.CreateAsync(newSupplierEntity);

                // 7. Integridad Vertical (DVV)
   //             await UpdateDVVAsync(_tableNameSupplier, _appSettings.EntitiesConnection);

                // 8. Registrar Bitácora
                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.CreateOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameSupplier,
                    extraInfo: $"Se creó el proveedor {newSupplierEntity.CompanyName} (TaxId: {newSupplierEntity.TaxId})"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                // 9. Confirmar Transacción
                await _uow.CommitAsync();

                // 10. Retornar DTO con el ID generado
                result.Value = SupplierMapper.ToDto(newSupplierEntity);
                return result;
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction)
                {
                    await _uow.RollbackAsync();
                }

                // Log técnico a BD
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameSupplier;
                try
                {
                    await _errorsRepository.CreateAsync(dbError);
                }
                catch { /* Ignoramos fallos al loguear para no romper el retorno */ }

                // Log limpio para la UI
                var uiError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNameSupplier);
                result.Errors.Add(new ErrorLogDTO
                {
                    Code = uiError.Code,
                    Message = uiError.Message,
                    RecommendedAction = uiError.RecommendedAction
                });

                return result;
            }
        }

        private async Task UpdateDVVAsync(string nombreTabla, string connectionString)
        {
            var hashes = await _uow.IntegrityRepo.GetVerticalHashesAsync(nombreTabla, connectionString);
            var dvvFinal = IntegrityService.CalculateDVV(hashes);
            await _uow.IntegrityRepo.UpdateDVVAsync(nombreTabla, dvvFinal, connectionString);
        }
    }
}