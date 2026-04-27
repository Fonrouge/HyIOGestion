using SharedAbstractions.Enums;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.UserControls;

namespace WinformsUI.Forms
{
    public partial class InitialLoadingForm : Form
    {
        private readonly Func<Task> _work;
        public bool Success { get; private set; }
        private SilentFeedbackBar _feedbackBar;

        public InitialLoadingForm(Func<Task> work)
        {
            InitializeComponent();
            DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());
            _work = work;
            _feedbackBar = new SilentFeedbackBar();

            this.Load += InitialLoadingForm_Load;

            panel1.Controls.Add(_feedbackBar);
            

            
            _feedbackBar.Dock = DockStyle.Fill;
        }

        private async void InitialLoadingForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Iniciamos la animación (esto hace que la barrita empiece a fluir)
                await _feedbackBar.TriggerFeedbackAsync(FeedbackState.Loading);

                // 2. Definimos el tiempo mínimo que queremos que dure el splash (ej: 2.5 segundos)
                var minimumDisplayTask = Task.Delay(4000);

                // 3. Ejecutamos la tarea REAL de integridad
                var integrityTask = _work();

                // 4. Esperamos a que AMBAS terminen
                // Esto garantiza que si la integridad tarda más, esperamos; 
                // pero si tarda menos, al menos cumplimos los 2.5 segundos.
                await Task.WhenAll(minimumDisplayTask, integrityTask);

                Success = true;
                this.Close();
            }
            catch (Exception ex)
            {
                Success = false;
                // Opcional: mostrar error antes de cerrar
                this.Close();
            }
        }
    }
}
