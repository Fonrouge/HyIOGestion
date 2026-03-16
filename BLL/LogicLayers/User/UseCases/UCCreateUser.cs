using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.DTOs.Mappers;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using BLL.LogicLayers;
using Domain.Entities;
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

namespace BLL.UseCases
{
    public class UCCreateUser : IUCCreateUser
    {
        private readonly IApplicationSettings _appSettings;
        private readonly IUnitOfWork _uow;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;
        private readonly IHashEncryptionService _encryptionSvc;
        private readonly string _tableNameUser;
        private readonly string _tableNameEmployee;

        public UCCreateUser
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
            _uow = uow;
            _appSettings = appSettings;
            _bitacoraFact = bitacoraFact;
            _sessionProvider = sessionProvider;
            _errorsFactory = errorsFactory;
            _errorsRepository = errorsRepository;
            _encryptionSvc = encryptionSvc;
            _tableNameUser = _appSettings.UserTableName ?? "Users";
            _tableNameEmployee = _appSettings.EmployeeTableName ?? "Employee";
        }

        public async Task<OperationResult<UsuarioDTO>> CreateAsync(UsuarioDTO userDto)
        {
            var result = new OperationResult<UsuarioDTO>();

            try
            {
                // 1. Sesión y Permisos
                if (_sessionProvider.Current == null)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.SessionExpired)));
                    return result;
                }

                var currentSession = _sessionProvider.Current; // ← Movido aquí (más eficiente)

                // 2. FAIL FAST: Verificación de Duplicados antes de tocar la DB
                _uow.SetConnectionString(_appSettings.SecurityConnection);
                var existingUser = await _uow.UserRepo.GetByUsernameAsync(userDto.Username);
                if (existingUser.Any())
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameUser)));
                    return result;
                }

                if (userDto.EmployeeDTO == null)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InvalidData, "EmployeeDTO es requerido")));
                    return result;
                }

                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                var existingEmployee = await _uow.EmployeeRepo.GetByNationalIdAsync(userDto.EmployeeDTO.NationalId);
                if (existingEmployee != null)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameEmployee)));
                    return result;
                }

                // 3. Preparación de Entidades
                var employeeEntity = EmployeeMapper.ToEntity(userDto.EmployeeDTO);
                var userEntity = UsuarioMapper.ToEntity(userDto);
                
                userEntity.UpdateEmployeeId(employeeEntity.Id);
                userEntity.UpdatePassword(_encryptionSvc.Hash(userDto.Password));

                // 4. FASE 1: GUARDAR EMPLEADO (Entities)
                try
                {
                    _uow.SetConnectionString(_appSettings.EntitiesConnection);
                    await _uow.BeginTransactionAsync();
                    await _uow.EmployeeRepo.CreateAsync(employeeEntity);
                    await UpdateDVVAsync(_tableNameEmployee, _appSettings.EntitiesConnection);
                    await _uow.CommitAsync();
                }
                catch (Exception ex)
                {
                    if (_uow.HasActiveTransaction) await _uow.RollbackAsync();
                    return await HandleExceptionAsync(ex, result, _tableNameEmployee);
                }

                // 5. FASE 2: GUARDAR USUARIO Y BITÁCORA (Security)
                try
                {
                    _uow.SetConnectionString(_appSettings.SecurityConnection);
                    await _uow.BeginTransactionAsync();

                    await _uow.UserRepo.CreateAsync(userEntity);
                    await UpdateDVVAsync(_tableNameUser, _appSettings.SecurityConnection);

                    var log = _bitacoraFact.Create(
                        entry: BitacoraCatalogEnum.CreateOnBD,
                        user: currentSession.CurrentUserId.ToString(),
                        tableName: _tableNameUser,
                        extraInfo: $"Se creó el usuario {userEntity.Username}."
                    );
                    await _uow.BitacoraRepo.CreateAsync(log);
                    await _uow.CommitAsync();

                    // Respuesta exitosa
                    var finalDto = UsuarioMapper.ToDto(userEntity);
                    finalDto.EmployeeDTO = EmployeeMapper.ToDto(employeeEntity);
                    result.Value = finalDto;
                    return result;
                }
                catch (Exception ex)
                {
                    if (_uow.HasActiveTransaction) await _uow.RollbackAsync();
                    await DeleteEmployeeForConsistency(employeeEntity.Id);
                    return await HandleExceptionAsync(ex, result, _tableNameUser);
                }
            }
            catch (Exception ex)
            {
                // Catch raíz por si algo falla en el mapeo o validaciones iniciales
                return await HandleExceptionAsync(ex, result, "General");
            }
        }

        private async Task<OperationResult<UsuarioDTO>> HandleExceptionAsync(Exception ex, OperationResult<UsuarioDTO> result, string table)
        {
            var dbError = _errorsFactory.CreateFromException(ex);
            dbError.Table = table;
            try { await _errorsRepository.CreateAsync(dbError); } catch { }

            var uiError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, table);
            var errorDto = ErrorMapper.ToDTO(uiError);
            errorDto.LogId = dbError.Id;
            errorDto.InformativeMessage = $"Falla técnica en la tabla {table}. Ref ID: {dbError.Id}";
            result.Errors.Add(errorDto);
            return result;
        }

        private async Task DeleteEmployeeForConsistency(Guid employeeId)
        {
            try
            {
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                await _uow.EmployeeRepo.DeleteAsync(employeeId);
                await UpdateDVVAsync(_tableNameEmployee, _appSettings.EntitiesConnection);
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameEmployee;
                dbError.InformativeMessage = $"CRÍTICO: Falló la compensación. El empleado {employeeId} quedó huérfano.";
                try { await _errorsRepository.CreateAsync(dbError); } catch { }
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