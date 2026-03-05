using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.Infrastructure.Errors;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.Repositories;
using Shared;
using Shared.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Products
{
    public class UCGetAllProducts : IUCGetAllProducts
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IErrorsRepository _errorsRepository;
        private readonly IErrorsFactory _errorsFactory;
        private readonly ISessionProvider _sessionProvider;

        public UCGetAllProducts
        (
            IUnitOfWork uow,
            IApplicationSettings appSettings,
            IErrorsRepository errorsRepository,
            IErrorsFactory errorsFactory,
            ISessionProvider sessionProvider
        )
        {
            _uow = uow;
            _appSettings = appSettings;
            _errorsRepository = errorsRepository;
            _errorsFactory = errorsFactory;
            _sessionProvider = sessionProvider;
        }

        public async Task<(IEnumerable<ProductDTO>, OperationResult<ProductDTO>)> ExecuteAsync()
        {
            var result = new OperationResult<ProductDTO>();
            var listDto = new List<ProductDTO>();

            if (_sessionProvider.Current == null)
            {
                var error = _errorsFactory.Create(ErrorCatalogEnum.SessionExpired);

                try
                {
                    await _errorsRepository.CreateAsync(error);
                }
                catch { }

                result.Errors.Add(ErrorMapper.ToDTO(_errorsFactory.Create(ErrorCatalogEnum.SessionExpired)));

                return (listDto, result);

            }



            _uow.SetConnectionString(_appSettings.EntitiesConnection);
            await _uow.BeginTransactionAsync();

            try
            {
                // 2. AWAIT CRÍTICO: El Repositorio devuelve los Products con la propiedad "Categories" poblada.
                var products = await _uow.ProductRepo.GetAllAsync();

                // 3. Como es un ProductMapper "inteligente", 
                // esto mapea automáticamente las categorías internas también.
                listDto = ProductMapper.ToListDTO(products).ToList();

                // 4. Confirmamos (y liberamos la conexión)
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                // 1. Rollback de seguridad
                
                if (_uow.HasActiveTransaction)
                {
                    await _uow.RollbackAsync();
                }

             
                //Se crea el error como Log y se filtra la info necesaria en DTO (Stacktrace)
                var error = _errorsFactory.Create(ErrorCatalogEnum.DatabaseUnavailable);
                var exceptionError = _errorsFactory.CreateFromException(ex);

                await _errorsRepository.CreateAsync(error);
                await _errorsRepository.CreateAsync(exceptionError);

                var errorDto = ErrorMapper.ToDTO(error);
                var exceptionErrorDto = ErrorMapper.ToDTO(exceptionError);
                
                result.Errors.Add(errorDto);
                result.Errors.Add(exceptionErrorDto);
            }

            return (listDto, result);
        }
    }
}