using System;

namespace Shared.Sessions
{
    public interface ISession
    {
        Guid Id { get; }
        Guid CurrentUserId { get; }
        DateTime LoginTime { get; }
        DateTime LastActivity { get; set; }      
    }
}
