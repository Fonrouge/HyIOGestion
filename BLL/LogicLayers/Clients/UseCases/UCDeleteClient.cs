using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.BaseContracts;
using Domain.Exceptions;
using Domain.Exceptions.Base;
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

                // 2. Conexión a Base de Datos de Negocio
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                // 3. Validar Permisos
                var currentUser = await _uow.UserRepo.GetById(_sessionProvider.Current.CurrentUserId);
                if (!currentUser.HasPermission("CLIENT_DELETE")) // Patente específica
                {
                    var authError = _errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameClient);
                    result.Errors.Add(ErrorMapper.ToDTO(authError));
                    return result;
                }

                // 4. Acción Principal: Eliminación (Hard o Soft, el repo decide internamente)
                await _uow.ClientRepo.Delete(dto.Id);

                // 5. Integridad Vertical (DVV): Obligatorio porque la tabla perdió/modificó una fila
                await UpdateDVVAsync(_tableNameClient, _appSettings.EntitiesConnection);

                // 6. Auditoría (Bitácora)
                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.DeleteOnBD, // Asumo que tienes algo así en tu Enum
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameClient,
                    extraInfo: $"Se eliminó el cliente ID: {dto.Id} (Nombre Ref: {dto.Name})"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                // 7. Confirmación
                await _uow.CommitAsync();

                // 8. Retorno (Devolvemos el mismo DTO que nos pasaron para indicar qué se borró)
                result.Value = dto;
                return result;
            }

            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                // --- LOG TÉCNICO INTERNO ---
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameClient;
                try { await _errorsRepository.CreateAsync(dbError); } catch { }

                // --- MANEJO DE RESTRICCIONES ---
                // Si la BD tira un error porque el cliente tiene ventas asociadas (Foreign Key constraint 547 en SQL Server),
                // ErrorCatalogEnum.DeleteRestriction es PERFECTO para esto.
                ErrorLog uiError;

                if (ex.Message.Contains("REFERENCE constraint") || ex.Message.Contains("FK_"))
                {
                    uiError = _errorsFactory.Create(ErrorCatalogEnum.DeleteRestriction, _tableNameClient);
                }
                else
                {
                    uiError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNameClient);
                }

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