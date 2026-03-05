using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Exceptions.Base;
using Domain.Infrastructure;
using Domain.Infrastructure.Audit;
using Domain.Repositories;
using Shared;
using Shared.Services;
using Shared.Sessions;
using System;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Payments
{
    public class UCUpdatePayment : IUCUpdatePayment // Asumo que tenés la interfaz creada
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;

        private readonly string _tableNamePayment;

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

                // Seteamos la conexión para lectura
                _uow.SetConnectionString(_appSettings.EntitiesConnection);

                // 2. Validar Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                if (!currentUser.HasPermission("PAYMENT_UPDATE"))
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNamePayment)));
                    return result;
                }

                // 3. Buscar Entidad Existente y Validar Nulidad
                Payment existingPayment = await _uow.PaymentRepo.GetByIdAsync(dto.Id);

                // Aplicamos tu retoque de seguridad (Asumiendo que le pusiste la interfaz ISoftDeletable o la propiedad IsDeleted a Payment)
                // Si Payment no tiene IsDeleted en tu dominio, sacá la segunda parte del IF.
                if (existingPayment == null /* || existingPayment.IsDeleted */)
                {
                    // Usando la Factory como pediste en lugar del "new ErrorDTO"
                    var notFoundError = _errorsFactory.Create(ErrorCatalogEnum.NotFound, _tableNamePayment);
                    result.Errors.Add(ErrorMapper.ToDTO(notFoundError));
                    return result;
                }

                // 4. Mapeo a Entidad (Fail Fast de VOs en el Reconstitute)
                var paymentToUpdate = PaymentMapper.ToEntity(dto);

                // 5. ABRIR TRANSACCIÓN
                await _uow.BeginTransactionAsync();

                // 6. Persistencia
                await _uow.PaymentRepo.UpdateAsync(paymentToUpdate);

                // 7. Integridad Vertical (DVV)
                // Como cambió un DVH de la tabla, la firma de toda la tabla cambió. Hay que recalcular el DVV.
                await UpdateDVVAsync(_tableNamePayment, _appSettings.EntitiesConnection);

                // 8. Auditoría (Bitácora)
                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.UpdateOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNamePayment,
                    extraInfo: $"Se actualizó el pago ID: {paymentToUpdate.Id} (Monto: $ {paymentToUpdate.Amount.Value}, Ref: {paymentToUpdate.Reference.Value})"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                // 9. Confirmación
                await _uow.CommitAsync();

                // 10. Retorno
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