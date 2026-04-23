using BLL.Infrastructure.Errors;
using BLL.LogicLayer;
using BLL.LogicLayers;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Permisos.Abstracts;
using Domain.Entities.Permisos.Concrete;
using Domain.Entities.Products;
using Domain.Exceptions;
using Domain.Exceptions.Base;
using Domain.Infrastructure;
using Domain.Repositories;
using Shared;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
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
        public VerifyDVH
        (
            IUnitOfWork uow,
            IVerifyDVV verifyDVV,
            IErrorsRepository exceptionRepo,
            IApplicationSettings appSettings,
            IErrorsFactory errorsFactory
        )
        {
            _uow = uow ?? throw new ArgumentException($"{nameof(uow)} cannot be null.");
            _verifyDVV = verifyDVV ?? throw new ArgumentException($"{nameof(verifyDVV)} cannot be null.");
            _exceptionRepo = exceptionRepo ?? throw new ArgumentException($"{nameof(exceptionRepo)} cannot be null.");
            _appSettings = appSettings ?? throw new ArgumentException($"{nameof(appSettings)} cannot be null.");
            _errorsFactory = errorsFactory ?? throw new ArgumentException($"{nameof(errorsFactory)} cannot be null.");

            _tableUser = appSettings.UsuarioTableName;
            _tableEmployee = appSettings.EmployeeTableName;
        }

        /// <summary>
        /// Ejecuta el proceso de validación de integridad horizontal (DVH) y vertical (DVV).
        /// </summary>
        /// <returns>True si la integridad es correcta.</returns>
        /// <exception cref="IntegrityException">Lanzada si se detecta una alteración en los datos.</exception>
        public async Task<bool> ExecuteAsync()
        {
            // 1. Validar integridad fila por fila (DVH

            //Clients
            await ValidateUsersDVHAsync((await _uow.ClientRepo.GetAllAsync()).ToList());
            await ValidateUsersDVHAsync((await _uow.ClientRepo.GetAllDeletedAsync()).ToList());

            //Employees
            await ValidateUsersDVHAsync((await _uow.EmployeeRepo.GetAllAsync()).ToList());
            await ValidateUsersDVHAsync((await _uow.EmployeeRepo.GetAllDeletedAsync()).ToList());

            //Payments
            await ValidateUsersDVHAsync((await _uow.PaymentRepo.GetAllAsync()).ToList());
            await ValidateUsersDVHAsync((await _uow.PaymentRepo.GetAllDeletedAsync()).ToList());

            //Products + Relateds
            await ValidateUsersDVHAsync((await _uow.ProductRepo.GetAllAsync()).ToList());
            await ValidateUsersDVHAsync((await _uow.ProductRepo.GetAllDeletedAsync()).ToList());
            await ValidateUsersDVHAsync((await _uow.ProductRepo.GetAllProductCategoryAsync()).ToList());

            await ValidateUsersDVHAsync((await _uow.CategoryRepo.GetAllAsync()).ToList());
            await ValidateUsersDVHAsync((await _uow.CategoryRepo.GetAllDeletedAsync()).ToList());

            //Sale + Relateds
            await ValidateUsersDVHAsync((await _uow.SaleRepo.GetAllAsync()).ToList());
            await ValidateUsersDVHAsync((await _uow.SaleRepo.GetAllDeletedAsync()).ToList());

            await ValidateUsersDVHAsync((await _uow.SaleDetailRepo.GetAllAsync()).ToList());
            await ValidateUsersDVHAsync((await _uow.SaleDetailRepo.GetAllDeletedAsync()).ToList());

            //Suppliers
            await ValidateUsersDVHAsync((await _uow.SupplierRepo.GetAllAsync()).ToList());

            await ValidateUsersDVHAsync((await _uow.SupplierRepo.GetAllDeletedAsync()).ToList());

            //User + Relateds
            await ValidateUsersDVHAsync((await _uow.UserRepo.GetAllAsync()).ToList());
            await ValidateUsersDVHAsync((await _uow.UserRepo.GetAllDeletedAsync()).ToList());

            await ValidateUsersDVHAsync((await _uow.PermisoRepo.GetAllAsync()).ToList());
            await ValidateUsersDVHAsync((await _uow.PermisoRepo.GetAllPermisoPermisoAsync()).ToList());

            //Bitácora
            await ValidateUsersDVHAsync((await _uow.BitacoraRepo.GetAllAsync()).ToList());



            /*
            // 2. Validar integridad de tablas completas (DVV)
            var dvvChecks = new[]
            {
              (Table: _appSettings.ClientTableName, Conn: _appSettings.EntitiesConnection),
              (Table: _appSettings.EmployeeTableName, Conn: _appSettings.EntitiesConnection),
              (Table: _appSettings.PaymentTableName, Conn: _appSettings.EntitiesConnection),
              (Table: _appSettings.ProductTableName, Conn: _appSettings.EntitiesConnection),
              (Table: _appSettings.CategoryTableName, Conn: _appSettings.EntitiesConnection),
              (Table: _appSettings.ProductCategoryTableName, Conn: _appSettings.EntitiesConnection),
              (Table: _appSettings.SaleTableName, Conn: _appSettings.EntitiesConnection),
              (Table: _appSettings.SaleDetailTableName, Conn: _appSettings.EntitiesConnection),
              (Table: _appSettings.SupplierTableName, Conn: _appSettings.EntitiesConnection),

              (Table: _appSettings.UsuarioTableName, Conn: _appSettings.SecurityConnection),
              (Table: _appSettings.UsuarioPermisoTableName, Conn: _appSettings.SecurityConnection),
              (Table: _appSettings.PermisoTableName, Conn: _appSettings.SecurityConnection),
              (Table: _appSettings.PermisoPermisoTableName, Conn: _appSettings.SecurityConnection),
              (Table: _appSettings.BitacoraTableName, Conn: _appSettings.SecurityConnection)

          };
            
            foreach (var check in dvvChecks)
            {
                if (!await _verifyDVV.ExecuteAsync(check.Table, check.Conn))
                {
                    await HandleIntegrityErrorAsync(
                        check.Table,
                        Guid.Empty,
                        "Falla de DVV: El número de registros o el orden han sido alterados.",
                        ErrorCatalogEnum.InconsistentTableIntegrity
                    );
                }
            }
            */
            return true;
        }

        /// <summary>
        /// Valida el Dígito Verificador Horizontal para la lista de usuarios.
        /// </summary>
        private async Task ValidateUsersDVHAsync<TEntity>(IEnumerable<TEntity> allEntities) where TEntity : IIntegrityCheckable
        {
            if (allEntities == null)
                return;

            if (!allEntities.Any())
                return;

            foreach (var u in allEntities)
            {
                // El cálculo de hash es de CPU (memoria pura), así que se mantiene síncrono.
                var calculatedDVH = IntegrityService.GetIntegrityHash(u.GetDvhSerialization());

                //Activar para càlculo manual
                //if (u is Client c)
                //    Console.WriteLine($"{c.Id} {calculatedDVH}");



                //Desactivar para càlculo manual
                if (u.DVH.Value.ToString() != calculatedDVH)
                {
                    Guid entityId = Guid.Empty;

                    if (u is EntityBase entity)
                    {
                        entityId = entity.Id;
                    }

                    await HandleIntegrityErrorAsync(_tableUser, entityId, "Violación externa de registro individual (DVH).", ErrorCatalogEnum.InconsistentRowIntegrity);
                }

            }

            //Activar para càlculo manual
            //Debugger.Break();

        }

        /// <summary>
        /// Centraliza la creación, el registro en el repositorio de errores y el lanzamiento de excepciones de integridad.
        /// </summary>
        private async Task HandleIntegrityErrorAsync(string tableName, Guid entityId, string message, ErrorCatalogEnum errorType)
        {
            string stringId = (entityId == Guid.Empty) ? "N/A" : entityId.ToString();

            var exc = new IntegrityException(tableName, stringId, message);
            var error = _errorsFactory.Create(errorType, tableName);

            await _exceptionRepo.CreateAsync(error);
            throw exc;
        }
    }
}

