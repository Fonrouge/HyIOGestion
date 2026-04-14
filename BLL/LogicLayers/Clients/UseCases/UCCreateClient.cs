using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
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

namespace BLL.LogicLayers.Clients  //=======================================================================REFACTORIZADO AL 27/02=======================================================================
{
    public class UCCreateClient : IUCCreateClient
    {
        private readonly IApplicationSettings _appSettings;
        private readonly IUnitOfWork _uow;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;

        private readonly string _tableNameClient;
        private readonly Guid _correlationId;


        public UCCreateClient
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
            _correlationId = Guid.NewGuid();
        }

        public async Task<OperationResult<ClientDTO>> ExecuteAsync(ClientDTO dto)
        {
            var opRes = new OperationResult<ClientDTO>();

            try
            {
                // 1. Validar Sesión Activa (Fail Fast)
                if (_sessionProvider.Current == null)
                {
                    var newError = _errorsFactory.Create(ErrorCatalogEnum.SessionExpired);

                    opRes.Errors.Add(ErrorMapper.ToDTO(newError));
                    return opRes;
                }


                // 2. Configurar conexión y abrir transacción
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();


                // 3. Validar Permisos (Ahora funcionará porque las queries son Cross-DB o usan SecurityConnection)
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.CLIENT_CREATE.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());

                if (!hasAccess)
                {
                    opRes.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameClient)));
                    return opRes;
                }

                // 4. Validar unicidad}
                var existingClient = await _uow.ClientRepo.GetByDocNumberAsync(dto.DocNumber);

                if (existingClient != null)
                {
                    var dupError = _errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameClient);
                    opRes.Errors.Add(ErrorMapper.ToDTO(dupError));
                    return opRes;
                }

                // 5. Mapeo a Entidad
                var newClientEntity = ClientMapper.ToEntity(dto);


                //  Podría optarse por incluir DVH de ser necesario

                newClientEntity.UpdateDVH(IntegrityService.GetIntegrityHash(newClientEntity.GetDvhSerialization()));


                // 6. Persistencia principal
                await _uow.ClientRepo.CreateAsync(newClientEntity);

                // 7. Integridad Vertical (DVV)
                //       await UpdateDVVAsync(_tableNameClient, _appSettings.EntitiesConnection);

                // 8. Registrar Bitácora
                var log = _bitacoraFact.Create
                (
                    entry: BitacoraCatalogEnum.CreateOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameClient,
                    sessionId: _sessionProvider.Current.Id,
                    correlationId: _correlationId,
                    extraInfo: $"Se creó el cliente {newClientEntity.Id} (N° Documento: {newClientEntity.DocNumber})"
                );


                await _uow.BitacoraRepo.CreateAsync(log);

                // 9. Confirmar Transacción
                await _uow.CommitAsync();

                // 10. Retornamos el DTO actualizado (ej. con su nuevo ID)
                opRes.Value = ClientMapper.ToDto(newClientEntity);
                return opRes;
            }

            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction)
                {
                    await _uow.RollbackAsync();
                }

                // Generamos error técnico para la BD y seguro para la UI
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameClient;

                try
                {
                    await _errorsRepository.CreateAsync(dbError);
                }
                catch { }


                // Emitimos error limpio a la UI
                var uiError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNameClient);

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
