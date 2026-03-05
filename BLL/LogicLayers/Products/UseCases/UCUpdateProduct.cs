using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Entities;
using Domain.Exceptions;
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
    public class UCUpdateProduct : IUCUpdateProduct
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;

        private readonly string _tableNameProduct;

        public UCUpdateProduct
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
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.SessionExpired)));
                    return result;
                }

                // Seteamos la conexión para validaciones
                _uow.SetConnectionString(_appSettings.EntitiesConnection);

                // 2. Validar Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);

                if (!currentUser.HasPermission("PRODUCT_UPDATE"))
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameProduct)));
                    return result;
                }

                // 3. Buscar Entidad y Validar Existencia (Tu patrón: null | IsDeleted)
                var existingProduct = await _uow.ProductRepo.GetByIdAsync(dto.Id);

                if (existingProduct == null | existingProduct.IsDeleted)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.NotFound, _tableNameProduct)));
                    return result;
                }

                // 4. Validación de Negocio: Nombre Duplicado (Si cambió el nombre)
                var productWithSameName = await _uow.ProductRepo.GetByNameAsync(dto.Name);
                if (productWithSameName != null && productWithSameName.Id != dto.Id)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameProduct)));
                    return result;
                }

                // 5. Mapeo a Entidad (Fail Fast de VOs en el Reconstitute)
                var productToUpdate = ProductMapper.ToEntity(dto);

                // 6. ABRIR TRANSACCIÓN
                await _uow.BeginTransactionAsync();

                // 7. Persistencia
                // El Repo actualizará la tabla Products y sincronizará ProductsCategories
                await _uow.ProductRepo.UpdateAsync(productToUpdate);

                // 8. Integridad Vertical (DVV): Obligatorio al modificar datos de la tabla
                await UpdateDVVAsync(_tableNameProduct, _appSettings.EntitiesConnection);

                // 9. Auditoría (Bitácora)
                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.UpdateOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameProduct,
                    extraInfo: $"Se actualizó el producto ID: {productToUpdate.Id} (Nombre: {productToUpdate.Name.Value})"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                // 10. Confirmación
                await _uow.CommitAsync();

                // 11. Retorno
                result.Value = ProductMapper.ToDto(productToUpdate);
                return result;
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                // Log técnico para soporte
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameProduct;
                try { await _errorsRepository.CreateAsync(dbError); } catch { }

                // Atrapamos errores de Dominio (Value Objects)
                if (ex is ArgumentException domainEx)
                {
                    var errorDto = ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNameProduct));
                    errorDto.InformativeMessage = domainEx.Message;
                    result.Errors.Add(errorDto);
                    return result;
                }

                // Error genérico de sistema
                var uiError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNameProduct);
                var resultError = ErrorMapper.ToDTO(uiError);
                resultError.LogId = dbError.Id;
                resultError.InformativeMessage = $"Error técnico al actualizar producto. Ref ID: {dbError.Id}";

                result.Errors.Add(resultError);
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