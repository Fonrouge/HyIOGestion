using System.Collections.Generic;

namespace WinformsUI.Infrastructure.Localization
{
    public interface ILocalizationService
    {
        string Translate(string originalValue, string targetLanguage);
        List<LanguageInfo> GetAvailableLanguages();
    }
}
