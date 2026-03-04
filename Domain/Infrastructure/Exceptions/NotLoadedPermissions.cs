using Domain.Exceptions.Base;

namespace Domain.Infrastructure.Exceptions
{
    public class NotLoadedPermissions: BaseException
    {
        public string UserId { get; }
        public string Msg { get; }


        public NotLoadedPermissions(string userId, string message ) : base(message)
        {
            UserId = userId;
            Msg = message;
        }

        public override string ToString()
        {
            return $"{base.ToString()} + UserId: {UserId} + Message: {Msg}";
        }
    }
}
