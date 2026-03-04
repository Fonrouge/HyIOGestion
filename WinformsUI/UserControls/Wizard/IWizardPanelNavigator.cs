using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinformsUI.UserControls.Wizard
{
    public interface IWizardPanelNavigator
    {
        // --- Propiedades ---

        /// <summary>
        /// Índice del panel que se está mostrando actualmente.
        /// </summary>
        int CurrentIndex { get; }

        /// <summary>
        /// Referencia al control Panel activo.
        /// </summary>
        Panel CurrentPanel { get; }

        // --- Métodos de Control ---

        /// <summary>
        /// Configura el set de paneles sobre los que operará el navegador.
        /// </summary>
        void Initialize(Panel[] panelsArray);

        /// <summary>
        /// Mueve la visualización a un panel específico por su índice.
        /// </summary>
        void GoTo(int targetIndex);

        /// <summary>
        /// Avanza N cantidad de paneles.
        /// </summary>
        void Advance(int steps = 1);

        /// <summary>
        /// Retrocede N cantidad de paneles.
        /// </summary>
        void Back(int steps = 1);

        /// <summary>
        /// Obtiene el tamaño del panel actual.
        /// </summary>
        Size GetPanelSize();
    }
}