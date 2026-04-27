using System;

namespace Presenter.Messaging
{
    public class ViewContainerCloseRequestMessage : Message<Guid>
    {
        public ViewContainerCloseRequestMessage(Guid formId, object sender = null) : base(formId, sender) { }
    }
}
