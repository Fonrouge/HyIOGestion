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
using Shared.Sessions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Products
{
    public class UCCreateProduct : IUCCreateProduct //=======================================================================REFACTORIZADO AL 27/02=======================================================================
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;

        private readonly string _tableNameProduct;

        public UCCreateProduct
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

                // Seteamos la conexión (Lectura inicial)
                _uow.SetConnectionString(_appSettings.EntitiesConnection);

                // 2. Validar Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                if (!currentUser.HasPermission("PRODUCT_CREATE"))
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameProduct)));
                    return result;
                }

                // 3. Validaciones de Negocio: Nombre Duplicado
                // Usamos el nombre del DTO para chequear antes de instanciar el VO
                var existing = await _uow.ProductRepo.GetByNameAsync(dto.Name);
                if (existing != null)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameProduct)));
                    return result;
                }

                // 4. Instanciar Entidad (Fail Fast de VOs: Precio, Stock, Nombre)
                // Usamos el Factory Method Create del Dominio
                var newProduct = Product.Create(
                    rawName: dto.Name,
                    rawDescription: dto.Description,
                    rawPrice: dto.Price,
                    rawStock: dto.Stock,
                    categories: CategoryMapper.ToListEntity(dto.Categories)
                );

                // 5. ABRIR TRANSACCIÓN
                await _uow.BeginTransactionAsync();

                // 6. Persistencia (El repo se encarga de la tabla base y la intermedia de categorías)
                await _uow.ProductRepo.CreateAsync(newProduct);

                // 7. Auditoría (Bitácora)
                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.CreateOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameProduct,
                    extraInfo: $"Producto Creado: {newProduct.Name.Value} | Stock Inicial: {newProduct.Stock.Value}"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                // 8. Confirmación
                await _uow.CommitAsync();

                // 9. Retorno (Mapeamos la entidad real creada a DTO)
                result.Value = ProductMapper.ToDto(newProduct);
                return result;
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();

                // Log técnico interno para el desarrollador
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameProduct;
                try { await _errorsRepository.CreateAsync(dbError); } catch { }

                // Atrapamos excepciones de los Value Objects (Dominio gritando)
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
                resultError.InformativeMessage = $"Error al crear producto. Ref ID: {dbError.Id}";

                result.Errors.Add(resultError);
                return result;
            }
        }
    }
}