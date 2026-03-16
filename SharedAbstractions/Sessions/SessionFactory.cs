using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Shared.Sessions
{
    public class SessionFactory : ISessionFactory
    {

        private readonly IServiceProvider _sp;

        public SessionFactory(IServiceProvider sp)
        {
            _sp = sp;
        }

        public ISession Create(Guid userDtoId, List<string> permissionsCodes)
        {
            if (userDtoId == Guid.Empty) throw new NullReferenceException("User ID -Guid- cannot be empty/null");

            return ActivatorUtilities.CreateInstance<Session>(_sp, userDtoId, permissionsCodes);
        }
    }
}
