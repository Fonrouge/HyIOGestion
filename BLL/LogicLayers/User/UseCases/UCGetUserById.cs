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

namespace BLL.LogicLayers.User.UseCases
{
    public class UCGetUserById : IUCGetUserById
    {
        private readonly IUnitOfWork _uow;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;
        private readonly ISessionProvider _sessionProvider;
        private readonly IApplicationSettings _appSettings;

        private readonly string _tableNameUser;

        public UCGetUserById
        (
            IUnitOfWork uow,
            IApplicationSettings appSettings,
            IErrorsFactory errorsFactory,
            IErrorsRepository errorsRepository,
            ISessionProvider sessionProvider
        )
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _errorsFactory = errorsFactory ?? throw new ArgumentNullException(nameof(errorsFactory));
            _errorsRepository = errorsRepository ?? throw new ArgumentNullException(nameof(errorsRepository));
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));

            _tableNameUser = _appSettings.UsuarioTableName;
        }

        public async Task<(UsuarioDTO, OperationResult<UsuarioDTO>)> ExecuteAsync(Guid id)
        {
            var result = new OperationResult<UsuarioDTO>();
            var userDto = new UsuarioDTO();

            try
            {
                _uow.SetConnectionString(_appSettings.SecurityConnection);

                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);

                if (currentUser == null)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.SessionExpired)));
                    return (userDto, result);
                }

                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);
                if (permissionsList.Count < 1)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameUser)));
                    return (userDto, result);
                }
                else
                {
                    permissionsList.ForEach(p => currentUser.AddPermiso(p));
                }

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.USER_MANAGEMENT.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());
                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameUser)));
                    return (userDto, result);
                }
                
                userDto = UsuarioMapper.ToDto(currentUser);

                return (userDto, result);
            }

            catch (Exception ex)
            {
                // 1. Log técnico interno
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameUser ?? "User";

                try
                {
                    await _errorsRepository.CreateAsync(dbError);
                }
                catch { }

                // 2. Error catalogado para el usuario (CERO strings mágicos)
                var uiError = _errorsFactory.Create(ErrorCatalogEnum.DataLoadError, _tableNameUser);

                // 3. Mapeo y vinculación de trazabilidad para soporte técnico
                var errorDto = ErrorMapper.ToDTO(uiError);

                errorDto.LogId = dbError.Id;
                errorDto.InformativeMessage = $"Falla técnica. Reference ID: {dbError.Id}";

                result.Errors.Add(errorDto);
            }

            return (userDto, result);
        }

    }
}

