using BLL.UseCases;
using Bootstrapper;
using Microsoft.Extensions.DependencyInjection;
using Presenter.LoginScreen;
using Presenter.MainFormNavigation;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinformsUI.Forms;
using WinformsUI.Forms.ConfigurationsMenu;
using WinformsUI.Forms.Login;
using WinformsUI.Forms.Main;
using WinformsUI.Infrastructure.DI;
using WinformsUI.Infrastructure.Factories;

namespace Winforms.Theme
{
    internal static class Program
    {
        private static IServiceProvider _serviceProvider;

        [STAThread]
        static void Main()
        {
            // 1. Configuración básica
            InitializeWinForms();
            SetGlobalPalette();
            _serviceProvider = CreateServiceProvider();

            // 2. Ejecutar integridad con el LoadingForm
            var checker = _serviceProvider.GetRequiredService<IVerifyDVH>();

            // Pasamos la ejecución como un delegado
            using (var loading = new InitialLoadingForm(() => checker.ExecuteAsync()))
            {
                Application.Run(loading); // El programa se detiene aquí hasta que loading se cierre

                if (!loading.Success) return; // Si falló la integridad, cerramos la app
            }

            // 3. Flujo de Login
            if (!RunLoginFlow()) return;

            // 4. Flujo Principal
            RunMainFlow();

        }

        private static void InitializeWinForms()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        private static void SetGlobalPalette() => DarkTheme.SetGlobalPalette(DarkTheme.PalettesDark.Oceanic());

        private static IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();

            // Capas externas e internas
            services.AddApplicationLayer();
            services.AddUILayer();
            services.AddFromPresenter();

            return services.BuildServiceProvider();
        }

        private static bool RunIntegrityCheckAsync()
        {
            // IMPORTANTE: En fase de prueba esto podría retornar true directamente
            // pero mantenemos la lógica asíncrona corregida.
            var checker =  _serviceProvider.GetRequiredService<IVerifyDVH>();
            try
            {
                Task.WaitAll(checker.ExecuteAsync());
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error crítico de integridad: {ex.Message}\nEl sistema se cerrará.",
                    "ALERTA DE SEGURIDAD", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            
        }

        private static bool RunLoginFlow()
        {
            var factory = _serviceProvider.GetRequiredService<IFormsFactory>();
            ActivatorUtilities.CreateInstance<ConfigurationsForm>(_serviceProvider);


            // Usamos 'using' para asegurar que el Form se libere
            using (var loginForm = factory.CreateGeneric<LoginFrm>())
            {
                // Instanciamos el Presenter y usamos 'using' para su Dispose (eventos)
                using (ActivatorUtilities.CreateInstance<LoginPresenter>(_serviceProvider, loginForm))
                {
                    // En WinForms ShowDialog devuelve DialogResult
                    var result = loginForm.ShowDialog();
                    return result == DialogResult.OK || result == DialogResult.Yes;
                }
            }
        }

        private static void RunMainFlow()
        {
            var mainForm = _serviceProvider.GetRequiredService<MainForm>();

            // Vinculamos el Presenter de navegación principal
            using (ActivatorUtilities.CreateInstance<MainFormNavigationPresenter>(_serviceProvider, mainForm))
            {
                Application.Run(mainForm);
            }
        }



    }
}


//  List<string> dvhs = new List<string>()
//  {
//      "8c317ae4d9443c542540fc4e7215c4abd96bf2d9ea496e553fb51ce6d2963dc9",
//      "90706ce7b37ebdb19ad7f997371dd46053deb151b82179898abaad06b5500ca8",
//      "edddbde47942bc7497b7bfb62f889e958e6cae4c9a5aaeb4bf0250a71b5bc630",
//      "596ec69321b4bc9afc8b7d4cf80d5c6bb695580afca86da39cd0ba0e81df78e7",
//      "39e0a5d60cd67e1e2106271483a75b69d14f6b9ffb23b1b0f0e000f59f490bf2",
//      "ee8475f8b486fdf1d2c3dd3b545e9ade1212290d9f8149e402500e40101d2ae7",
//
//  };
//
//
//
//  Console.Write(IntegrityService.CalculateDVV(dvhs));
//
//  Debugger.Break();
//
//
//
//
//
//
//
//
//  Debugger.Break();
