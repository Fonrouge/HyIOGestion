using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.Infrastructure.Audit;
using Domain.Infrastructure.Permisos.Concrete;
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
                // 1. Seteamos la conexión de SEGURIDAD primero para las validaciones
                _uow.SetConnectionString(_appSettings.SecurityConnection);

                if (_sessionProvider.Current == null)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.SessionExpired)));
                    return result;
                }

                // 2. Validar Permisos (Ahora funcionará porque las queries son Cross-DB o usan SecurityConnection)
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                var permissionsList = await _uow.PermisoRepo.GetPermissionsByUserAsync(_sessionProvider.Current.CurrentUserId);

                bool hasAccess = permissionsList.Any(p => p.PermisoCode == PermisosEnum.PRODUCT_CREATE.ToString()
                                                       || p.PermisoCode == PermisosEnum.ADMIN_ACCESS.ToString());
                if (!hasAccess)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameProduct)));
                    return result;
                }

                // 3. Cambiamos a la conexión de ENTIDADES para el trabajo pesado
                _uow.SetConnectionString(_appSettings.EntitiesConnection);

                // 4. Validaciones de Negocio
                var existing = await _uow.ProductRepo.GetByNameAsync(dto.Name);
                if (existing != null)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameProduct)));
                    return result;
                }

                // 5. Instanciar y Transaccionar
                var newProduct = Product.Create(dto.Name, dto.Description, dto.Price, dto.Stock, CategoryMapper.ToListEntity(dto.Categories));

                await _uow.BeginTransactionAsync();

                await _uow.ProductRepo.CreateAsync(newProduct);

                // 6. Auditoría (Bitácora)            
                var log = _bitacoraFact.Create
                (
                    entry: BitacoraCatalogEnum.CreateOnBD, 
                    currentUser.Id.ToString(),
                    tableName: _tableNameProduct,
                    sessionId: _sessionProvider.Current.Id,
                    correlationId: Guid.NewGuid(),
                    extraInfo: $"Creado: {newProduct.Name.Value}"
                );

                await _uow.BitacoraRepo.CreateAsync(log);

                await _uow.CommitAsync();

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