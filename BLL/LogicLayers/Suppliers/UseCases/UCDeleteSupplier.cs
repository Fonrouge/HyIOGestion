using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.BaseContracts;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Exceptions.Base;
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

namespace BLL.LogicLayers.Suppliers
{
    public class UCDeleteSupplier : IUCDeleteSupplier
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;

        private readonly string _tableNameSupplier;

        public UCDeleteSupplier
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

                // 2. Validar Permisos 
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.SUPPLIER_DELETE.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());
                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameSupplier)));
                    return result;
                }


                // 3. Conexión a Base de Datos de Negocio
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();


                // 4. Buscar Entidad a Eliminar
                Supplier entity = await _uow.SupplierRepo.GetByIdAsync(dto.Id);

                if (entity == null)
                {
                    result.Errors.Add(new ErrorLogDTO { InformativeMessage = "El proveedor no existe o ya fue eliminado." });
                    await _uow.RollbackAsync();
                    return result;
                }

                // 5. Acción Principal: Eliminación (Soft Delete)
                if (entity is ISoftDeletable)
                {
                    entity.MarkAsDeleted();
                    await _uow.SupplierRepo.UpdateAsync(entity);
                }
                else
                {
                    await _uow.SupplierRepo.DeleteAsync(dto.Id);
                }

                // 6. Integridad Vertical (DVV)
                // Si al final decidís que Proveedores NO usa DVV, borrá esta línea y el método privado abajo:
          //      await UpdateDVVAsync(_tableNameSupplier, _appSettings.EntitiesConnection);

                // 7. Auditoría (Bitácora)
                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.DeleteOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameSupplier,
                    extraInfo: $"Se eliminó el proveedor ID: {dto.Id} (Nombre Ref: {dto.CompanyName})"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                // 8. Confirmación
                await _uow.CommitAsync();

                // 9. Retorno
                result.Value = dto;
                return result;
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                // --- LOG TÉCNICO INTERNO ---
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameSupplier;
                try { await _errorsRepository.CreateAsync(dbError); } catch { }

                // --- MANEJO DE RESTRICCIONES ---
                ErrorLog uiError;

                if (ex.Message.Contains("REFERENCE constraint") || ex.Message.Contains("FK_"))
                {
                    uiError = _errorsFactory.Create(ErrorCatalogEnum.DeleteRestriction, _tableNameSupplier);
                }
                else
                {
                    uiError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNameSupplier);
                }

                // --- MAPEO A DTO ---
                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id;
                errorDto.InformativeMessage = $"Falla técnica al eliminar proveedor. Ref ID: {dbError.Id}";

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