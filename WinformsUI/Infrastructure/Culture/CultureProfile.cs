using System;
using System.Globalization;

namespace WinformsUI.Infrastructure.Culture
{

    public class CultureProfile : ICultureSwitcher
    {
        private CultureInfo _currentUICulture;
        private CultureInfo _persistanceCulture;

        public CultureProfile(string uiCultureCode)
        {
            _currentUICulture = new CultureInfo(uiCultureCode);
            _persistanceCulture = CultureInfo.InvariantCulture;
        }

        public void SetUICulture(string userInterfaceCultCode)
        {
            if (string.IsNullOrWhiteSpace(userInterfaceCultCode)) throw new ArgumentException("Código de cultura inválido.");

            var culture = new CultureInfo(userInterfaceCultCode);
            _currentUICulture = culture;

        }

        public CultureInfo GetCurrentUICulture()
        {
            return _currentUICulture ?? CultureInfo.InvariantCulture;
        }

        public CultureInfo GetPersistenceCulture()
        {
            return _persistanceCulture;
        }
    }
}