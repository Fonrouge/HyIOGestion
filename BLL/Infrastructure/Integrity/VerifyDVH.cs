using BLL.Infrastructure.Errors;
using BLL.LogicLayer;
using Domain.Entities;
using Domain.Entities.Permisos.Concrete;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.Repositories;
using Shared;
using Shared.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.UseCases
{
    /// <summary>
    /// Caso de uso encargado de verificar la integridad de los datos (DVH y DVV) 
    /// para las entidades críticas del sistema.
    /// </summary>
    public class VerifyDVH : IVerifyDVH // Recuerda actualizar esta interfaz a Task<bool> ExecuteAsync()
    {
        private readonly IUnitOfWork _uow;
        private readonly IVerifyDVV _verifyDVV;
        private readonly IErrorsRepository _exceptionRepo;
        private readonly IApplicationSettings _appSettings;
        private readonly IErrorsFactory _errorsFactory;

        private readonly string _tableUser;
        private readonly string _tableEmployee;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="VerifyDVH"/>.
        /// </summary>
        public VerifyDVH(IUnitOfWork uow, IVerifyDVV verifyDVV, IErrorsRepository exceptionRepo, IApplicationSettings appSettings, IErrorsFactory errorsFactory)
        {
            _uow = uow;
            _verifyDVV = verifyDVV;
            _exceptionRepo = exceptionRepo;
            _appSettings = appSettings;
            _errorsFactory = errorsFactory;

            _tableUser = appSettings.UserTableName;
            _tableEmployee = appSettings.EmployeeTableName;
        }

        /// <summary>
        /// Ejecuta el proceso de validación de integridad horizontal (DVH) y vertical (DVV).
        /// </summary>
        /// <returns>True si la integridad es correcta.</returns>
        /// <exception cref="IntegrityException">Lanzada si se detecta una alteración en los datos.</exception>
        public async Task<bool> ExecuteAsync()
        {
            // 1. Validar integridad fila por fila (DVH) - Hacemos el await primero y luego materializamos a lista
            var users = await _uow.UserRepo.GetAllAsync();
            await ValidateUsersDVHAsync(users.ToList());

            var employees = await _uow.EmployeeRepo.GetAllAsync();
            await ValidateEmployeesDVHAsync(employees.ToList());

            // 2. Validar integridad de tablas completas (DVV)
            var dvvChecks = new[]
            {
                (Table: _tableUser, Conn: _appSettings.SecurityConnection),
                (Table: _tableEmployee, Conn: _appSettings.EntitiesConnection)
            };

            foreach (var check in dvvChecks)
            {
                // Asumiendo que adaptaste IVerifyDVV para que sea ExecuteAsync
                if (!await _verifyDVV.ExecuteAsync(check.Table, check.Conn))
                {
                    await HandleIntegrityErrorAsync(
                        check.Table,
                        "Falla de DVV: El número de registros o el orden han sido alterados.",
                        ErrorCatalogEnum.InconsistentTableIntegrity
                    );
                }
            }

            return true;
        }

        /// <summary>
        /// Valida el Dígito Verificador Horizontal para la lista de usuarios.
        /// </summary>
        private async Task ValidateUsersDVHAsync(List<Usuario> allUsers)
        {
            foreach (var u in allUsers)
            {
                // El cálculo de hash es de CPU (memoria pura), así que se mantiene síncrono.
                var calculatedDVH = IntegrityService.GetIntegrityHash(u.Id, u.Username, u.Language, u.EmployeeId);

                if (u.DVH != calculatedDVH)
                {
                    // Si falla, el registro de error es asíncrono (I/O)
                    await HandleIntegrityErrorAsync(_tableUser, "Violación externa de registro individual (DVH).", ErrorCatalogEnum.InconsistentRowIntegrity);
                }
            }
        }

        /// <summary>
        /// Valida el Dígito Verificador Horizontal para la lista de empleados.
        /// </summary>
        private async Task ValidateEmployeesDVHAsync(List<Employee> allEmployees)
        {
            foreach (var e in allEmployees)
            {
                var calculatedDVH = IntegrityService.GetIntegrityHash(e.Id, e.FirstName, e.LastName, e.NationalId);

                if (e.DVH != calculatedDVH)
                {
                    await HandleIntegrityErrorAsync(_tableEmployee, "Violación externa de registro individual (DVH).", ErrorCatalogEnum.InconsistentRowIntegrity);
                }
            }
        }

        /// <summary>
        /// Centraliza la creación, el registro en el repositorio de errores y el lanzamiento de excepciones de integridad.
        /// </summary>
        private async Task HandleIntegrityErrorAsync(string tableName, string message, ErrorCatalogEnum errorType)
        {
            var exc = new IntegrityException(tableName, "N/A", message);
            var error = _errorsFactory.Create(errorType, tableName);

            // Guardamos el error de forma asíncrona
            await _exceptionRepo.CreateAsync(error);

            // Rompemos la ejecución de la aplicación (esto es correcto en fallos de integridad)
            throw exc;
        }
    }
}