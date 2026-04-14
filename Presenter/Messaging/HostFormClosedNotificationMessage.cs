using System;

namespace Presenter.Messaging
{
    public class HostFormClosedNotificationMessage : Message<Guid> 
    {
        public HostFormClosedNotificationMessage(Guid formId, object sender = null) : base(formId, sender) { }
    }
}