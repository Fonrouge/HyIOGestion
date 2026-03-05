using BLL.DTOs;
using BLL.DTOs.Errors;
using BLL.DTOs.Mappers;
using BLL.Infrastructure.Errors;
using Domain.Infrastructure;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.LogicLayers.Employees //=======================================================================REFACTORIZADO AL 27/02=======================================================================
{
    public class UCGetAllEmployees : IUCGetAllEmployees
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationSettings _appSettings;
        private readonly IErrorsFactory _errorsFactory;

        public UCGetAllEmployees
        (
            IUnitOfWork uow,
            IApplicationSettings appSettings,
            IErrorsFactory errorsFactory
        )
        {
            _uow = uow;
            _appSettings = appSettings;
            _errorsFactory = errorsFactory;
        }

        public async Task<(IEnumerable<EmployeeDTO>, OperationResult<EmployeeDTO>)> ExecuteAsync()
        {
            var result = new OperationResult<EmployeeDTO>();
            var listDto = new List<EmployeeDTO>();

            try
            {
                _uow.SetConnectionString(_appSettings.EntitiesConnection);
                await _uow.BeginTransactionAsync();

                var employees = await _uow.EmployeeRepo.GetAllAsync();

                listDto = employees.Select(e => EmployeeMapper.ToDto(e)).ToList();

                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                if (_uow.HasActiveTransaction)
                {
                    await _uow.RollbackAsync();
                }

                var errorDTO = ErrorMapper.ToDTO(_errorsFactory.Create(Domain.Exceptions.ErrorCatalogEnum.DatabaseUnavailable, _appSettings.EmployeeTableName));

                result.Errors.Add(errorDTO);
            }

            return (listDto, result);
        }
    }
}