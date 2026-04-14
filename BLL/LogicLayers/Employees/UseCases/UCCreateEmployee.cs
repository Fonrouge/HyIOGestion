using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.DTOs.Mappers;
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

namespace BLL.LogicLayers.Employees //=======================================================================REFACTORIZADO AL 27/02=======================================================================
{
    public class UCCreateEmployee : IUCCreateEmployee
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;

        private readonly string _tableNameEmployee;

        public UCCreateEmployee(
            IUnitOfWork uow,
            IApplicationSettings appSettings,
            IBitacoraFactory bitacoraFact,
            ISessionProvider sessionProvider,
            IErrorsFactory errorsFactory,
            IErrorsRepository errorsRepository)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _bitacoraFact = bitacoraFact ?? throw new ArgumentNullException(nameof(bitacoraFact));
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
            _errorsFactory = errorsFactory ?? throw new ArgumentNullException(nameof(errorsFactory));
            _errorsRepository = errorsRepository ?? throw new ArgumentNullException(nameof(errorsRepository));

            _tableNameEmployee = _appSettings.EmployeeTableName ?? "Employee";
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

                // 2. Conexión y Transacción (Base de Datos de Negocio)
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                // 3. Validar Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.EMPLOYEE_CREATE.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());
                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameEmployee)));
                    return result;
                }


                // 4. Validar Duplicados (Por Legajo/FileNumber)
                // Nota: Asegúrate de tener GetByFileNumberAsync en el repositorio
                var existing = await _uow.EmployeeRepo.GetByFileNumberAsync(dto.FileNumber);
                if (existing != null)
                {
                    var dupError = _errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameEmployee);
                    result.Errors.Add(ErrorMapper.ToDTO(dupError));
                    return result;
                }

                // 5. Mapeo a Entidad (Recuerda: EntityBase ya maneja el Id)
                var newEmployee = EmployeeMapper.ToEntity(dto);


                // --- OPCIONAL: Integridad Horizontal (DVH) ---
                // newEmployee.DVH = IntegrityService.GetIntegrityHash(
                //    newEmployee.Id, 
                //    newEmployee.FileNumber, 
                //    newEmployee.NationalId);


                // 6. Persistencia
                await _uow.EmployeeRepo.CreateAsync(newEmployee);

                // 7. Integridad Vertical (DVV)
       //         await UpdateDVVAsync(_tableNameEmployee, _appSettings.EntitiesConnection);

                // 8. Auditoría
                var log = _bitacoraFact.Create
                (
                    entry: BitacoraCatalogEnum.CreateOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameEmployee,
                    sessionId: _sessionProvider.Current.Id,
                    correlationId: Guid.NewGuid(),
                    extraInfo: $"Se creó el empleado: {newEmployee.LastName}, {newEmployee.FirstName} (Legajo: {newEmployee.FileNumber})"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                // 9. Confirmar Transacción
                await _uow.CommitAsync();

                // 10. Retorno
                result.Value = EmployeeMapper.ToDto(newEmployee);
                return result;
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                // Log técnico
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameEmployee;
                try { await _errorsRepository.CreateAsync(dbError); } catch { }

                // Error UI
                var uiError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNameEmployee);
                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id;
                errorDto.InformativeMessage = $"Falla técnica al crear empleado. Ref ID: {dbError.Id}";

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


