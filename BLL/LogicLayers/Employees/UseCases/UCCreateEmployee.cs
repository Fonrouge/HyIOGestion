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

namespace BLL.LogicLayers.Employees //=======================================================================REFACTORIZADO AL 14/04=======================================================================
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
        private Guid _correlationId;

        public UCCreateEmployee
        (
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

            _tableNameEmployee = _appSettings.EmployeeTableName ?? "Employees";
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


                // 2. Conexión y Transacción (Base de Datos de Negocio) Necesaria para poder chequear permisos.
                _uow.SetConnectionString(_appSettings.EntitiesConnection);


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
                var existing = await _uow.EmployeeRepo.GetByFileNumberAsync(dto.FileNumber);

                if (existing != null)
                {
                    var dupError = _errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameEmployee);
                    result.Errors.Add(ErrorMapper.ToDTO(dupError));

                    return result;
                }

                // 5. Validación de Duplicados (Contra DB)
                var existingEmployeeByTaxIdNumber = await _uow.EmployeeRepo.GetByNationalIdAsync(dto.NationalId);

                if (existingEmployeeByTaxIdNumber != null)
                {
                    var dupError = _errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameEmployee);
                    result.Errors.Add(ErrorMapper.ToDTO(dupError));
                    return result;
                }

                //6. Se abre transacción sólo si se cumplen los requisitos mínimos para poder operar            
                await _uow.BeginTransactionAsync();

                // 7. Mapeo a Entidad (Recuerda: EntityBase ya maneja el Id)
                var newEmployee = EmployeeMapper.ToEntity(dto);


                // 8. Se calcula y setea su DVH
                IntegrityFacade.RecalculateAndSetEntityDVH(newEmployee);


                // 9. Persistencia con DVH ya incluido
                await _uow.EmployeeRepo.CreateAsync(newEmployee);

                // 10. Integridad Vertical (DVV) - Una vez creado el dato
                await UpdateDVVAsync(_tableNameEmployee, _appSettings.EntitiesConnection);


                // 11. Auditoría
                var log = _bitacoraFact.Create
                (
                    entry: BitacoraCatalogEnum.CreateOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameEmployee,
                    sessionId: _sessionProvider.Current.Id,
                    correlationId: _correlationId,
                    extraInfo: $"Se creó el Empleado Id: {newEmployee.Id}, ({newEmployee.LastName}, {newEmployee.FirstName} - Legajo: {newEmployee.FileNumber})"
                );
                await _uow.BitacoraRepo.CreateAsync(log);


                // 12. Confirmar Transacción
                await _uow.CommitAsync();


                // 13. Retorno
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
                errorDto.InformativeMessage = $"Falla técnica al crear empleado. Contacte a soporte y brinde el siguiente código para un diagnóstico preciso ({dbError.Id}), en caso de no hacerlo por favor reinicie la aplicación e intente nuevamente.";

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


