using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.BaseContracts;
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

namespace BLL.LogicLayers.Sales
{
    public class UCDeleteSale : IUCDeleteSale
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;
        private readonly string _tableNameSale;

        public UCDeleteSale
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
            _tableNameSale = _appSettings.SaleTableName ?? "Sales";
        }

        public async Task<OperationResult<SaleDTO>> ExecuteAsync(SaleDTO dto)
        {
            var result = new OperationResult<SaleDTO>();

            try
            {
                // 1. Validar Sesión Activa
                if (_sessionProvider.Current == null)
                {
                    var newError = _errorsFactory.Create(ErrorCatalogEnum.SessionExpired);
                    result.Errors.Add(ErrorMapper.ToDTO(newError));
                    return result;
                }


                // 2. Validar Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.SALE_DELETE.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());
                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameSale)));
                    return result;

                }

                // Conexión y Transacción
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();


                var entity = SaleMapper.ToEntity(dto);

                if (entity is ISoftDeletable)
                {
                    entity.MarkAsDeleted();                    // ← ahora propaga a los detalles
                    await _uow.SaleRepo.UpdateAsync(entity);   // ← UNA sola llamada (borra + reinserta TODO con IsDeleted=true)
                }


                // 5. Integridad Vertical (DVV)
                //         await UpdateDVVAsync(_tableNameSale, _appSettings.EntitiesConnection);

                // 6. Auditoría
                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.DeleteOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameSale,
                    extraInfo: $"Se eliminó la venta ID: {dto.Id} - Total: {dto.TotalAmount:C2} ({dto.Items?.Count() ?? 0} ítems)"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                // 7. Confirmar Transacción
                await _uow.CommitAsync();

                // 8. Retorno (devolvemos el DTO que nos pasaron para confirmar qué se borró)
                result.Value = dto;
                return result;
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                // Log técnico interno
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameSale;

                try { await _errorsRepository.CreateAsync(dbError); } catch { }

                // Manejo especial de restricciones (FK) - muy común en ventas
                ErrorLog uiError;

                if (ex.Message.Contains("REFERENCE constraint") || ex.Message.Contains("FK_") || ex.Message.Contains("The DELETE statement conflicted"))
                {
                    uiError = _errorsFactory.Create(ErrorCatalogEnum.DeleteRestriction, _tableNameSale);
                }
                else
                {
                    uiError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNameSale);
                }

                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id;
                errorDto.InformativeMessage = $"No se pudo eliminar la venta. Ref ID: {dbError.Id}";
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