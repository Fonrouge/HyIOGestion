using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinformsUI.Infrastructure.Localization
{
    public interface ILocalizationService
    {
        string Translate(string originalValue, string targetLanguage);
    }
}
