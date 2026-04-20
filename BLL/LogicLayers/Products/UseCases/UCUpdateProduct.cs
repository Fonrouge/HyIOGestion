using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Entities;
using Domain.Entities.Products;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.Infrastructure.Audit;
using Domain.Infrastructure.Permisos.Concrete;
using Domain.Repositories;
using Shared;
using Shared.Services;
using Shared.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private Guid _correlationId;


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
            _correlationId = Guid.NewGuid();
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

                
                // 2.  Seteamos la conexión para validaciones
                _uow.SetConnectionString(_appSettings.EntitiesConnection);


                // 3. Validar Permisos
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.PRODUCT_UPDATE.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());
                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameProduct)));
                    return result;
                }


                // 4. Buscar Entidad y Validar Existencia (Tu patrón: null | IsDeleted)
                var entity = await _uow.ProductRepo.GetByIdAsync(dto.Id);

                if (entity == null || entity.IsDeleted)
                {
                    var notFoundError = _errorsFactory.Create(ErrorCatalogEnum.NotFound, _tableNameProduct);
                    result.Errors.Add(ErrorMapper.ToDTO(notFoundError));
                    return result;
                }


                // 5. Validación de Negocio: Nombre Duplicado (Si cambió el nombre)
                var productWithSameName = await _uow.ProductRepo.GetByNameAsync(dto.Name);

                if (productWithSameName != null && productWithSameName.Id != dto.Id && !productWithSameName.IsDeleted)
                {
                    var dupError = _errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameProduct);
                    result.Errors.Add(ErrorMapper.ToDTO(dupError));
                    return result;
                }


                // 6. Ignoramos campos que no son editables por el usuario o técnicos y obtenemos info detallada de cambios (para bitácora/log)
                string changedValues = IntegrityFacade.GetDelta(entity, dto, "Id", "DVH");


                // 7. Entity, que ya sabemos que existe y no está duplicada, adquiere los datos del nuevo producto proveniente de front.
                entity = ProductMapper.ToEntity(dto);


                // 8. ABRIR TRANSACCIÓN antes de persistir entidades
                await _uow.BeginTransactionAsync();


                // 9. Como la relación Producto-Categoría tiene DVH, éste debe ser calculado previo persistir.                
                var relaciones = entity.Categories.Select(cat =>
                {
                    var rel = ProductCategoryDTO.Create(entity.Id, cat.Id); // 8.1 - Se linkean las relaciones
                    IntegrityFacade.RecalculateAndSetEntityDVH(rel); // 8.2 - Se calcula el DVH de cada relación
                    return rel;

                }).ToList();



                // 10. Después de modificada la entidad, se recalcula el DVH.
                IntegrityFacade.RecalculateAndSetEntityDVH(entity);


                // 11. Pasamos todo al repositorio para persistencia
                await _uow.ProductRepo.UpdateAsync(entity, relaciones);
          

                // 12. Integridad Vertical (DVV): Obligatorio al modificar datos de la tabla
                await UpdateDVVAsync(_tableNameProduct, _appSettings.EntitiesConnection);
                await UpdateDVVAsyncWithNoId(_appSettings.ProductCategoryTableName, _appSettings.EntitiesConnection);

                // 13. Auditoría (Bitácora)
                var log = _bitacoraFact.Create
                (
                    entry: BitacoraCatalogEnum.UpdateOnBD,
                    user: _sessionProvider.Current.CurrentUserId.ToString(),
                    tableName: _tableNameProduct,
                    sessionId: _sessionProvider.Current.Id,
                    correlationId: _correlationId,
                    extraInfo: $"Se actualizó el producto ID: {entity.Id} - Detalles: {changedValues}"
                );

                await _uow.BitacoraRepo.CreateAsync(log);

                // 14. Confirmación
                await _uow.CommitAsync();

                // 15. Retorno
                result.Value = ProductMapper.ToDto(entity);
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
        private async Task UpdateDVVAsyncWithNoId(string nombreTabla, string connectionString)
        {
            var hashes = await _uow.IntegrityRepo.GetVerticalHashesAsync(nombreTabla, connectionString, false);
            var dvvFinal = IntegrityService.CalculateDVV(hashes);
            await _uow.IntegrityRepo.UpdateDVVAsync(nombreTabla, dvvFinal, connectionString);
        }
    }
}