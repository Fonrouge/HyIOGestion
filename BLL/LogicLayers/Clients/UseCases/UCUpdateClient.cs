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

namespace BLL.LogicLayers.Clients //=======================================================================REFACTORIZADO AL 14/04=======================================================================
{
    public class UCUpdateClient : IUCUpdateClient
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;

        private readonly string _tableNameClient;
        private readonly Guid _logsCorrelationId;

        public UCUpdateClient
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

            _tableNameClient = appSettings.ClientTableName ?? "Clients";
            _logsCorrelationId = Guid.NewGuid();
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

                await _uow.BeginTransactionAsync();

                // 2. Validar Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.CLIENT_VIEW.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());
                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameClient)));
                    return result;
                }
                
                // 3. Verificar duplicidad de Nro de Documento, teniendo en cuenta que que "técnicamente" estaría siempre duplicado por este mismo registro.
                var existingClientWithTaxId = await _uow.ClientRepo.GetByDocNumberAsync(dto.DocNumber);

                // Si el CUIT existe y NO es el del cliente que se está editando, entonces es un duplicado ilegal.
                if (existingClientWithTaxId != null && existingClientWithTaxId.Id != dto.Id)
                {
                    var dupError = _errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameClient);
                    result.Errors.Add(ErrorMapper.ToDTO(dupError));
                    return result;
                }

                // 4. Mapeo a Entidad
                var clientEntityToUpdate = ClientMapper.ToEntity(dto);

                // 5. Se recalcula el hash a partir de los nuevos datos de la entidad.
                IntegrityFacade.RecalculateAndSetEntityDVH(clientEntityToUpdate);

                // 6. Persistencia
                await _uow.ClientRepo.UpdateAsync(clientEntityToUpdate);

                // 7. Integridad Vertical (DVV)
                await UpdateDVVAsync(_tableNameClient, _appSettings.EntitiesConnection);

                // 8. Auditoría (Bitácora)
                var log = _bitacoraFact.Create
                (
                    entry: BitacoraCatalogEnum.UpdateOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameClient,
                    sessionId: _sessionProvider.Current.Id,
                    correlationId: _logsCorrelationId,
                    extraInfo: $"Se actualizó el cliente ID: {clientEntityToUpdate.Id} (Documento N°: {clientEntityToUpdate.DocNumber})"
                );
                await _uow.BitacoraRepo.CreateAsync(log);
               
                // 9. Confirmación
                await _uow.CommitAsync();

                // 10. Retorno
                result.Value = ClientMapper.ToDto(clientEntityToUpdate);
                return result;
            }

            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                // Log técnico interno
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameClient;
                try { await _errorsRepository.CreateAsync(dbError); } catch { }

                // Error catalogado para el usuario
                var uiError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNameClient);
                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id;
                errorDto.InformativeMessage = $"Falla técnica al actualizar. Ref ID: {dbError.Id}";

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