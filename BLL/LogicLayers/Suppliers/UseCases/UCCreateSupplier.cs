using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.Infrastructure.Audit;
using Domain.Repositories;
using Shared;
using Shared.Services;
using Shared.Sessions;
using System;
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
            _tableNameSupplier = appSettings.SupplierTableName ?? "Supplier";
        }

        // Corregido: La interfaz pedía ExecuteAsync
        public async Task<OperationResult<SupplierDTO>> ExecuteAsync(SupplierDTO dto)
        {
            var opRes = new OperationResult<SupplierDTO>();

            try
            {
                // 1. Validar Sesión Activa (Fail Fast)
                if (_sessionProvider.Current == null)
                {
                    var newError = _errorsFactory.Create(ErrorCatalogEnum.SessionExpired);
                    opRes.Errors.Add(new ErrorLogDTO
                    {
                        Code = newError.Code,
                        Message = newError.Message,
                        RecommendedAction = newError.RecommendedAction
                    });
                    return opRes;
                }

                // 2. Configurar conexión y abrir transacción
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                // 3. Validar Usuario y Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);

                if (!currentUser.HasPermission("SUPPLIER_CREATE")) // Patente específica
                {
                    var authError = _errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameSupplier);
                    opRes.Errors.Add(new ErrorLogDTO
                    {
                        Code = authError.Code,
                        Message = authError.Message,
                        RecommendedAction = authError.RecommendedAction
                    });
                    return opRes;
                }

                // 4. Validar Reglas de Negocio Básicas
                if (string.IsNullOrWhiteSpace(dto.TaxId) || string.IsNullOrWhiteSpace(dto.CompanyName))
                {
                    opRes.Errors.Add(new ErrorLogDTO { Message = "La Razón Social (CompanyName) y el CUIT/RUT (TaxId) son obligatorios." });
                    return opRes;
                }

                // Validación de Duplicados (Requiere que agregues GetByTaxIdAsync en ISupplierRepository, igual que en Client)
                var existingSupplier = await _uow.SupplierRepo.GetByTaxIdAsync(dto.TaxId);
                if (existingSupplier != null)
                {
                    var dupError = _errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameSupplier);
                    opRes.Errors.Add(new ErrorLogDTO
                    {
                        Code = dupError.Code,
                        Message = dupError.Message,
                        RecommendedAction = dupError.RecommendedAction
                    });
                    return opRes;
                }

                // 5. Mapeo a Entidad (KISS)
                var newSupplierEntity = SupplierMapper.ToEntity(dto);

                // Si Supplier tuviera DVH en tu modelo de dominio, lo calcularías aquí:
                // newSupplierEntity.DVH = IntegrityService.GetIntegrityHash(newSupplierEntity.Id, newSupplierEntity.TaxId, newSupplierEntity.CompanyName);

                // 6. Persistencia principal
                await _uow.SupplierRepo.CreateAsync(newSupplierEntity);

                // 7. Integridad Vertical (DVV)
                await UpdateDVVAsync(_tableNameSupplier, _appSettings.EntitiesConnection);

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
                opRes.Value = SupplierMapper.ToDto(newSupplierEntity);
                return opRes;
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
                opRes.Errors.Add(new ErrorLogDTO
                {
                    Code = uiError.Code,
                    Message = uiError.Message,
                    RecommendedAction = uiError.RecommendedAction
                });

                return opRes;
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