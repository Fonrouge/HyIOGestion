using Domain.Exceptions;
using Domain.Exceptions.Base;
using System;

namespace BLL.Infrastructure.Errors
{
    public interface IErrorsFactory
    {
        ErrorLog Create(ErrorCatalogEnum error, string table = null);
        ErrorLog CreateFromException(Exception ex);
    }
}