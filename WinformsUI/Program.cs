using BLL.UseCases;
using Bootstrapper;
using Microsoft.Extensions.DependencyInjection;
using Presenter.LoginScreen;
using Presenter.MainFormNavigation;
using System;
using System.Windows.Forms;
using WinformsUI.Forms.Login;
using WinformsUI.Forms.Main;
using WinformsUI.Infrastructure.DI;
using WinformsUI.Infrastructure.Factories;

namespace Winforms.Theme
{
    internal static class Program
    {
        private static ServiceCollection _services;
        private static ServiceProvider _finalProvider;
        private static MainForm _mainForm;

        [STAThread]
        static void Main()
        {
           // // Detectar si hay un debugger de Windows o un Profiler de memoria
           // if (System.Diagnostics.Debugger.IsAttached ||
           //     System.Runtime.InteropServices.Marshal.GetExceptionCode() != 0)
           // {
           //     // "Suicidio" silencioso del proceso
           //     System.Diagnostics.Process.GetCurrentProcess().Kill();
           // }

            Initialize();
            
            SetGlobalPalette();
            CreateServiceCollection();

            AddExternalServiceLayer(_services); //Bootstrapper goes first because UI is gonna need an Instance of IApplicationSettings         
            AddInternalLayer(_services); //Goes second.
            _finalProvider = _services.BuildServiceProvider();
          

            if (!RunIntegrityCheck()) return; //Desactivado porquie la base de datos está en fase de prueba y hay datos que modifican constantemente directamente por el motor de bbdd.
            if (!RunLoginFlow()) return;
      
            InstanceServices();
            Application.Run(_mainForm);
        }

        private static void Initialize()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        private static void SetGlobalPalette() => DarkTheme.SetGlobalPalette(DarkTheme.PalettesDark.Oceanic());

        private static void CreateServiceCollection() => _services = new ServiceCollection();

        private static void AddExternalServiceLayer(IServiceCollection services) => services.AddApplicationLayer();

        private static void AddInternalLayer(IServiceCollection services)
        {
            services.AddUILayer();
            services.AddFromPresenter();
        }
 

        private static void InstanceServices()
        {
           
            _mainForm = _finalProvider.GetRequiredService<MainForm>();

            ActivatorUtilities.CreateInstance<MainFormNavigationPresenter>(_finalProvider, _mainForm);
        }

        private static bool RunIntegrityCheck()
        {
            try
            {
                _finalProvider.GetRequiredService<IVerifyDVH>().ExecuteAsync();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error crítico de integridad: {ex.Message}\nEl sistema se cerrará.", //CAMBIAR POR MENSAJE DE ERROR PROVENIENTE DE ARCHIVO DE CONFIGURACION A FUTURO
                                "ALERTA DE SEGURIDAD", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
        }

        private static bool RunLoginFlow()
        {
            var UCLogin = _finalProvider.GetRequiredService<IUCLogin>();
            var fact = _finalProvider.GetRequiredService<IFormsFactory>();

            ILoginView loginform = fact.CreateGeneric<LoginFrm>();

            ActivatorUtilities.CreateInstance<LoginPresenter>(_finalProvider, loginform, UCLogin);

            using (loginform)
            {
                var result = loginform.ShowDialog();
                return result == true;
            }
        }
    }
}