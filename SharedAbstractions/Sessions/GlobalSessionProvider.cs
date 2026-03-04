namespace Shared.Sessions
{
    public class GlobalSessionProvider : ISessionProvider
    {
        // Al ser un Singleton, esta variable estática o de instancia vivirá 
        // desde que arranca el programa hasta que se cierra.
        private ISession _currentSession;

        public ISession Current
        {
            get => _currentSession;
            set => _currentSession = value;
        }

        public bool IsAuthenticated => _currentSession != null;
    }
}