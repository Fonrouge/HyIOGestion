using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.Infrastructure.Audit;
using Domain.Repositories;
using Shared;
using Shared.Services;
using Shared.Sessions;
using System;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Clients //=======================================================================REFACTORIZADO AL 27/02=======================================================================
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

                // 2. Conexión a Base de Datos de Negocio
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                // 3. Validar Permisos
                var currentUser = await _uow.UserRepo.GetById(_sessionProvider.Current.CurrentUserId);
                if (!currentUser.HasPermission("CLIENT_UPDATE")) // Patente específica
                {
                    var authError = _errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameClient);
                    result.Errors.Add(ErrorMapper.ToDTO(authError));
                    return result;
                }

                // 4. Validaciones de Negocio
                if (string.IsNullOrWhiteSpace(dto.TaxId) || string.IsNullOrWhiteSpace(dto.Name))
                {
                    // Como este es un error de input directo, lo podemos armar a mano o agregarlo al catálogo como 'InvalidInput'
                    result.Errors.Add(new ErrorLogDTO { Message = "El Nombre y el Documento/CUIT son obligatorios para actualizar." });
                    return result;
                }

                // --- TRAMPA DEL UPDATE: Validación de Duplicados ---
                var existingClientWithTaxId = await _uow.ClientRepo.GetByTaxIdAsync(dto.TaxId);
                // Si el CUIT existe y NO es el del cliente que estoy editando, entonces es un duplicado ilegal.
                if (existingClientWithTaxId != null && existingClientWithTaxId.Id != dto.Id)
                {
                    var dupError = _errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameClient);
                    result.Errors.Add(ErrorMapper.ToDTO(dupError));
                    return result;
                }

                // 5. Mapeo a Entidad (Recordamos que tu EntityBase ya no requiere que toquemos el ID, el DTO ya lo trae)
                var clientEntityToUpdate = ClientMapper.ToEntity(dto);

           //     // OPCIONAL SI CRECE: Integridad Horizontal (DVH): CRÍTICO recalcularlo porque los datos cambiaron
           //     clientEntityToUpdate.DVH = IntegrityService.GetIntegrityHash
           //     (
           //         clientEntityToUpdate.Id,
           //         clientEntityToUpdate.TaxId,
           //         clientEntityToUpdate.Name
           //     );

                // 6. Persistencia
                await _uow.ClientRepo.Update(clientEntityToUpdate);

                // 7. Integridad Vertical (DVV)
                await UpdateDVVAsync(_tableNameClient, _appSettings.EntitiesConnection);

                // 8. Auditoría (Bitácora)
                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.UpdateOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameClient,
                    extraInfo: $"Se actualizó el cliente ID: {clientEntityToUpdate.Id} (Nuevo TaxId: {clientEntityToUpdate.TaxId})"
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