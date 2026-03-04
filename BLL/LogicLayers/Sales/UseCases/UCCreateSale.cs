using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.DTOs.Mappers;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.Infrastructure.Audit;
using Domain.Repositories;
using Shared;
using Shared.Services;
using Shared.Sessions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Sales
{
    public class UCCreateSale : IUCCreateSale
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;
        private readonly string _tableNameSale;

        public UCCreateSale
        (
            IUnitOfWork uow,
            IApplicationSettings appSettings,
            IBitacoraFactory bitacoraFact,
            ISessionProvider sessionProvider,
            IErrorsFactory errorsFactory,
            IErrorsRepository errorsRepository)
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

                // 2. Conexión y Transacción
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                // 3. Validar Permisos
                var currentUser = await _uow.UserRepo.GetById(_sessionProvider.Current.CurrentUserId);
                if (!currentUser.HasPermission("SALE_CREATE"))
                {
                    var authError = _errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameSale);
                    result.Errors.Add(ErrorMapper.ToDTO(authError));
                    return result;
                }

                // 4. Validar Duplicados (ejemplo: por Cliente + Fecha - ajustar según reglas de negocio)
                // var existing = await _uow.SaleRepo.GetByClientAndDateAsync(dto.ClientId, dto.Date);
                // if (existing != null)
                // {
                //     var dupError = _errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameSale);
                //     result.Errors.Add(ErrorMapper.ToDTO(dupError));
                //     return result;
                // }

                // 5. Mapeo DTO → Entidad (Rich Domain Model)
                var newSale = SaleMapper.ToEntity(dto);

                // 6. Persistencia (el repositorio ya maneja los SaleDetails del agregado)
                await _uow.SaleRepo.Create(newSale);

                //    // 7. Integridad Vertical (DVV)
                //    await UpdateDVVAsync(_tableNameSale, _appSettings.EntitiesConnection);

                // 8. Auditoría
                var log = _bitacoraFact.Create
                (
                    entry: BitacoraCatalogEnum.CreateOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameSale,
                    extraInfo: $"Se creó la venta #{newSale.Id} - Cliente: {newSale.ClientId} - Total: {newSale.TotalAmount.Value:C2} ({newSale.Items.Count()} ítems)"
                );

                await _uow.BitacoraRepo.CreateAsync(log);

                // 9. Confirmar Transacción
                await _uow.CommitAsync();

                // 10. Retorno
                result.Value = SaleMapper.ToDto(newSale);
                return result;
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                // Log técnico
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameSale;

                try
                {
                    await _errorsRepository.CreateAsync(dbError);
                }
                catch { }

                // Error UI
                var uiError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNameSale);
                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id;
                errorDto.InformativeMessage = $"Falla técnica al crear venta. Ref ID: {dbError.Id}";
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