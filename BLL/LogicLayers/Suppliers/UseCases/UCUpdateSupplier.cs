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

namespace BLL.LogicLayers.Suppliers
{
    public class UCUpdateSupplier : IUCUpdateSupplier
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;

        private readonly string _tableNameSupplier;

        public UCUpdateSupplier
        (
            IUnitOfWork uow,
            IApplicationSettings appSettings,
            IBitacoraFactory bitacoraFact,
            ISessionProvider sessionProvider,
            IErrorsFactory errorsFactory,
            IErrorsRepository errorsRepository
        )
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _bitacoraFact = bitacoraFact ?? throw new ArgumentNullException(nameof(bitacoraFact));
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
            _errorsFactory = errorsFactory ?? throw new ArgumentNullException(nameof(errorsFactory));
            _errorsRepository = errorsRepository ?? throw new ArgumentNullException(nameof(errorsRepository));

            _tableNameSupplier = appSettings.SupplierTableName ?? "Suppliers";
        }

        public async Task<OperationResult<SupplierDTO>> ExecuteAsync(SupplierDTO dto)
        {
            var result = new OperationResult<SupplierDTO>();

            try
            {
                // 1. Validar Sesión Activa
                if (_sessionProvider.Current == null)
                {
                    var newError = _errorsFactory.Create(ErrorCatalogEnum.SessionExpired);
                    result.Errors.Add(ErrorMapper.ToDTO(newError));
                    return result;
                }

                // 2. Conexión y Transacción
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                // 3. Validar Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                if (!currentUser.HasPermission("SUPPLIER_UPDATE"))
                {
                    var authError = _errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameSupplier);
                    result.Errors.Add(ErrorMapper.ToDTO(authError));
                    return result;
                }

                // 5. Validación de Duplicados (Trampa del Update)
                var existingSupplierWithTaxId = await _uow.SupplierRepo.GetByTaxIdAsync(dto.TaxId);
                if (existingSupplierWithTaxId != null && existingSupplierWithTaxId.Id != dto.Id)
                {
                    var dupError = _errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameSupplier);
                    result.Errors.Add(ErrorMapper.ToDTO(dupError));
                    return result;
                }

                // 6. Mapeo a Entidad
                var supplierEntityToUpdate = SupplierMapper.ToEntity(dto);

                // 7. Persistencia
                await _uow.SupplierRepo.UpdateAsync(supplierEntityToUpdate);

                // 8. Integridad Vertical (DVV)
                await UpdateDVVAsync(_tableNameSupplier, _appSettings.EntitiesConnection);

                // 9. Auditoría (Bitácora)
                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.UpdateOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameSupplier,
                    extraInfo: $"Se actualizó el proveedor ID: {supplierEntityToUpdate.Id} (CUIT: {supplierEntityToUpdate.TaxId})"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                // 10. Confirmación
                await _uow.CommitAsync();

                // 11. Retorno
                result.Value = SupplierMapper.ToDto(supplierEntityToUpdate);
                return result;
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                // Log técnico para soporte
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameSupplier;
                try { await _errorsRepository.CreateAsync(dbError); } catch { }

                // Error amigable para la UI
                var uiError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNameSupplier);
                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id;
                errorDto.InformativeMessage = $"Error técnico al actualizar proveedor. Ref ID: {dbError.Id}";

                result.Errors.Add(errorDto);
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