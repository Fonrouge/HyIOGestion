using BLL.UseCases;
using Bootstrapper;
using Microsoft.Extensions.DependencyInjection;
using Presenter.LoginScreen;
using Presenter.MainFormNavigation;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            // 1. Configuración básica de WinForms
            InitializeWinForms();

            // 2. Estética y DI
            SetGlobalPalette();
            _serviceProvider = CreateServiceProvider();

            // 3. Flujo de Integridad (Ahora sí es esperado/awaited)
            //           if (!await RunIntegrityCheckAsync()) return;

            // 4. Flujo de Login
            if (!RunLoginFlow()) return;

            // 5. Flujo Principal (Main Form)
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

        private static async Task<bool> RunIntegrityCheckAsync()
        {
            // IMPORTANTE: En fase de prueba esto podría retornar true directamente
            // pero mantenemos la lógica asíncrona corregida.
            try
            {
                var checker = _serviceProvider.GetRequiredService<IVerifyDVH>();
                await checker.ExecuteAsync();
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