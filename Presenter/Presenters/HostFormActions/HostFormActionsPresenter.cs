using Shared.ArchitecturalMarkers;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter.HostFormActions
{
    public class HostFormActionsPresenter : IHostFormActionPresenter, IPresenter
    { //entonces, ihostformactionpresenter como nueva interfaz de la que hereda hoistformactionpresenter para que pueda escuchar imainformnavigationpresenter y enterarme cuando se cierra un hostform para sacarlo de la lista de presenters añadidos cuando se abre unopa nuievo

        private readonly IHostFormActions _view;
        public EventHandler OnMinimizingWindow;
        public EventHandler OnRestoringFromMinimized;
        public EventHandler OnExpandingWindow;
        public EventHandler OnContractingWindow;
        public Guid FormId { get; }  // Nuevo: Reemplaza FormTitle, usa GUID
        public EventHandler OnClosingHostForm { get; set; }

        public HostFormActionsPresenter(IHostFormActions view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            FormId = _view.Id;  // Nuevo: Set desde view

            _view.MinimizeWindowRequested += (s, e) => MinimizeWindow();
            _view.RestoreWindowFromMinimizedRequested += (s, e) => RestoreWindowFromMinimized();

            _view.ExpandRequested += (s, e) => ExpandWindow();
            _view.ContractRequested += (s, e) => ContractWindow();
            _view.CloseWindowRequested += (s, e) => _view.CloseWindow();
        }

        public void MinimizeWindow()
        {
            _view.MinimizeWindow();
            OnMinimizingWindow?.Invoke(this, EventArgs.Empty);
        }

        public void ExpandWindow()
        {
            _view.ExpandWindow();
            OnExpandingWindow?.Invoke(this, EventArgs.Empty);
        }

        public void ContractWindow()
        {
            _view.ContractWindow();
            OnContractingWindow?.Invoke(this, EventArgs.Empty);
        }

        public void RestoreWindowFromMinimized()
        {
            _view.RestoreWindowFromMinimized();
            OnRestoringFromMinimized?.Invoke(this, EventArgs.Empty);
        }

        public bool IsMinimized => _view.IsMinimized;



        public void SetMinimizeStatus(bool isMin)
        {
            _view.IsMinimized = isMin;
        }
        public void SetMaximizeStatus(bool isMax)
        {
            _view.IsMaximized = isMax;
        }
    }
}
