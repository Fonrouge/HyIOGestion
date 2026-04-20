using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.Infrastructure.Audit;
using Domain.Infrastructure.Permisos.Concrete;
using Domain.Repositories;
using Shared;
using Shared.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Payments
{
    public class UCGetAllPayments : IUCGetAllPayments //=======================================================================REFACTORIZADO AL 14/04=======================================================================
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly IErrorsRepository _errorsRepository;


        private readonly string _tableNamePayment;
        private Guid _correlationId;

        public UCGetAllPayments
        (
            IUnitOfWork uow,
            IApplicationSettings appSettings,
            ISessionProvider sessionProvider,
            IErrorsFactory errorsFactory,
            IBitacoraFactory bitacoraFact,
            IErrorsRepository errorsRepository
        )
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
            _errorsFactory = errorsFactory ?? throw new ArgumentNullException(nameof(errorsFactory));
            _bitacoraFact = bitacoraFact ?? throw new ArgumentNullException(nameof(bitacoraFact));
            _errorsRepository = errorsRepository ?? throw new ArgumentNullException(nameof(errorsRepository));

            _tableNamePayment = _appSettings.PaymentTableName ?? "Payments";
            _correlationId = Guid.NewGuid();
        }

        public async Task<(IEnumerable<PaymentDTO>, OperationResult<PaymentDTO>)> ExecuteAsync()
        {
            var result = new OperationResult<PaymentDTO>();
            var listDto = new List<PaymentDTO>();

            try
            {
                // 1. Fail-Fast: Sesión
                if (_sessionProvider.Current == null)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.SessionExpired)));
                    return (listDto, result);
                }

                // 2. Validar Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.PAYMENT_VIEW.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());

                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNamePayment)));
                    return (listDto, result);
                }


                // 3. Configuramos la conexión
                _uow.SetConnectionString(_appSettings.EntitiesConnection);

                // 4. Esperamos la tarea (AWAIT CRÍTICO)
                var payments = await _uow.PaymentRepo.GetAllAsync(); // o GetAllAsync() según tu interfaz

                // 5. Mapeo con LINQ
                listDto = payments.Select(p => PaymentMapper.ToDto(p)).ToList();

                // 6. Auditoría (Bitácora)
                var log = _bitacoraFact.Create
                (
                    entry: BitacoraCatalogEnum.GetAllFromDB,
                    user: currentUser.Id.ToString(),
                    sessionId: _sessionProvider.Current.Id,
                    correlationId: _correlationId,
                    tableName: _tableNamePayment,
                    extraInfo: $"Se extrajeron todos los datos de la tabla {_tableNamePayment} para su visualización."
                );
            
                await _uow.BitacoraRepo.CreateAsync(log);
            }

            catch (Exception ex)
            {
                // 1. Log técnico interno
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNamePayment;

                try
                {
                    await _errorsRepository.CreateAsync(dbError);
                }
                catch { }

                // 2. Error catalogado para el usuario
                var uiError = _errorsFactory.Create(ErrorCatalogEnum.DataLoadError, _tableNamePayment);

                // 3. Mapeo y vinculación de trazabilidad para soporte técnico
                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id;
                errorDto.InformativeMessage = $"Se falló en la obtención de los pagos. Si se comunica con soporte, utilice este código para una mejor asistencia ({dbError.Id}). Si no desea hacerlo, por favor reinicie el programa e intente nuevamente.";

                result.Errors.Add(errorDto);
            }

            return (listDto, result);
        }
    }
}