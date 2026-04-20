using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Exceptions.Base;
using Domain.Infrastructure;
using Domain.Infrastructure.Audit;
using Domain.Infrastructure.Permisos.Concrete;
using Domain.Repositories;
using Shared;
using Shared.Services;
using Shared.Sessions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Payments//=======================================================================REFACTORIZADO AL 14/04=======================================================================
{
    public class UCUpdatePayment : IUCUpdatePayment
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;

        private readonly string _tableNamePayment;
        private Guid _correlationId;

        public UCUpdatePayment
        (
            IUnitOfWork uow,
            IApplicationSettings appSettings,
            IBitacoraFactory bitacoraFact,
            ISessionProvider sessionProvider,
            IErrorsFactory errorsFactory,
            IErrorsRepository errorsRepository
        )
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _bitacoraFact = bitacoraFact ?? throw new ArgumentNullException(nameof(bitacoraFact));
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
            _errorsFactory = errorsFactory ?? throw new ArgumentNullException(nameof(errorsFactory));
            _errorsRepository = errorsRepository ?? throw new ArgumentNullException(nameof(errorsRepository));

            _tableNamePayment = appSettings.PaymentTableName ?? "Payments";
            _correlationId = Guid.NewGuid();
        }

        public async Task<OperationResult<PaymentDTO>> ExecuteAsync(PaymentDTO dto)
        {
            var result = new OperationResult<PaymentDTO>();

            try
            {
                // 1. Validar Sesión Activa
                if (_sessionProvider.Current == null)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.SessionExpired)));
                    return result;
                }

                // 2. Seteo de conexión para lectura
                _uow.SetConnectionString(_appSettings.EntitiesConnection);

                // 3. Validar Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.PAYMENT_UPDATE.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());

                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNamePayment)));
                    return result;
                }


                // 4. Buscar Entidad Existente y Validar Nulidad
                Payment existingPayment = await _uow.PaymentRepo.GetByIdAsync(dto.Id);

                // Aplicamos tu retoque de seguridad 
                if (existingPayment == null || existingPayment.IsDeleted)
                {
                    var notFoundError = _errorsFactory.Create(ErrorCatalogEnum.NotFound, _tableNamePayment);
                    result.Errors.Add(ErrorMapper.ToDTO(notFoundError));
                    return result;
                }


                // 5. Mapeo a Entidad (Fail Fast de VOs en el Reconstitute)
                var paymentToUpdate = PaymentMapper.ToEntity(dto);

                // 6. ABRIR TRANSACCIÓN
                await _uow.BeginTransactionAsync();

                // 7. Actualización de DVH por cambio de parámetro/s
                IntegrityFacade.RecalculateAndSetEntityDVH(paymentToUpdate);

                // 7. Persistencia
                await _uow.PaymentRepo.UpdateAsync(paymentToUpdate);

                // 8. Integridad Vertical (DVV) por cambio de fila
                await UpdateDVVAsync(_tableNamePayment, _appSettings.EntitiesConnection);

                // 9. Auditoría (Bitácora)
                var log = _bitacoraFact.Create
                (
                    entry: BitacoraCatalogEnum.UpdateOnBD,
                    user: currentUser.Id.ToString(),
                    sessionId: _sessionProvider.Current.Id,
                    correlationId: _correlationId,
                    tableName: _tableNamePayment,
                    extraInfo: $"Se actualizó el pago ID: {paymentToUpdate.Id} (Monto: $ {paymentToUpdate.Amount.Value}, N° Ref: {paymentToUpdate.Reference.Value})"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                // 10. Confirmación
                await _uow.CommitAsync();

                // 11. Retorno
                result.Value = PaymentMapper.ToDto(paymentToUpdate);
                return result;
            }

            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNamePayment;
                try
                {
                    await _errorsRepository.CreateAsync(dbError);
                }
                catch { }

                ErrorLog uiError = (ex.Message.Contains("REFERENCE constraint") || ex.Message.Contains("FK_"))
                    ? _errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNamePayment)
                    : _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNamePayment);

                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id;
                errorDto.InformativeMessage = $"Falla técnica al actualizar el pago. Ref ID: {dbError.Id}";

                result.Errors.Add(errorDto);
                return result;
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