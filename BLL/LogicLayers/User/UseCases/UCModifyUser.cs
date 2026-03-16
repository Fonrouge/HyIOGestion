using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.DTOs.Mappers;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.Infrastructure.Audit;
using Domain.Repositories;
using Shared;
using Shared.Services;
using Shared.Sessions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.LogicLayers //=======================================================================REFACTORIZADO AL 27/02=======================================================================
{
    public class UCModifyUser : IUCModifyUser
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;
        private readonly IHashEncryptionService _encryptionSvc;

        private readonly string _tableNameUser;
        private readonly string _tableNameEmployee;

        public UCModifyUser
        (
            IUnitOfWork uow,
            IApplicationSettings appSettings,
            IBitacoraFactory bitacoraFact,
            ISessionProvider sessionProvider,
            IErrorsFactory errorsFactory,
            IErrorsRepository errorsRepository,
            IHashEncryptionService encryptionSvc
        )
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _bitacoraFact = bitacoraFact ?? throw new ArgumentNullException(nameof(bitacoraFact));
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
            _errorsFactory = errorsFactory ?? throw new ArgumentNullException(nameof(errorsFactory));
            _errorsRepository = errorsRepository ?? throw new ArgumentNullException(nameof(errorsRepository));
            _encryptionSvc = encryptionSvc ?? throw new ArgumentNullException(nameof(encryptionSvc));

            _tableNameUser = _appSettings.UserTableName ?? "Users";
            _tableNameEmployee = _appSettings.EmployeeTableName ?? "Employee";
        }

        public async Task<OperationResult<UsuarioDTO>> ExecuteAsync(UsuarioDTO userDto)
        {
            var result = new OperationResult<UsuarioDTO>();

            // 1. Validar Sesión Activa (Fail Fast)
            if (_sessionProvider.Current == null)
            {
                var newError = _errorsFactory.Create(ErrorCatalogEnum.SessionExpired);
                result.Errors.Add(ErrorMapper.ToDTO(newError));
                return result;
            }

            // Para verificar permisos, abrimos la BD de Seguridad
            _uow.SetConnectionString(_appSettings.SecurityConnection);

            var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);

            if (!currentUser.HasPermission("USER_UPDATE"))
            {
                var authError = _errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameUser);
                result.Errors.Add(ErrorMapper.ToDTO(authError));
                return result;
            }

            // --- TRAMPA DEL UPDATE: Validación de Username Duplicado ---
            var existingUsers = await _uow.UserRepo.GetByUsernameAsync(userDto.Username);
            var duplicateUser = existingUsers.FirstOrDefault();

            if (duplicateUser != null && duplicateUser.Id != userDto.Id)
            {
                var dupError = _errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameUser);
                result.Errors.Add(ErrorMapper.ToDTO(dupError));
                return result;
            }

            // 2. Mapeo a Entidades
            var userEntity = UsuarioMapper.ToEntity(userDto);
            var employeeEntity = EmployeeMapper.ToEntity(userDto.EmployeeDTO);
            userEntity.UpdateEmployeeId(employeeEntity.Id);

            // Encriptar password solo si viene uno nuevo (lógica típica de updates)
            if (!string.IsNullOrWhiteSpace(userDto.Password))
            {
                userEntity.UpdatePassword(_encryptionSvc.Hash(userDto.Password));
            }
            else
            {
                // Si no mandan password, conservamos el actual (requiere buscarlo)
                var oldUser = await _uow.UserRepo.GetByIdAsync(userDto.Id);
                userEntity.UpdatePassword(oldUser.Password);
            }

            // OPCIONAL SI CRECE: Integridad Horizontal (DVH)

            // userEntity.DVH = IntegrityService.GetIntegrityHash(...);
            // employeeEntity.DVH = IntegrityService.GetIntegrityHash(...);



            // =========================================================================
            // FASE 1: ACTUALIZAR EMPLEADO (Base de Datos: ENTITIES)
            // =========================================================================
            try
            {
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                await _uow.EmployeeRepo.UpdateAsync(employeeEntity);
                await UpdateDVVAsync(_tableNameEmployee, _appSettings.EntitiesConnection);

                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();
                return await HandleExceptionAsync(ex, result, _tableNameEmployee);
            }

            // =========================================================================
            // FASE 2: ACTUALIZAR USUARIO Y BITÁCORA (Base de Datos: SECURITY)
            // =========================================================================
            try
            {
                _uow.SetConnectionString(_appSettings.SecurityConnection);
                await _uow.BeginTransactionAsync();

                await _uow.UserRepo.UpdateAsync(userEntity);
                await UpdateDVVAsync(_tableNameUser, _appSettings.SecurityConnection);

                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.UpdateOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameUser,
                    extraInfo: $"Se modificó el usuario {userEntity.Username}."
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                await _uow.CommitAsync();

                result.Value = UsuarioMapper.ToDto(userEntity);
                return result;
            }

            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                // Nota: A diferencia del Create, en el Update hacer "compensación" (Saga) 
                // implicaría restaurar los datos viejos del Empleado. Por ahora, solo logueamos.

                return await HandleExceptionAsync(ex, result, _tableNameUser);
            }
        }

        private async Task UpdateDVVAsync(string nombreTabla, string connectionString)
        {
            var hashes = await _uow.IntegrityRepo.GetVerticalHashesAsync(nombreTabla, connectionString);
            var dvvFinal = IntegrityService.CalculateDVV(hashes);
            await _uow.IntegrityRepo.UpdateDVVAsync(nombreTabla, dvvFinal, connectionString);
        }

        private async Task<OperationResult<UsuarioDTO>> HandleExceptionAsync(Exception ex, OperationResult<UsuarioDTO> result, string table)
        {
            var dbError = _errorsFactory.CreateFromException(ex);
            dbError.Table = table;
            try { await _errorsRepository.CreateAsync(dbError); } catch { }

            var uiError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, table);
            var errorDto = ErrorMapper.ToDTO(uiError);
            errorDto.LogId = dbError.Id;
            errorDto.InformativeMessage = $"Falla técnica al modificar en {table}. Ref ID: {dbError.Id}";

            result.Errors.Add(errorDto);
            return result;
        }
    }
}