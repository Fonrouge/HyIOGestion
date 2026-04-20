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
using Shared.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BLL.LogicLayers.Clients //=======================================================================REFACTORIZADO AL 14/04=======================================================================
{
    public class UCGetAllClients : IUCGetAllClients
    {
        private readonly IUnitOfWork _uow;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;
        private readonly ISessionProvider _sessionProvider;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;

        private readonly string _tableNameClients;
        private readonly Guid _logsCorrelationId;

        public UCGetAllClients
        (
            IUnitOfWork uow,
            IApplicationSettings appSettings,
            IErrorsFactory errorsFactory,
            IErrorsRepository errorsRepository,
            ISessionProvider sessionProvider,
            IBitacoraFactory bitacoraFact
        )
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow)); ;
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings)); ;
            _errorsFactory = errorsFactory ?? throw new ArgumentNullException(nameof(errorsFactory)); ;
            _errorsRepository = errorsRepository ?? throw new ArgumentNullException(nameof(errorsRepository)); ;
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
            _bitacoraFact = bitacoraFact ?? throw new ArgumentNullException(nameof(bitacoraFact)); ;

            _tableNameClients = _appSettings.ClientTableName ?? "Clients";
            _logsCorrelationId = Guid.NewGuid();
        }

        public async Task<(IEnumerable<ClientDTO>, OperationResult<ClientDTO>)> ExecuteAsync()
        {
            var result = new OperationResult<ClientDTO>();
            var listDto = new List<ClientDTO>();

            try
            {
                // 1. Validar Sesión Activa (Fail Fast)
                if (_sessionProvider.Current == null)
                {
                    var newError = _errorsFactory.Create(ErrorCatalogEnum.SessionExpired);

                    result.Errors.Add(ErrorMapper.ToDTO(newError));
                    return (listDto, result);
                }


                // 2. Validar Permisos (Patente específica de Empleados)
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.CLIENT_VIEW.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());

                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameClients)));
                    return (listDto, result);

                }

                //3. Recién validados los permisos, se procede en caso de éxito a iniciar la conexión a BD.
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
               
                //4. Se obtiene el total de entidades y se mapea para UI.
                var clients = await _uow.ClientRepo.GetAllAsync();

                listDto = clients.Select(c => ClientMapper.ToDto(c)).ToList();

                //5. Se deja registro detallado de la acción
                var log = _bitacoraFact.Create
                (
                    entry: BitacoraCatalogEnum.GetAllFromDB,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameClients,
                    sessionId: _sessionProvider.Current.Id,
                    correlationId: _logsCorrelationId,
                    extraInfo: $"Se extrajeron todos los datos de la tabla {_tableNameClients} para su visualización."
                );

                await _uow.BitacoraRepo.CreateAsync(log);

                //Fuera del bloquie try-catch se devuelve una lista llena o vacía junto al OperationResult.
            }

            catch (Exception ex)
            {
                // 1. Log técnico interno
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameClients ?? "Clients";

                try
                {
                    await _errorsRepository.CreateAsync(dbError);
                }
                catch { }

                // 2. Error catalogado para el usuario
                var uiError = _errorsFactory.Create(ErrorCatalogEnum.DataLoadError, _appSettings.ClientTableName);

                // 3. Mapeo y vinculación de trazabilidad para soporte técnico
                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id;
                errorDto.InformativeMessage = $"Se falló en la obtención de los clientes. Si se comunica con soporte, utilice este código para una mejor asistencia ({dbError.Id}). Si no desea hacerlo, por favor reinicie el programa e intente nuevamente.";

                result.Errors.Add(errorDto);
            }

            return (listDto, result);
        }
    }
}