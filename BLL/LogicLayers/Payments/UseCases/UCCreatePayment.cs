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
    public class UCCreatePayment : IUCCreatePayment // Asumo que tenés esta interfaz
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;

        private readonly string _tableNamePayment;

        public UCCreatePayment
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

                // Seteamos la conexión (Aún SIN abrir transacción)
                _uow.SetConnectionString(_appSettings.EntitiesConnection);

                // 2. Validar Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                if (!currentUser.HasPermission("PAYMENT_CREATE"))
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNamePayment)));
                    return result;
                }

                // 3. Validar Negocio: Verificar que el Cliente exista antes de registrarle un pago
                var client = await _uow.ClientRepo.GetByIdAsync(dto.ClientId);
                if (client == null)
                {
                    result.Errors.Add(new ErrorLogDTO { InformativeMessage = "El Cliente especificado no existe o fue eliminado." });
                    return result;
                }

                // 4. Instanciar Entidad (Fail Fast de VOs: Monto negativo, método vacío, etc.)
                // Como es una creación nueva, NO usamos el Mapper (que usa Reconstitute), usamos el Factory estático
                var newPayment = Payment.Create(
                    rawAmount: dto.Amount,
                    clientId: dto.ClientId,
                    rawMethod: dto.Method,
                    rawReference: dto.Reference
                );

                // --- INICIO BLOQUE DE INTEGRIDAD CRIPTOGRÁFICA (CORE FINANCIERO) ---

             //   // 5. Calcular DVH (Horizontal) para la nueva fila
             //   var dvh = IntegrityService.CalculateDVH(newPayment);
             //   newPayment.UpdateDVH(dvh);

                // 6. ABRIR TRANSACCIÓN (Solo cuando estamos listos para guardar)
                await _uow.BeginTransactionAsync();

                // 7. Persistencia
                await _uow.PaymentRepo.CreateAsync(newPayment);

                // 8. Calcular y Actualizar DVV (Vertical) - ¡Fundamental tras un Insert!
                await UpdateDVVAsync(_tableNamePayment, _appSettings.EntitiesConnection);

                // --- FIN BLOQUE DE INTEGRIDAD CRIPTOGRÁFICA ---

                // 9. Auditoría (Bitácora)
                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.CreateOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNamePayment,
                    extraInfo: $"Se registró un pago de $ {newPayment.Amount.Value} para el Cliente ID: {newPayment.ClientId} (Ref: {newPayment.Reference.Value})"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                // 10. Confirmación
                await _uow.CommitAsync();

                // 11. Retorno (Actualizamos el DTO con el ID generado y las fechas asignadas por el Dominio)
                result.Value = PaymentMapper.ToDto(newPayment);
                return result;
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                // Log técnico interno
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNamePayment;
                try { await _errorsRepository.CreateAsync(dbError); } catch { }

                // Atrapar excepciones de Dominio (Fail Fast de los VOs del Payment)
                if (ex is ArgumentException domainEx)
                {
                    result.Errors.Add(new ErrorLogDTO { InformativeMessage = domainEx.Message });
                    return result;
                }

                // Manejo de restricciones FK (Ej: si se coló un ClientId inválido)
                ErrorLog uiError = (ex.Message.Contains("REFERENCE constraint") || ex.Message.Contains("FK_"))
                    ? _errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNamePayment) // O un error específico de FK
                    : _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNamePayment);

                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id;
                errorDto.InformativeMessage = $"Falla técnica al registrar el pago. Ref ID: {dbError.Id}";

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