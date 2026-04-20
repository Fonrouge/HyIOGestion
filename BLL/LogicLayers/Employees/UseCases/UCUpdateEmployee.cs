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
using Shared.Services;
using Shared.Sessions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Employees //=======================================================================REFACTORIZADO AL 14/04=======================================================================
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
        private Guid _correlationId;

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
            _correlationId = Guid.NewGuid();
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


                // 3. Seteamos la conexión para las consultas de validación
                _uow.SetConnectionString(_appSettings.EntitiesConnection);


                // 4. Validación de Duplicados (DNI y Legajo)
                var employeeWithSameDni = await _uow.EmployeeRepo.GetByNationalIdAsync(dto.NationalId);
                if (employeeWithSameDni != null && employeeWithSameDni.Id != dto.Id)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, "El DNI ya pertenece a otro empleado")));
                    return result;
                }

                var employeeWithSameFile = await _uow.EmployeeRepo.GetByFileNumberAsync(dto.FileNumber);
                if (employeeWithSameFile != null && employeeWithSameFile.Id != dto.Id)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, "El número de legajo ya está en uso")));
                    return result;
                }

                // 5. ABRIR TRANSACCIÓN (Solo llegamos acá si todo lo anterior es válido)
                await _uow.BeginTransactionAsync();

                // 6. Mapeo a Entidad (Fail Fast de VOs ocurre acá adentro)
                var employeeEntityToUpdate = EmployeeMapper.ToEntity(dto);

                // 7. Recalcular DVH después de una modificación
                IntegrityFacade.RecalculateAndSetEntityDVH(employeeEntityToUpdate);

                // 8. Persistencia
                await _uow.EmployeeRepo.UpdateAsync(employeeEntityToUpdate);

                // 9. Recálculo de DVV una vez persistida la entidad
                await UpdateDVVAsync(_tableNameEmployee, _appSettings.EntitiesConnection);

                // 10. Auditoría (Bitácora)
                var log = _bitacoraFact.Create
                (
                    tableName: _tableNameEmployee,
                    entry: BitacoraCatalogEnum.UpdateOnBD,
                    user: currentUser.Id.ToString(),
                    sessionId: _sessionProvider.Current.Id,
                    correlationId: _correlationId,
                    extraInfo: $"Se actualizó el empleado ID: {employeeEntityToUpdate.Id} (Nuevo TaxId: {employeeEntityToUpdate.NationalId})"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                // 11. Confirmación
                await _uow.CommitAsync();

                // 12. Retorno
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

        private async Task UpdateDVVAsync(string nombreTabla, string connectionString)
        {
            var hashes = await _uow.IntegrityRepo.GetVerticalHashesAsync(nombreTabla, connectionString);
            var dvvFinal = IntegrityService.CalculateDVV(hashes);
            await _uow.IntegrityRepo.UpdateDVVAsync(nombreTabla, dvvFinal, connectionString);
        }
    }
}