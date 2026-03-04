
using System;

namespace Shared.Sessions
{
    public interface ISessionFactory
    {
        ISession Create(Guid userDtoId);
    }
}
