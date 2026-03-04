using System;

namespace Shared.Sessions
{
    public class Session : ISession
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Guid CurrentUserId { get; }
        public DateTime LoginTime { get; } = DateTime.UtcNow;
        public DateTime LastActivity { get; set; }

        public Session(Guid useDtoId)
        {
            CurrentUserId = useDtoId;
        }

    }
}
