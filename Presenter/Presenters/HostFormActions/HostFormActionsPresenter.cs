using Presenter.Messaging;
using Shared.ArchitecturalMarkers;
using SharedAbstractions.ArchitecturalMarkers;
using System;

namespace Presenter.HostFormActions
{
    public class HostFormActionsPresenter : IHostFormActionPresenter, IPresenter
    {

        private readonly IHostFormActions _view;

        private readonly IMessenger _messenger;
        public IHostFormActions View => _view;
        
        public EventHandler OnMinimizingWindow { get; set; }
        public EventHandler OnRestoringFromMinimized { get; set; }
        public EventHandler OnExpandingWindow { get; set; }
        public EventHandler OnContractingWindow { get; set; }
        public EventHandler OnClosingHostForm { get; set; }
        

        public HostFormActionsPresenter(IHostFormActions view, IMessenger messenger)
        {
            //Servicios inyectados
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            

            //Acciones de la view
            _view.MinimizeRequested += (s, e) => MinimizeWindow();
            _view.RestoreFromMinimizedRequested += (s, e) => RestoreWindowFromMinimized();
            _view.ExpandRequested += (s, e) => ExpandWindow();
            _view.ContractRequested += (s, e) => ContractWindow();                       
            _view.CloseWindowRequested += (s, e) => _view.CloseWindow();

            //Para llamados exteriores vía Messenger
            _messenger.Subscribe<HostFormCloseRequestMessage>(CloseHostForm);
        }

        private void CloseHostForm(HostFormCloseRequestMessage payload)
        {
            _view.CloseWholeForm(payload);
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
