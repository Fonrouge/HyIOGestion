using Microsoft.Extensions.DependencyInjection;
using Shared.ArchitecturalMarkers;
using Shared.Factories;
using System;
using Winforms.Theme;

namespace WinformsUI.Infrastructure.Factories
{
    public class AppEnvironmentFactory : IAppEnvironmentFactory
    {
        private readonly IServiceProvider _sp;
        private object _mainDash;
        private object _mainToolbar;
        private bool _isInitialized = false;

        public AppEnvironmentFactory(IServiceProvider sp) => _sp = sp;

        public void SetMainContainers(object dashBoard, object rightToolBar)
        {
            _mainDash = dashBoard ?? throw new ArgumentNullException(nameof(dashBoard));
            _mainToolbar = rightToolBar ?? throw new ArgumentNullException(nameof(rightToolBar));
            _isInitialized = true;
        }

        public IAppEnvironment CreateCustom(object dashBoard, object rightToolBar, object palette, int formType, object icon = null)
        {
            var env = _sp.GetRequiredService<IAppEnvironment>();

            env.Initialize(dashBoard, rightToolBar, palette, formType, icon);

            return env;
        }

        public IAppEnvironment GetDefault()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("La Factory debe inicializarse con los contenedores del MainForm.");
    
            return CreateCustom(
                _mainDash,
                _mainToolbar,
                DarkTheme.GetCurrentPalette(),
                (int)FormTypeEnum.Module
            );
        }
    }
}
