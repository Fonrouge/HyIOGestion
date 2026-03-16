using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.DTOs.Mappers;
using BLL.Infrastructure.Errors;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.Infrastructure.Permisos.Concrete;
using Shared;
using Shared.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Employees //=======================================================================REFACTORIZADO AL 27/02=======================================================================
{
    public class UCGetAllEmployees : IUCGetAllEmployees
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IErrorsFactory _errorsFactory;
        private readonly ISessionProvider _sessionProvider;
        private readonly string _tableNameEmployee;

        public UCGetAllEmployees
        (
            IUnitOfWork uow,
            IApplicationSettings appSettings,
            IErrorsFactory errorsFactory,
            ISessionProvider sessionProvider
        )
        {
            _uow = uow;
            _appSettings = appSettings;
            _errorsFactory = errorsFactory;
            _sessionProvider = sessionProvider;

            _tableNameEmployee = _appSettings.EmployeeTableName;
        }

        public async Task<(IEnumerable<EmployeeDTO>, OperationResult<EmployeeDTO>)> ExecuteAsync()
        {
            var result = new OperationResult<EmployeeDTO>();
            var listDto = new List<EmployeeDTO>();

            try
            {

                // 1. Validar Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.EMPLOYEE_VIEW.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());
                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameEmployee)));
                    return (listDto, result);
                }

                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                var employees = await _uow.EmployeeRepo.GetAllAsync();

                listDto = employees.Select(e => EmployeeMapper.ToDto(e)).ToList();

                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction)
                {
                    await _uow.RollbackAsync();
                }

                var errorDTO = ErrorMapper.ToDTO(_errorsFactory.Create(Domain.Exceptions.ErrorCatalogEnum.DatabaseUnavailable, _appSettings.EmployeeTableName));

                result.Errors.Add(errorDTO);
            }

            return (listDto, result);
        }
    }
}