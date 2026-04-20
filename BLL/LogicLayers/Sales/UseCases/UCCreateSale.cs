using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.DTOs.Mappers;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Entities;
using Domain.Exceptions;
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
    public class UCCreateSale : IUCCreateSale
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;
        
        private readonly string _tableNameSale;
        private Guid _correlationId;

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
            _correlationId = Guid.NewGuid();
        }

        public async Task<OperationResult<SaleDTO>> ExecuteAsync(SaleDTO dto)
        {
            var result = new OperationResult<SaleDTO>();

            try
            {
                // 1. Validar Sesión Activa (Barato, en memoria)
                if (_sessionProvider.Current == null)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.SessionExpired)));
                    return result;
                }

                // 2. Regla de negocio mínima (vender algo)
                if (dto.Items == null || !dto.Items.Any())
                {
                    result.Errors.Add(new ErrorLogDTO { InformativeMessage = "No se puede crear una venta sin productos." });
                    return result;
                }

                _uow.SetConnectionString(_appSettings.EntitiesConnection);

                // 2. Validar Permisos
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.SALE_CREATE.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());
                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameSale)));
                    return result;
                }

                // 4. Mapeo a Entidad (Fail Fast de VOs ocurre acá)
                var newSale = SaleMapper.ToEntity(dto);

                // 5. INICIO DE TRANSACCIÓN (Ahora sí, para los cambios de datos)
                await _uow.BeginTransactionAsync();

                // 6. CONTROL DE STOCK Y ACTUALIZACIÓN
                foreach (var item in newSale.Items)
                {
                    // --- DENTRO DEL LOOP DE ÍTEMS EN UCCreateSale ---

                    var product = await _uow.ProductRepo.GetByIdAsync(item.ProductId);

                    if (product == null || product.IsDeleted)
                    {
                        var notFound = _errorsFactory.Create(ErrorCatalogEnum.NotFound, "Products");
                        result.Errors.Add(ErrorMapper.ToDTO(notFound));
                        await _uow.RollbackAsync();
                        return result;
                    }

                    try
                    {
                        // 1. El Producto valida su propio stock e invariantes internamente. Si no hay stock, lanza una InvalidOperationException con el mensaje definido
                        product.ReduceStock((decimal)item.Quantity.Value);

                        // 2. Producto mmodificado = Parámetros modificados = Recálculo de DVH
                        IntegrityFacade.RecalculateAndSetEntityDVH(product);

                        // 2. Si pasó el punto uno, significa que el stock es válido y ya se restó en memoria..
                        // Se persiste el cambio de estado del producto dentro de la misma transacción.
                        await _uow.ProductRepo.UpdateAsync(product);
                    }
                    catch (InvalidOperationException ex)
                    {
                        // 3. Atrapamos el error de negocio del dominio y lo transformamos en un error de UI
                        var stockError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, "Products");
                        var errorDto = ErrorMapper.ToDTO(stockError);

                        // Le pasamos el mensaje exacto que generó el Producto (Ej: "Stock insuficiente para...")
                        errorDto.InformativeMessage = ex.Message;
                        result.Errors.Add(errorDto);

                        await _uow.RollbackAsync();
                        return result;
                    }
                }

                // 7. Se firma TODA la estructura antes de que toque el Repo (Sale + SaleDetails + Products)
                
                // 7.1. Primero a los hijos -los detalles- > (El producto actualiza su DVH en el bloque 6)
                foreach (var item in newSale.Items)
                {
                    IntegrityFacade.RecalculateAndSetEntityDVH(item);
                }

                // Luego el padre (la venta)
                IntegrityFacade.RecalculateAndSetEntityDVH(newSale);
                
                               
                // 8. Persistencia de la Venta (que internamente guardará los detalles ya firmados)
                await _uow.SaleRepo.CreateAsync(newSale);


                // 9. Integridad Vertical (DVV) de todas las tablas involucradasx1
                await UpdateDVVAsync(_tableNameSale, _appSettings.EntitiesConnection);
                await UpdateDVVAsync(_appSettings.SaleDetailTableName, _appSettings.EntitiesConnection);
                await UpdateDVVAsync(_appSettings.ProductTableName, _appSettings.EntitiesConnection);


                // 10. Auditoría
                var log = _bitacoraFact.Create
                (
                    entry: BitacoraCatalogEnum.CreateOnBD,
                    user: _sessionProvider.Current.CurrentUserId.ToString(),
                    tableName: _tableNameSale,
                    sessionId: _sessionProvider.Current.Id,
                    correlationId: _correlationId,
                    extraInfo: $"Venta Id {newSale.Id} - Total: ${newSale.TotalAmount.Value:C2}"
                );

                await _uow.BitacoraRepo.CreateAsync(log);

                // 11. Confirmación
                await _uow.CommitAsync();

                result.Value = SaleMapper.ToDto(newSale);
            }

            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameSale;

                try
                {
                    await _errorsRepository.CreateAsync(dbError);
                }
                catch { }


                // Emitimos error limpio a la UI
                var uiError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNameSale);
                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id;
                errorDto.InformativeMessage = $"Hubo un problema técnico al procesar la venta. " +
                    $"Por favor, contacte a soporte e indique el código: {dbError.Id}." +
                    $"Si en este momento no puede contactarse, por favor reinicie la aplicación e intente nuevamente.";

                result.Errors.Add(errorDto);
            }

            return result;
        }

        private async Task UpdateDVVAsync(string nombreTabla, string connectionString)
        {
            var hashes = await _uow.IntegrityRepo.GetVerticalHashesAsync(nombreTabla, connectionString);
            var dvvFinal = IntegrityService.CalculateDVV(hashes);
            await _uow.IntegrityRepo.UpdateDVVAsync(nombreTabla, dvvFinal, connectionString);
        }
    }
}