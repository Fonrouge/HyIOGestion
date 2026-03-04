using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinformsUI.UserControls.Wizard
{
    public class WizardPanelNavigator : IWizardPanelNavigator
    {
        private Panel[] _panelsArray;
        private int _panelIndex;
        private bool _initialized = false;

        public int CurrentIndex => _panelIndex;
        public Panel CurrentPanel => _panelsArray[_panelIndex];

        public void Initialize(Panel[] panelsArray)
        {
            if (panelsArray == null || panelsArray.Length == 0)
                throw new ArgumentException("Panels array cannot be null or empty.");

            _panelsArray = panelsArray;
            _panelIndex = 0;
            _initialized = true;
        }

        /// <summary>
        /// Salta directamente a un panel específico por su índice.
        /// </summary>
        /// <param name="targetIndex">El ID (índice) del panel al que se desea ir.</param>
        public void GoTo(int targetIndex)
        {
            if (!_initialized)
                throw new InvalidOperationException("Navigator not initialized. Call Initialize() first.");

            if (targetIndex < 0 || targetIndex >= _panelsArray.Length)
                return; // O lanzar una excepción según prefieras

            if (targetIndex == _panelIndex) return;

            // Obtenemos el control padre para suspender el layout globalmente
            Control parent = _panelsArray[_panelIndex].Parent;

            try
            {
                parent?.SuspendLayout();

                // Lógica de Swap: Intercambiamos posiciones entre el actual y el destino
                Point targetLocation = _panelsArray[targetIndex].Location;
                Point currentLocation = _panelsArray[_panelIndex].Location;

                _panelsArray[targetIndex].Location = currentLocation;
                _panelsArray[_panelIndex].Location = targetLocation;

                // Actualizamos el índice actual
                _panelIndex = targetIndex;
            }
            finally
            {
                // ResumeLayout(true) fuerza a aplicar los cambios de inmediato de forma limpia
                parent?.ResumeLayout(true);
            }
        }

        // Ahora Advance y Back son simples atajos de GoTo
        public void Advance(int steps = 1) => GoTo(_panelIndex + steps);
        public void Back(int steps = 1) => GoTo(_panelIndex - steps);

        public Size GetPanelSize()
        {
            if (!_initialized) throw new InvalidOperationException("Navigator not initialized.");
            return CurrentPanel.Size;
        }
    }
}