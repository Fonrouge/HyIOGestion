using System;

namespace Presenter.Messaging
{
    public class SuppliersRelistRequestMessage : Message<Guid>
    {
        public SuppliersRelistRequestMessage(Guid formId, object sender = null) : base(formId, sender) { }
    }
}
