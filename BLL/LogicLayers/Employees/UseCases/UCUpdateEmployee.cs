using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.DTOs.Mappers;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Exceptions;
using Domain.Exceptions.Base;
using Domain.Infrastructure;
using Domain.Infrastructure.Audit;
using Domain.Infrastructure.Permisos.Concrete;
using Domain.Repositories;
using Shared;
using Shared.Sessions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Employees
{
    public class UCUpdateEmployee : IUCUpdateEmployee
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;

        private readonly string _tableNameEmployee;

        public UCUpdateEmployee
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

            _tableNameEmployee = appSettings.EmployeeTableName ?? "Employees";
        }

        public async Task<OperationResult<EmployeeDTO>> ExecuteAsync(EmployeeDTO dto)
        {
            var result = new OperationResult<EmployeeDTO>();

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

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.EMPLOYEE_UPDATE.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());
                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameEmployee)));
                    return result;
                }


                // 3. Seteamos la conexión para las consultas de validación (Aún SIN abrir transacción)
                _uow.SetConnectionString(_appSettings.EntitiesConnection);


                // 4. Validación de Duplicados (Contra DB)
                var existingEmployeeWithTaxId = await _uow.EmployeeRepo.GetByNationalIdAsync(dto.NationalId);

                if (existingEmployeeWithTaxId != null && existingEmployeeWithTaxId.Id != dto.Id)
                {
                    var dupError = _errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameEmployee);
                    result.Errors.Add(ErrorMapper.ToDTO(dupError));
                    return result;
                }

                // 5. ABRIR TRANSACCIÓN (Solo llegamos acá si todo lo anterior es válido)
                await _uow.BeginTransactionAsync();

                // 6. Mapeo a Entidad (Fail Fast de VOs ocurre acá adentro)
                var employeeEntityToUpdate = EmployeeMapper.ToEntity(dto);

                // 7. Persistencia
                await _uow.EmployeeRepo.UpdateAsync(employeeEntityToUpdate);

                // 8. Auditoría (Bitácora)
                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.UpdateOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameEmployee,
                    extraInfo: $"Se actualizó el empleado ID: {employeeEntityToUpdate.Id} (Nuevo TaxId: {employeeEntityToUpdate.NationalId})"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                // 9. Confirmación
                await _uow.CommitAsync();

                // 10. Retorno
                result.Value = EmployeeMapper.ToDto(employeeEntityToUpdate);
                return result;
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                // Log técnico interno
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameEmployee;
                try { await _errorsRepository.CreateAsync(dbError); } catch { }

                // Error catalogado para el usuario
                ErrorLog uiError;

                if (ex.Message.Contains("REFERENCE constraint") || ex.Message.Contains("FK_") || ex.Message.Contains("UNIQUE"))
                {
                    uiError = _errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameEmployee);
                }
                else
                {
                    uiError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNameEmployee);
                }

                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id;
                errorDto.InformativeMessage = $"Falla técnica al actualizar el empleado. Ref ID: {dbError.Id}";

                result.Errors.Add(errorDto);
                return result;
            }
        }
    }
}