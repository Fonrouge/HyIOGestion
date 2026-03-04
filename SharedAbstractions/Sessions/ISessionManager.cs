using System;
using System.Collections.Generic;

namespace Shared.Sessions
{
    public interface ISessionManager
    {
        void AddSession(ISession session);

        void RemoveSession(Guid sessionId);

        IEnumerable<ISession> GetAllSessions();

        ISession GetSessionById(Guid sessionId);

        // void Broadcast(string message); // Lo dejamos comentado hasta que implementes websockets/SignalR
    }
}