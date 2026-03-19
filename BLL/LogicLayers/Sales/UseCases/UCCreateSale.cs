using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.DTOs.Mappers;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
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
                // 1. Validar Sesión Activa (Barato, en memoria)
                if (_sessionProvider.Current == null)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.SessionExpired)));
                    return result;
                }

                // 2. Validar Input Básico (No vender aire)
                if (dto.Items == null || !dto.Items.Any())
                {
                    result.Errors.Add(new ErrorLogDTO { InformativeMessage = "No se puede crear una venta sin productos." });
                    return result;
                }

                _uow.SetConnectionString(_appSettings.EntitiesConnection);

                // 2. Validar Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
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
                        // 1. El Producto valida su propio stock e invariantes internamente
                        // Si no hay stock, lanza una InvalidOperationException con el mensaje que definimos
                        product.ReduceStock(item.Quantity.Value);

                        // 2. Si pasó la línea anterior, el stock ya se restó en memoria del objeto.
                        // Persistimos el cambio de estado del producto dentro de la misma transacción.
                        await _uow.ProductRepo.UpdateAsync(product);
                    }
                    catch (InvalidOperationException ex)
                    {
                        // 3. Atrapamos el error de negocio del dominio y lo transformamos en un error de UI
                        // Usamos InternalError o podrías usar uno nuevo como 'BusinessRuleViolation'
                        var stockError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, "Products");
                        var errorDto = ErrorMapper.ToDTO(stockError);

                        // Le pasamos el mensaje exacto que generó el Producto (Ej: "Stock insuficiente para...")
                        errorDto.InformativeMessage = ex.Message;
                        result.Errors.Add(errorDto);

                        await _uow.RollbackAsync();
                        return result;
                    }
                }

                // 7. Persistencia de la Venta
                await _uow.SaleRepo.CreateAsync(newSale);

                // 8. Integridad Vertical (DVV) - ¡ACTIVALO! Es dinero.
    //            await UpdateDVVAsync(_tableNameSale, _appSettings.EntitiesConnection);

                // 9. Auditoría
                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.CreateOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameSale,
                    extraInfo: $"Venta #{newSale.Id} - Total: {newSale.TotalAmount.Value:C2}"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                // 10. Confirmación
                await _uow.CommitAsync();

                result.Value = SaleMapper.ToDto(newSale);
                return result;
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                // todavía me falta hacer esto
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