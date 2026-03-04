using BLL.DTOs;
using BLL.DTOs.Errors; // Para usar el ErrorLogDTO
using Domain.Infrastructure;
using Shared; // Para IApplicationSettings
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

        public UCGetAllSuppliers(IUnitOfWork uow, IApplicationSettings appSettings)
        {
            _uow = uow;
            _appSettings = appSettings;
        }

        public async Task<(IEnumerable<SupplierDTO>, OperationResult<SupplierDTO>)> ExecuteAsync()
        {
            var result = new OperationResult<SupplierDTO>();
            var listDto = new List<SupplierDTO>();

            try
            {
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