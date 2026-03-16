
using System;
using System.Collections.Generic;

namespace Shared.Sessions
{
    public interface ISessionFactory
    {
        ISession Create(Guid userDtoId, List<string> permissionsCodes);
    }
}
