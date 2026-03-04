using Shared.Sessions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;

namespace BLL.SessionInfo
{
    public class SessionManager : ISessionManager, IDisposable
    {        
        private readonly ConcurrentDictionary<Guid, ISession> _sessions = new ConcurrentDictionary<Guid, ISession>();
        private readonly Timer _cleanupTimer;
        private readonly TimeSpan _sessionTimeout = TimeSpan.FromMinutes(30);

        public SessionManager()
        {
            _cleanupTimer = new Timer(60000); // 1 minuto
            _cleanupTimer.Elapsed += OnCleanupTimerElapsed;
            _cleanupTimer.AutoReset = true;
            _cleanupTimer.Enabled = true;
        }

        public void AddSession(ISession session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            session.LastActivity = DateTime.UtcNow;

            // NOTA: TryAdd es una operación atómica. Si dos hilos intentan agregar al mismo tiempo, no crashea.
            _sessions.TryAdd(session.Id, session);
        }

        public void RemoveSession(Guid sessionId)
        {
            _sessions.TryRemove(sessionId, out _);
        }

        public IEnumerable<ISession> GetAllSessions()
        {
            return _sessions.Values;
        }

        public ISession GetSessionById(Guid sessionId)
        {
            if (_sessions.TryGetValue(sessionId, out var session))
            {
                session.LastActivity = DateTime.UtcNow;
                return session;
            }

            return null; // NOTA: Se podría lanzar una excepción personalizada como SessionExpiredException
        }

        private void OnCleanupTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var now = DateTime.UtcNow;

            // NOTA:
            // En un ConcurrentDictionary, es totalmente legal usar foreach mientras otros hilos agregan/sacan items.
            // Si fuera una List<T>, esto lanzaría "Collection was modified".
            foreach (var kvp in _sessions)
            {
                if ((now - kvp.Value.LastActivity) > _sessionTimeout)
                {
                    // La sesión expiró. La eliminamos de forma atómica.
                    _sessions.TryRemove(kvp.Key, out _);
                }
            }
        }

        public void Dispose()
        {
            _cleanupTimer?.Stop();
            _cleanupTimer?.Dispose();
            _sessions.Clear();
        }
    }
}