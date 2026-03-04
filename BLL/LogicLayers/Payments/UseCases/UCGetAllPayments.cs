using BLL.DTOs;
using BLL.DTOs.Errors; // Para el ErrorLogDTO
using Domain.Infrastructure;
using Shared; // Para IApplicationSettings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Asumiendo que tienes un PaymentMapper en este namespace o similar
// using BLL.DTOs.Mappers; 

namespace BLL.LogicLayers.Payments
{
    public class UCGetAllPayments : IUCGetAllPayments
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;

        public UCGetAllPayments(IUnitOfWork uow, IApplicationSettings appSettings)
        {
            _uow = uow;
            _appSettings = appSettings;
        }

        public async Task<(IEnumerable<PaymentDTO>, OperationResult<PaymentDTO>)> ExecuteAsync()
        {
            var result = new OperationResult<PaymentDTO>();
            var listDto = new List<PaymentDTO>();

            try
            {
                // 1. Configuramos y abrimos la conexión
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                // 2. Esperamos la tarea (AWAIT CRÍTICO)
                var payments = await _uow.PaymentRepo.GetAllAsync(); // o GetAllAsync() según tu interfaz

                // 3. Mapeo con LINQ
                listDto = payments.Select(p => PaymentMapper.ToDto(p)).ToList();

                // 4. Confirmamos y cerramos conexión
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction)
                {
                    await _uow.RollbackAsync();
                }

                // Agregamos el error al resultado de la operación
                result.Errors.Add(new ErrorLogDTO
                {
                    Message = "Error al intentar obtener la lista de pagos.",
                    InformativeMessage = ex.Message
                });
            }

            return (listDto, result);
        }
    }
}