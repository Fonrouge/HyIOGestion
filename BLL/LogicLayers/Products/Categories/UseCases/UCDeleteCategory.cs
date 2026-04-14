using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
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

namespace BLL.LogicLayers.Products.Categories.UseCases
{
    public class UCDeleteCategory : IUCDeleteCategory
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IBitacoraFactory _bitacoraFact;
        private readonly ISessionProvider _sessionProvider;
        private readonly IErrorsFactory _errorsFactory;
        private readonly IErrorsRepository _errorsRepository;

        private readonly string _tableNameCategory;

        public UCDeleteCategory
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

            _tableNameCategory = appSettings.CategoryTableName ?? "Categories";
        }

        public async Task<OperationResult<CategoryDTO>> ExecuteAsync(CategoryDTO dto)
        {
            var result = new OperationResult<CategoryDTO>();

            try
            {
                // 1. Validar Sesión Activa
                if (_sessionProvider.Current == null)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.SessionExpired)));
                    return result;
                }

                // Seteamos conexión para validaciones
                _uow.SetConnectionString(_appSettings.EntitiesConnection);

                // 2. Validar Permisos
                var currentUser = await _uow.UserRepo.GetByIdAsync(_sessionProvider.Current.CurrentUserId);
                
                if (!currentUser.HasPermission("CATEGORY_DELETE"))
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.InsufficientPermissions, _tableNameCategory)));
                    return result;
                }

                // 3. Buscar y Validar Existencia
                var existingCategory = await _uow.CategoryRepo.GetByIdAsync(dto.Id);

                // Aplicamos tu patrón de seguridad defensiva
                if (existingCategory == null)
                {
                    result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.NotFound, _tableNameCategory)));
                    return result;
                }

                // 4. ABRIR TRANSACCIÓN
                await _uow.BeginTransactionAsync();

                // 5. Acción Principal: Eliminación Física
                // Nota: SQL tirará error si hay productos asociados (FK), el catch lo atrapará.
                await _uow.CategoryRepo.DeleteAsync(dto.Id);

                // 6. Integridad Vertical (DVV): Obligatorio al borrar físicamente una fila
                await UpdateDVVAsync(_tableNameCategory, _appSettings.EntitiesConnection);

                // 7. Auditoría (Bitácora)
                var log = _bitacoraFact.Create(
                    entry: BitacoraCatalogEnum.SoftDeleteOnBD,
                    user: currentUser.Id.ToString(),
                    tableName: _tableNameCategory,
                    sessionId: _sessionProvider.Current.Id,
                    correlationId: Guid.NewGuid(),
                    extraInfo: $"Se eliminó la categoría ID: {dto.Id} (Nombre: {existingCategory.Name})"
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

                // Log técnico interno
                var dbError = _errorsFactory.CreateFromException(ex);
                dbError.Table = _tableNameCategory;
                try { await _errorsRepository.CreateAsync(dbError); } catch { }

                // Manejo de restricciones de integridad (FK con Productos)
                ErrorLog uiError;

                if (ex.Message.Contains("REFERENCE constraint") || ex.Message.Contains("FK_"))
                    uiError = _errorsFactory.Create(ErrorCatalogEnum.DeleteRestriction, _tableNameCategory);
                else
                    uiError = _errorsFactory.Create(ErrorCatalogEnum.InternalError, _tableNameCategory);

                var errorDto = ErrorMapper.ToDTO(uiError);
                errorDto.LogId = dbError.Id;
                errorDto.InformativeMessage = $"Error al eliminar la categoría. Ref ID: {dbError.Id}";

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