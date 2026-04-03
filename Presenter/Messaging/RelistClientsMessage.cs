using System;

namespace Presenter.Messaging
{
    public class RelistClientsMessage : Message<Guid>
    {
        public RelistClientsMessage(Guid formId, object sender = null) : base(formId, sender) { }
    }
}
