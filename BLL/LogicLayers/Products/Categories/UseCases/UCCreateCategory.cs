using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using BLL.LogicLayers.Products.Categories.UseCases;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.Infrastructure.Audit;
using Domain.Repositories;
using Shared;
using Shared.Services;
using Shared.Sessions;
using System;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Categories
{
    public class UCCreateCategory : IUCCreateCategory
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;
        private readonly string _tableNameCategory;

        public UCCreateCategory
        (
            IUnitOfWork uow,
            IApplicationSettings appSettings,
            IBitacoraFactory bitacoraFact,
            ISessionProvider sessionProvider,
            IErrorsFactory errorsFactory,
            IErrorsRepository errorsRepository
        )
        {
            _uow = uow;
            _appSettings = appSettings;
            _bitacoraFact = bitacoraFact;
            _sessionProvider = sessionProvider;
            _errorsFactory = errorsFactory;
            _errorsRepository = errorsRepository;
            _tableNameCategory = appSettings.CategoryTableName ?? "Categories";
        }

        public async Task<OperationResult<CategoryDTO>> ExecuteAsync(CategoryDTO dto)
        {
            var result = new OperationResult<CategoryDTO>();
            try
            {
                if (_sessionProvider.Current == null)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.SessionExpired)));
                    return result;
                }

                _uow.SetConnectionString(_appSettings.EntitiesConnection);

                // 1. Validar Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                if (!currentUser.HasPermission("CATEGORY_CREATE"))
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameCategory)));
                    return result;
                }

                // 2. Validar Duplicados (Negocio)
                var existing = await _uow.CategoryRepo.GetByNameAsync(dto.Name);
                if (existing != null)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.DuplicateEntry, _tableNameCategory)));
                    return result;
                }

                // 3. Mapeo y Transacción
                var newCategory = CategoryMapper.ToEntity(dto);

                await _uow.BeginTransactionAsync();

                await _uow.CategoryRepo.CreateAsync(newCategory);

                // 4. Integridad Vertical (DVV)
                await UpdateDVVAsync(_tableNameCategory, _appSettings.EntitiesConnection);

                // 5. Auditoría
                var log = _bitacoraFact.Create
                (
                    entry: BitacoraCatalogEnum.CreateOnBD,
                    tableName: _tableNameCategory,
                    user: currentUser.Id.ToString(),
                    sessionId: _sessionProvider.Current.Id,
                    correlationId: Guid.NewGuid(),
                    extraInfo: $"Categoría creada: {newCategory.Name}"
                );
                await _uow.BitacoraRepo.CreateAsync(log);

                await _uow.CommitAsync();

                result.Value = CategoryMapper.ToDto(newCategory);
                return result;
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction) await _uow.RollbackAsync();
                var dbError = _errorsFactory.CreateFromException(ex);

                try
                {
                    await _errorsRepository.CreateAsync(dbError);
                }
                catch { }

                result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNameCategory)));
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