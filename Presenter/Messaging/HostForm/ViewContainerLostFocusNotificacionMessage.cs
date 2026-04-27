using System;

namespace Presenter.Messaging
{
    public class ViewContainerLostFocusNotificacionMessage : Message<Guid>
    {
        public ViewContainerLostFocusNotificacionMessage(Guid formId, object sender = null) : base(formId, sender) { }

    }
}
