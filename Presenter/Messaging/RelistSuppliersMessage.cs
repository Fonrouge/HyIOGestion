using System;

namespace Presenter.Messaging
{
    public class RelistSuppliersMessage : Message<Guid>
    {
        public RelistSuppliersMessage(Guid formId, object sender = null) : base(formId, sender) { }
    }
}
