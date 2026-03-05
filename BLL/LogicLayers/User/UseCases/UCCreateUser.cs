using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.DTOs.Mappers;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.Infrastructure.Audit;
using Domain.Repositories;
using Shared;
using Shared.Services;
using Shared.Sessions;
using System;
using System.Threading.Tasks;

namespace BLL.UseCases
{
    public class UCCreateUser : IUCCreateUser
    {
        private readonly IApplicationSettings _appSettings;
        private readonly IUnitOfWork _uow;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider; // Usamos el Provider que armamos antes
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;

        private readonly string _tableNameUser;
        private readonly string _tableNameEmployee;

        public UCCreateUser
        (
            IUnitOfWork uow,
            IApplicationSettings appSettings,
            IBitacoraFactory bitacoraFact,
            ISessionProvider sessionProvider,
            IErrorsFactory errorsFactory,
            IErrorsRepository errorsRepository
        )
        {
            _uow = uow;
            _appSettings = appSettings;
            _bitacoraFact = bitacoraFact;
            _sessionProvider = sessionProvider;
            _errorsFactory = errorsFactory;
            _errorsRepository = errorsRepository;

            _tableNameUser = _appSettings.UserTableName ?? "Users";
            _tableNameEmployee = _appSettings.EmployeeTableName ?? "Employee";
        }

        public async Task<OperationResult<UsuarioDTO>> CreateAsync(UsuarioDTO userDto)
        {
            var result = new OperationResult<UsuarioDTO>();

            // =========================================================================
            // 1. Validaciones iniciales (Fail Fast)
            // =========================================================================
            if (_sessionProvider.Current == null)
            {
                var newError = _errorsFactory.Create(ErrorCatalogEnum.SessionExpired);
                result.Errors.Add(ErrorMapper.ToDTO(newError));
                return result;
            }

            var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);

            if (!currentUser.HasPermission("USER_CREATE"))
            {
                var authError = _errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameUser);
                result.Errors.Add(ErrorMapper.ToDTO(authError));
                return result;
            }

            // =========================================================================
            // 2. Mapeo y preparación de Entidades (Aplicando Secure by Design)
            // =========================================================================

            // El mapper ya debería estar asignando userEntity.EmployeeId internamente 
            // o devolviéndolo listo, pero si no, lo extraemos del DTO anidado
            var userEntity = UsuarioMapper.ToEntity(userDto);
            var employeeEntity = EmployeeMapper.ToEntity(userDto.EmployeeDTO);

            // CORRECCIÓN CRÍTICA 1: Ya no asignamos el objeto entero, sino el ID para cruzar referencias
            // Aseguramos que el usuario apunte al ID generado por la entidad Empleado
            userEntity.EmployeeId = employeeEntity.Id;

            userEntity.Password = new HashEncryptionService().Hash(userDto.Password);

            // CORRECCIÓN CRÍTICA 2: Integridad (DVH) de Usuario
            userEntity.DVH = IntegrityService.GetIntegrityHash
            (
                userEntity.Id,
                userEntity.Username,
                userEntity.Language,
                userEntity.EmployeeId // Pasamos el EmployeeId en lugar de userEntity.Employee.Id
            );

            // Integridad (DVH) de Empleado - Calculamos el hash pidiendo el .Value a los Value Objects
            string calculatedEmployeeDvh = IntegrityService.GetIntegrityHash
            (
                employeeEntity.Id,
                employeeEntity.FirstName?.Value,
                employeeEntity.LastName?.Value,
                employeeEntity.NationalId?.Value
            );

            // Asignamos a través del comportamiento de la entidad (respetando el private set)
            employeeEntity.UpdateDVH(calculatedEmployeeDvh);

            // =========================================================================
            // FASE 1: GUARDAR EMPLEADO (Base de Datos: ENTITIES)
            // =========================================================================
            try
            {
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                await _uow.EmployeeRepo.CreateAsync(employeeEntity);
                await UpdateDVVAsync(_tableNameEmployee, _appSettings.EntitiesConnection);

                await _uow.CommitAsync(); // Empleado guardado con éxito
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();
                return await HandleExceptionAsync(ex, result, _tableNameEmployee); // Falla rápida
            }


            // =========================================================================
            // FASE 2: GUARDAR USUARIO Y BITÁCORA (Base de Datos: SECURITY)
            // =========================================================================
            try
            {
                // Cambiamos el "switch" a la base de datos de seguridad
                _uow.SetConnectionString(_appSettings.SecurityConnection);
                await _uow.BeginTransactionAsync();

                await _uow.UserRepo.CreateAsync(userEntity);
                await UpdateDVVAsync(_tableNameUser, _appSettings.SecurityConnection);

                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.CreateOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameUser,
                    extraInfo: $"Se creó el usuario {userEntity.Username}."
                );

                // OJO: Chequea si la bitácora va en SecurityConnection o en una BD separada.
                // Si va en Security, esto está perfecto.
                await _uow.BitacoraRepo.CreateAsync(log);

                await _uow.CommitAsync(); // Usuario y Log guardados con éxito

                // Todo salió perfecto, preparamos la respuesta
                var finalDto = UsuarioMapper.ToDto(userEntity);

                // Anidamos el empleado DTO creado para que el frontend lo tenga de inmediato 
                // sin necesidad de volver a consultar a la DB
                finalDto.EmployeeDTO = EmployeeMapper.ToDto(employeeEntity);

                result.Value = finalDto;
                return result;
            }
            catch (Exception ex)
            {
                // 1. Deshacemos la transacción de Seguridad (Usuario y Bitácora)
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                // 2. COMPENSACIÓN: Borramos el empleado en la BD de Negocio para mantener consistencia
                await DeleteEmployeeForConsistency(employeeEntity.Id);

                // 3. Retornamos el error original que causó todo este problema
                return await HandleExceptionAsync(ex, result, _tableNameUser);
            }
        }

        // Extraje el manejo de errores a un método auxiliar para no repetir código en las dos fases
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
                // Volvemos a apuntar a la base de datos de Negocio
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                // 1. Borramos el empleado huérfano
                await _uow.EmployeeRepo.DeleteAsync(employeeId);

                // 2. CRÍTICO: Como alteramos la tabla, recalculamos el DVV para no dejarla corrupta
                await UpdateDVVAsync(_tableNameEmployee, _appSettings.EntitiesConnection);

                // 3. Confirmamos la compensación
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                // Si la compensación falla, tenemos un problema grave (registro huérfano real).
                // Logueamos esto silenciosamente como error crítico interno.
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameEmployee;
                dbError.InformativeMessage = $"CRÍTICO: Falló la compensación. El empleado {employeeId} quedó huérfano.";

                try { await _errorsRepository.CreateAsync(dbError); } catch { }
            }
        }


        // Método privado arreglado (async Task y await)
        private async Task UpdateDVVAsync(string nombreTabla, string connectionString)
        {
            var hashes = await _uow.IntegrityRepo.GetVerticalHashesAsync(nombreTabla, connectionString);
            var dvvFinal = IntegrityService.CalculateDVV(hashes);
            await _uow.IntegrityRepo.UpdateDVVAsync(nombreTabla, dvvFinal, connectionString);
        }
    }
}