using BLL.DTOs;
using BLL.Infrastructure.AuditLogs;
using BLL.Infrastructure.Errors;
using Domain.Infrastructure;
using Shared;
using System;
using System.Collections.Generic;
using Domain.Exceptions.Base;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;
using Shared.Sessions;
using BLL.DTOs.Errors;

namespace BLL.LogicLayers.Products.Categories.UseCases
{
    public class UCGetAllCategories : IUCGetAllCategories
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IErrorsFactory _errorsFactory;
        private readonly ISessionProvider _sessionProvider;


        public UCGetAllCategories
        (
            IUnitOfWork uow,
            IApplicationSettings appSettings,
            IErrorsFactory errorsFactory,
            ISessionProvider sessionProvider
        )
        {
            _uow = uow;
            _appSettings = appSettings;
            _errorsFactory = errorsFactory;
            _sessionProvider = sessionProvider;
        }


        public async Task<(IEnumerable<CategoryDTO> categories, OperationResult<CategoryDTO> operationResult)> ExecuteAsync()
        {
            var result = new OperationResult<CategoryDTO>();

            if (_sessionProvider.Current == null)
            {
                result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.SessionExpired)));
                return (new List<CategoryDTO>(), result);
            }

            try
            {
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                var categories = await _uow.CategoryRepo.GetAllAsync();
                var categoriesDto = CategoryMapper.ToListDTO(categories);

                return (categoriesDto, result);
            }

            catch (Exception ex)
            {
                result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.DataLoadError)));
                return (new List<CategoryDTO>(), result);
            }

        }

    }
}
