using System;

namespace Presenter.Messaging
{
    public class ClientsRelistRequestMessage : Message
    {
        public ClientsRelistRequestMessage(object sender = null) : base(sender) { }
    }
}
