using System;

namespace Presenter.Messaging
{
    public class ViewContainerClosedNotificationMessage : Message<Guid> 
    {
        public ViewContainerClosedNotificationMessage(Guid formId, object sender = null) : base(formId, sender) { }
    }
}