using System;
using System.Collections.Generic;

namespace Shared.Sessions
{
    public class Session : ISession
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Guid CurrentUserId { get; private set; }
        public List<string> PermissionsCodes { get; private set; }
        public DateTime LoginTime { get; } = DateTime.UtcNow;
        public DateTime LastActivity { get; set; }

        public Session(Guid useDtoId, List<string> permissionsCodes)
        {
            CurrentUserId = useDtoId;
            PermissionsCodes = permissionsCodes;
        }

    }
}
