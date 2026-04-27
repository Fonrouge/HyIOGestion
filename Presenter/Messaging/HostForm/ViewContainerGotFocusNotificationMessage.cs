using System;

namespace Presenter.Messaging
{
    public class ViewContainerGotFocusNotificationMessage : Message<Guid>
    {
        public ViewContainerGotFocusNotificationMessage(Guid formId, object sender = null) : base(formId, sender) { }

    }
}
