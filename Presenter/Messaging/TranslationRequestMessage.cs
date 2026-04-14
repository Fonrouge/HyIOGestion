namespace Presenter.Messaging
{
    public class TranslationRequestMessage: Message<string>
    {
        public TranslationRequestMessage(string langCode, object sender = null) : base(langCode, sender) { }
    }
}
