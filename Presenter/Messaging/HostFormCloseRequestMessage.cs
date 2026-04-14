using System;

namespace Presenter.Messaging
{
    public class HostFormCloseRequestMessage : Message<Guid>
    {
        public HostFormCloseRequestMessage(Guid formId, object sender = null) : base(formId, sender) { }
    }
}
