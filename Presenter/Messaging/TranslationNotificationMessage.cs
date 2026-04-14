namespace Presenter.Messaging
{
    public class TranslationNotificationMessage : Message<string>
    {
        public TranslationNotificationMessage(string langCode, object sender = null) : base(langCode, sender) { }
    }
}
