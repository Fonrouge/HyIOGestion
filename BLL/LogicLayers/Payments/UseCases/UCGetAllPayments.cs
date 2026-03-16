using BLL.DTOs;
using BLL.DTOs.Errors; // Para el ErrorLogDTO
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

// Asumiendo que tienes un PaymentMapper en este namespace o similar
// using BLL.DTOs.Mappers; 

namespace BLL.LogicLayers.Payments
{
    public class UCGetAllPayments : IUCGetAllPayments
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        
        private readonly string _tableNamePayment;

        public UCGetAllPayments
        (   
            IUnitOfWork uow, 
            IApplicationSettings appSettings, 
            ISessionProvider sessionProvider, 
            IErrorsFactory errorsFactory
        )
        {
            _uow = uow;
            _appSettings = appSettings;
            _sessionProvider = sessionProvider;
            _errorsFactory = errorsFactory;

            _tableNamePayment = _appSettings.PaymentTableName;
        }

        public async Task<(IEnumerable<PaymentDTO>, OperationResult<PaymentDTO>)> ExecuteAsync()
        {
            var result = new OperationResult<PaymentDTO>();
            var listDto = new List<PaymentDTO>();

            try
            {
                // 1. Validar Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.PAYMENT_VIEW.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());
                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNamePayment)));
                    return (listDto, result);
                }


                // 2. Configuramos y abrimos la conexión
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                // 3. Esperamos la tarea (AWAIT CRÍTICO)
                var payments = await _uow.PaymentRepo.GetAllAsync(); // o GetAllAsync() según tu interfaz

                // 4. Mapeo con LINQ
                listDto = payments.Select(p => PaymentMapper.ToDto(p)).ToList();

                // 5. Confirmamos y cerramos conexión
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