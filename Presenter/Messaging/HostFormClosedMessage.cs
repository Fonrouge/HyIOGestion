using System;

namespace Presenter.Messaging
{
    public class HostFormClosedMessage : Message<Guid>  // Cambia <string> a <Guid>
    {
        public HostFormClosedMessage(Guid formId, object sender = null) : base(formId, sender) { }
    }
}