using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Contracts;
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

namespace BLL.LogicLayers.Payments
{
    public class UCDeletePayment : IUCDeletePayment
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;

        private readonly string _tableNamePayment;

        public UCDeletePayment
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

                // Seteamos la conexión para lectura (Sin abrir transacción aún)
                _uow.SetConnectionString(_appSettings.EntitiesConnection);

                // 2. Validar Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.PAYMENT_DELETE.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());
                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNamePayment)));
                    return result;
                }

                // 3. Buscar Entidad a Eliminar (Para dejar registro en bitácora de qué borramos)
                Payment entity = await _uow.PaymentRepo.GetByIdAsync(dto.Id);
                if (entity == null | entity.IsDeleted)
                {
                    // Manejo elegante si no se encuentra el registro
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.NotFound, _tableNamePayment)));
                    await _uow.RollbackAsync();
                    return result;
                }

                // 4. ABRIR TRANSACCIÓN
                await _uow.BeginTransactionAsync();


                // 5. Eliminación (Hard o Soft) 
                if (entity is ISoftDeletable)
                {
                    entity.MarkAsDeleted();
                    await _uow.PaymentRepo.UpdateAsync(entity); // Sin calcular DVH, directo a guardar
                }
                else
                {
                    await _uow.PaymentRepo.DeleteAsync(dto.Id);
                }


                // 6. Integridad Vertical (DVV) - OBLIGATORIO
                // Al eliminar físicamente un registro de una tabla blindada, el DVV debe recalcularse.
          //      await UpdateDVVAsync(_tableNamePayment, _appSettings.EntitiesConnection);

                // 7. Auditoría (Bitácora)
                // Dejamos un log muy claro de qué se voló físicamente
                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.SoftDeleteOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNamePayment,
                    sessionId: _sessionProvider.Current.Id,
                    correlationId: Guid.NewGuid(),
                    extraInfo: $"Se eliminó FÍSICAMENTE el pago ID: {dto.Id} (Monto original: $ {entity.Amount.Value}, Cliente: {entity.SaleId})"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                // 8. Confirmación
                await _uow.CommitAsync();

                // 9. Retorno
                result.Value = dto;
                return result;
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNamePayment;
                try { await _errorsRepository.CreateAsync(dbError); } catch { }

                ErrorLog uiError = (ex.Message.Contains("REFERENCE constraint") || ex.Message.Contains("FK_"))
                    ? _errorsFactory.Create(ErrorCatalogEnum.DeleteRestriction, _tableNamePayment)
                    : _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNamePayment);

                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id;
                errorDto.InformativeMessage = $"Falla técnica al eliminar el pago. Ref ID: {dbError.Id}";

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