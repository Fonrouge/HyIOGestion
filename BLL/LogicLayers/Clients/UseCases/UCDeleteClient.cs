using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Contracts;
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

namespace BLL.LogicLayers.Clients //=======================================================================REFACTORIZADO AL 27/02=======================================================================
{
    public class UCDeleteClient : IUCDeleteClient
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;

        private readonly string _tableNameClient;

        public UCDeleteClient
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

            _tableNameClient = appSettings.ClientTableName ?? "Client";
        }
        public async Task<OperationResult<ClientDTO>> ExecuteAsync(ClientDTO dto)
        {
            var result = new OperationResult<ClientDTO>();

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

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.CLIENT_DELETE.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());
                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameClient)));
                    return result;
                }

                // 3. Conexión a Base de Datos de Negocio y Transacción
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                // 4. Acción Principal: Buscar Entidad
                Client entity = await _uow.ClientRepo.GetByIdAsync(dto.Id);

                if (entity == null | entity.IsDeleted)
                {
                    // Manejo elegante si no se encuentra el registro
                    result.Errors.Add(new ErrorLogDTO { InformativeMessage = "El cliente no existe o ya fue eliminado." });
                    await _uow.RollbackAsync();
                    return result;
                }

                // 5. Eliminación (Hard o Soft) + ACTUALIZACIÓN DE DVH
                if (entity is ISoftDeletable)
                {
                    entity.MarkAsDeleted();
                    await _uow.ClientRepo.UpdateAsync(entity); // Sin calcular DVH, directo a guardar
                }
                else
                {
                    await _uow.ClientRepo.DeleteAsync(dto.Id);
                }

                // 6. Integridad Vertical (DVV): Obligatorio porque la tabla perdió/modificó una fila
   //             await UpdateDVVAsync(_tableNameClient, _appSettings.EntitiesConnection);

                // 7. Auditoría (Bitácora)
                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.DeleteOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameClient,
                    extraInfo: $"Se eliminó el cliente ID: {dto.Id} (Nombre Ref: {dto.Name})"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                // 8. Confirmación
                await _uow.CommitAsync();

                result.Value = dto;
                return result;
            }
            catch (Exception ex)
            {
                // El Catch está excelente, lo dejé tal cual lo tenías.
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                // --- LOG TÉCNICO INTERNO ---
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameClient;
                try { await _errorsRepository.CreateAsync(dbError); } catch { }

                // --- MANEJO DE RESTRICCIONES ---
                ErrorLog uiError;
                if (ex.Message.Contains("REFERENCE constraint") || ex.Message.Contains("FK_"))
                    uiError = _errorsFactory.Create(ErrorCatalogEnum.DeleteRestriction, _tableNameClient);
                else
                    uiError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNameClient);

                // --- MAPEO A DTO ---
                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id;
                errorDto.InformativeMessage = $"Falla técnica al eliminar. Ref ID: {dbError.Id}";

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