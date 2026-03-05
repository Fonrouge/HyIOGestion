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
using Domain.Repositories;
using Shared;
using Shared.Services;
using Shared.Sessions;
using System;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Products
{
    public class UCDeleteProduct : IUCDeleteProduct
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;

        private readonly string _tableNameProduct;

        public UCDeleteProduct
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

            _tableNameProduct = appSettings.ProductTableName ?? "Products";
        }

        public async Task<OperationResult<ProductDTO>> ExecuteAsync(ProductDTO dto)
        {
            var result = new OperationResult<ProductDTO>();

            try
            {
                // 1. Validar Sesión Activa
                if (_sessionProvider.Current == null)
                {
                    var newError = _errorsFactory.Create(ErrorCatalogEnum.SessionExpired);
                    result.Errors.Add(ErrorMapper.ToDTO(newError));
                    return result;
                }

                // 2. Validar Permisos (Patente específica)
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                if (!currentUser.HasPermission("PRODUCT_DELETE"))
                {
                    var authError = _errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameProduct);
                    result.Errors.Add(ErrorMapper.ToDTO(authError));
                    return result;
                }

                // 3. Conexión y Transacción
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                // 4. Buscar Entidad y Validar Existencia (Patrón null | IsDeleted)
                Product entity = await _uow.ProductRepo.GetByIdAsync(dto.Id);

                if (entity == null | entity.IsDeleted)
                {
                    var notFoundError = _errorsFactory.Create(ErrorCatalogEnum.NotFound, _tableNameProduct);
                    result.Errors.Add(ErrorMapper.ToDTO(notFoundError));
                    await _uow.RollbackAsync();
                    return result;
                }

                // 5. Eliminación (Lógica o Física)
                if (entity is ISoftDeletable)
                {
                    entity.MarkAsDeleted();
                    await _uow.ProductRepo.UpdateAsync(entity);
                }
                else
                {
                    await _uow.ProductRepo.DeleteAsync(dto.Id);
                }

                // 6. Integridad Vertical (DVV)
                await UpdateDVVAsync(_tableNameProduct, _appSettings.EntitiesConnection);

                // 7. Auditoría
                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.DeleteOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameProduct,
                    extraInfo: $"Se eliminó el producto ID: {dto.Id} (Nombre: {dto.Name})"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                // 8. Confirmación
                await _uow.CommitAsync();

                result.Value = dto;
                return result;
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                // Log técnico
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameProduct;

                try { await _errorsRepository.CreateAsync(dbError); } catch { }

                // Mapeo de error para UI
                ErrorLog uiError;

                if (ex.Message.Contains("REFERENCE constraint") || ex.Message.Contains("FK_"))
                    uiError = _errorsFactory.Create(ErrorCatalogEnum.DeleteRestriction, _tableNameProduct);
                else
                    uiError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNameProduct);

                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id;
                errorDto.InformativeMessage = $"Falla técnica al eliminar producto. Ref ID: {dbError.Id}";

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