using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.DTOs.Mappers;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Entities.Permisos.Concrete;
using Domain.Exceptions;
using Domain.Exceptions.Base;
using Domain.Infrastructure;
using Domain.Infrastructure.Audit;
using Domain.Repositories;
using Shared;
using Shared.Services;
using Shared.Sessions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.UseCases //=======================================================================REFACTORIZADO AL 27/02=======================================================================
{
    public class UCLogin : IUCLogin
    {
        //Unit of work
        private readonly IUnitOfWork _uow;
        
        //Cross-Cutting ('Shared' project)
        private readonly IHashEncryptionService _encryptionSvc;
        private readonly IApplicationSettings _appSettings;

        //Security / Audit
        private readonly ISessionManager _sessionMgr;
        private readonly IErrorsRepository _errorsRepo;        
        private readonly ISessionFactory _sessionFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IBitacoraFactory _bitacoraFact;               
        private readonly IErrorsFactory _errorsFactory;
        
        

        public UCLogin
        (
             IUnitOfWork uow,
             IHashEncryptionService encryptionSvc,
             IApplicationSettings appSettings,
             IErrorsRepository errorsRepo,
             ISessionManager sessionMgr,
             ISessionFactory sessionFact,
             ISessionProvider sessionProvider,
             IBitacoraFactory bitacoraFact,
             IErrorsFactory errorsFactory
        )
        {
            _uow = uow;
            _encryptionSvc = encryptionSvc ?? throw new ArgumentNullException(nameof(encryptionSvc));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _errorsRepo = errorsRepo ?? throw new ArgumentNullException(nameof(errorsRepo));
            _sessionMgr = sessionMgr ?? throw new ArgumentNullException(nameof(sessionMgr));
            _sessionFact = sessionFact ?? throw new ArgumentNullException(nameof(sessionFact));
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
            _bitacoraFact = bitacoraFact ?? throw new ArgumentNullException(nameof(bitacoraFact));
            _errorsFactory = errorsFactory ?? throw new ArgumentNullException(nameof(errorsFactory));
        }


          public async Task<OperationResult<UsuarioDTO>> ExecuteAsync(string userName, string pass)
        {
            var result = new OperationResult<UsuarioDTO>();

            try
            {
                // 1. FASE DE LECTURA (Seguridad): Buscamos al usuario
                _uow.SetConnectionString(_appSettings.SecurityConnection);
                var user = await GetValidatedUserAsync(userName, result);
                if (user == null) return result;

                // 2. FASE DE LECTURA CRUZADA (Negocio): Buscamos al empleado asociado
                // Cambiamos el connection string ANTES de abrir cualquier transacción
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                var employee = await _uow.EmployeeRepo.GetByIdAsync(user.EmployeeId);

                if (employee == null)
                {
                    // Fallo de integridad: El usuario apunta a un empleado que no existe
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InternalError, _appSettings.EmployeeTableName)));
                    return result;
                }

                // 3. FASE DE VALIDACIÓN Y ESCRITURA: Volvemos a Seguridad
                _uow.SetConnectionString(_appSettings.SecurityConnection);

                // Pasamos el empleado a la validación para verificar si está Activo
                if (!await ValidateCredentialsAsync(user, employee, pass, result)) return result;

                // AHORA SÍ abrimos la transacción de Seguridad (seguros de que no leeremos de Negocio)
                await _uow.BeginTransactionAsync();

                if (!await LoadPermissionsAndEmployeeAsync(user, result))
                {
                    await _uow.RollbackAsync();
                    return result;
                }

                // 4. Orquestación de Sesión y DTOs
                var userDto = UsuarioMapper.ToDto(user);
                // Mapeamos el empleado que ya habíamos ido a buscar en la Fase 2
                userDto.EmployeeDTO = EmployeeMapper.ToDto(employee);

                var currentSession = _sessionFact.Create(userDto.Id);

                _sessionMgr.AddSession(currentSession);
                _sessionProvider.Current = currentSession;
                result.SessionID = currentSession.Id;

                // 5. Auditoría
                await AuditLoginAsync(currentSession);

                // Commit final
                await _uow.CommitAsync();

                result.Value = userDto;
            }
            // ... (el catch queda igual) ...
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction)
                {
                    await _uow.RollbackAsync();
                }

                // 1. Guardar el error técnico en la base de datos (vital para el soporte)
                await LogSystemErrorAsync(ex);

                // 2. Usar tu fábrica de OperationResult para devolver el error a la UI
                //    Le pasás la excepción y, si la sesión llegó a crearse, se la pasás también.
                var errorResult = OperationResult<UsuarioDTO>.FromException(ex, result.SessionID);

                // Opcional: Si querés mantener tu ErrorCatalogEnum para la UI, podés sobreescribir el mensaje
                errorResult.Errors[0].Message = "Falla técnica al intentar iniciar sesión.";

                return errorResult;
            }
            return result;
        }

        private async Task<Usuario> GetValidatedUserAsync(string userName, OperationResult<UsuarioDTO> result)
        {
            var usersFound = (await _uow.UserRepo.GetByUsernameAsync(userName)).ToList();

            if (usersFound.Count == 0)
            {
                var error = await CreateErrorByEnumAsync(ErrorCatalogEnum.InvalidCredentials);
                result.Errors.Add(ErrorMapper.ToDTO(error));
                return null;
            }

            if (usersFound.Count > 1)
            {
                var error = await CreateErrorByEnumAsync(ErrorCatalogEnum.DuplicateEntry);
                result.Errors.Add(ErrorMapper.ToDTO(error));
                return null;
            }

            return usersFound[0];
        }

        private async Task<bool> ValidateCredentialsAsync(Usuario user, Domain.Entities.Employee employee, string providedPass, OperationResult<UsuarioDTO> result)
        {
            if (!_encryptionSvc.Verify(providedPass, user.Password))
            {
                var error = await CreateErrorByEnumAsync(ErrorCatalogEnum.InvalidCredentials);
                result.Errors.Add(ErrorMapper.ToDTO(error));
                return false;
            }

            // Validamos contra el objeto 'employee' que pasamos por parámetro
            if (!employee.Active)
            {
                var error = await CreateErrorByEnumAsync(ErrorCatalogEnum.EntityInactive);
                result.Errors.Add(ErrorMapper.ToDTO(error));
                return false;
            }

            return true;
        }

        private async Task<bool> LoadPermissionsAndEmployeeAsync(Usuario user, OperationResult<UsuarioDTO> result)
        {
            try
            {
                user.Permisos = await _uow.PermisoRepo.GetPermissionsByUserAsync(user.Id);

                if (user.Permisos == null || !user.Permisos.Any())
                {
                    var error = await CreateErrorByEnumAsync(ErrorCatalogEnum.InsufficientPermissions);
                    result.Errors.Add(ErrorMapper.ToDTO(error));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                await LogSystemErrorAsync(ex);
                return false;
            }
        }

        private async Task AuditLoginAsync(ISession session)
        {
            var log = _bitacoraFact.Create(
                entry: BitacoraCatalogEnum.LoginAttempt,
                user: $"User ID: {session.CurrentUserId}",
                tableName: _appSettings.UserTableName,
                extraInfo: $"Session ID: {session.Id}"
            );
           
            await _uow.BitacoraRepo.CreateAsync(log);
        }


        // --- Manejo de Errores Asíncrono ---

        private async Task<ErrorLog> CreateErrorByEnumAsync(ErrorCatalogEnum errorEnum)
        {
            var error = _errorsFactory.Create(errorEnum, "User");
            await _errorsRepo.CreateAsync(error);
            return error;
        }

        private async Task LogSystemErrorAsync(Exception ex)
        {
            if (ex == null) return;
            await _errorsRepo.CreateAsync(_errorsFactory.CreateFromException(ex));
        }
    }
}