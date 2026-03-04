namespace Shared.Sessions
{
    public interface ISessionProvider
    {
        ISession Current { get; set; }
        bool IsAuthenticated { get; }
    }
}
