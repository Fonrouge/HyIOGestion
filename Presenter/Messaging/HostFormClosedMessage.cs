namespace Presenter.Messaging
{
    public class HostFormClosedMessage : Message<string>
    {
        public HostFormClosedMessage(string formIdentifier, object sender = null)
            : base(formIdentifier, sender)
        {
            // Payload es el identifier (ej: _title de HostForm)
        }
    }
}
