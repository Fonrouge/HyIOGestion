using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.Errors;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.Infrastructure.Permisos.Concrete;
using Domain.Repositories;
using Shared;
using Shared.Sessions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Clients.UseCases //=======================================================================REFACTORIZADO AL 14/04=======================================================================
{

    public class UCGetClientById : IUCGetClientById
    {
        private readonly IUnitOfWork _uow;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;
        private readonly ISessionProvider _sessionProvider;
        private readonly IApplicationSettings _appSettings;

        private readonly string _tableNameClients;

        public UCGetClientById
        (
            IUnitOfWork uow,
            IApplicationSettings appSettings,
            IErrorsFactory errorsFactory,
            IErrorsRepository errorsRepository,
            ISessionProvider sessionProvider
        )
        {
            _uow = uow;
            _appSettings = appSettings;
            _errorsFactory = errorsFactory;
            _errorsRepository = errorsRepository;
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));

            _tableNameClients = _appSettings.ClientTableName;
        }

        public async Task<(ClientDTO, OperationResult<ClientDTO>)> ExecuteAsync(Guid id)
        {
            var opRes = new OperationResult<ClientDTO>();
            var clientDto = new ClientDTO();

            try
            {
                // 1. Validar Sesión Activa (Fail Fast)
                if (_sessionProvider.Current == null)
                {
                    var newError = _errorsFactory.Create(ErrorCatalogEnum.SessionExpired);

                    opRes.Errors.Add(ErrorMapper.ToDTO(newError));
                    return (clientDto, opRes);
                }


                // 2. Validar Permisos (Patente específica de Empleados)
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.CLIENT_VIEW.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());
                if (!hasAccess)
                {
                    opRes.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameClients)));
                    return (clientDto, opRes);

                }

                // 3. De tener permisos, recién ahí se conecta a BD.
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                
                var client = await _uow.ClientRepo.GetByIdAsync(id);
                clientDto = ClientMapper.ToDto(client);

            }

            catch (Exception ex)
            {
                // 1. Log técnico interno
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _appSettings.ClientTableName ?? "Client";

                try
                {
                    await _errorsRepository.CreateAsync(dbError);
                }
                catch { }

                // 2. Error catalogado para el usuario (CERO strings mágicos)
                var uiError = _errorsFactory.Create(ErrorCatalogEnum.DataLoadError, _appSettings.ClientTableName);

                // 3. Mapeo y vinculación de trazabilidad para soporte técnico
                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id; // ¡Clave para rastrear el fallo en la BD!
                errorDto.InformativeMessage = $"Falla técnica. Reference ID: {dbError.Id}";

                opRes.Errors.Add(errorDto);
            }

            return (clientDto, opRes);
        }

    }
}

