using BLL.DTOs;
using BLL.DTOs.Errors; // Para usar el ErrorLogDTO
using BLL.Infrastructure.Errors;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.Infrastructure.Permisos.Concrete;
using Shared; // Para IApplicationSettings
using Shared.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Descomentar cuando tengas el mapper en este o en otro namespace
// using BLL.DTOs.Mappers; 

namespace BLL.LogicLayers.Suppliers
{
    public class UCGetAllSuppliers : IUCGetAllSuppliers
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IErrorsFactory _errorsFactory;
        private readonly ISessionProvider _sessionProvider;


        private readonly string _tableNameSupplier;


        public UCGetAllSuppliers(IUnitOfWork uow, IApplicationSettings appSettings, IErrorsFactory errorsFactory, ISessionProvider sessionProvider)
        {
            _uow = uow;
            _appSettings = appSettings;
            _errorsFactory = errorsFactory;
            _sessionProvider = sessionProvider;

            _tableNameSupplier = appSettings.SupplierTableName ?? "Suppliers";
        }

        public async Task<(IEnumerable<SupplierDTO>, OperationResult<SupplierDTO>)> ExecuteAsync()
        {
            var result = new OperationResult<SupplierDTO>();
            var listDto = new List<SupplierDTO>();

            try
            {
                // 3. Validar Usuario y Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.SUPPLIER_CREATE.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());
                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameSupplier)));
                    return (listDto, result);
                }


                // 1. Configuramos y abrimos la conexión
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                // 2. AWAIT CRÍTICO: Esperamos a que la base de datos devuelva los datos
                var suppliers = await _uow.SupplierRepo.GetAllAsync();

                // 3. Mapeo limpio con LINQ
                listDto = suppliers.Select(s => SupplierMapper.ToDto(s)).ToList();

                // 4. Confirmamos y cerramos conexión
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                // 5. Manejo seguro de errores
                if (_uow.HasActiveTransaction)
                {
                    await _uow.RollbackAsync();
                }

                result.Errors.Add(new ErrorLogDTO
                {
                    Message = "Error al intentar obtener la lista de proveedores.",
                    InformativeMessage = ex.Message
                });
            }

            return (listDto, result);
        }
    }
}