using Presenter.Messaging;
using Shared.ArchitecturalMarkers;
using SharedAbstractions.ArchitecturalMarkers;
using System;

namespace Presenter.HostFormActions
{
    public class HostFormActionsPresenter : IHostFormActionPresenter, IPresenter
    {
        private readonly IHostFormActions _view;
        public IHostFormActions View => _view;

        private readonly IMessenger _messenger;

        public EventHandler OnMinimizingWindow { get; set; }        //Todos estos deberían ser Messages para que se suscriba MainFormNavigationPresenter y envíe el ViewID como Payload. No tiene sentido que sean métodos expuestos acoplando entre Presenter y Presenter, justamente para eso está el Messenger.
        public EventHandler OnRestoringFromMinimized { get; set; }  //Todos estos deberían ser Messages para que se suscriba MainFormNavigationPresenter y envíe el ViewID como Payload. No tiene sentido que sean métodos expuestos acoplando entre Presenter y Presenter, justamente para eso está el Messenger.
        public EventHandler OnExpandingWindow { get; set; }         //Todos estos deberían ser Messages para que se suscriba MainFormNavigationPresenter y envíe el ViewID como Payload. No tiene sentido que sean métodos expuestos acoplando entre Presenter y Presenter, justamente para eso está el Messenger.
        public EventHandler OnContractingWindow { get; set; }       //Todos estos deberían ser Messages para que se suscriba MainFormNavigationPresenter y envíe el ViewID como Payload. No tiene sentido que sean métodos expuestos acoplando entre Presenter y Presenter, justamente para eso está el Messenger.
        public EventHandler OnClosingHostForm { get; set; }         //Todos estos deberían ser Messages para que se suscriba MainFormNavigationPresenter y envíe el ViewID como Payload. No tiene sentido que sean métodos expuestos acoplando entre Presenter y Presenter, justamente para eso está el Messenger.
        public EventHandler OnGotFocus { get; set; }       //aun sin uso  //Todos estos deberían ser Messages para que se suscriba MainFormNavigationPresenter y envíe el ViewID como Payload. No tiene sentido que sean métodos expuestos acoplando entre Presenter y Presenter, justamente para eso está el Messenger.
        public EventHandler OnLostFocus { get; set; }                //Todos estos deberían ser Messages para que se suscriba MainFormNavigationPresenter y envíe el ViewID como Payload. No tiene sentido que sean métodos expuestos acoplando entre Presenter y Presenter, justamente para eso está el Messenger.


        public HostFormActionsPresenter(IHostFormActions view, IMessenger messenger)
        {
            //Servicios inyectados
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));

            //Acciones de la view
            _view.MinimizeRequested += MinimizeWindow;
            _view.RestoreFromMinimizedRequested += RestoreWindowFromMinimized;
            _view.ExpandRequested += ExpandWindow;
            _view.ContractRequested += ContractWindow;
            _view.CloseWindowRequested += CloseView;
        
            
                      
            _view.ViewGotFocusNotificacionMessage += (s, e) => _messenger.Send(new ViewContainerGotFocusNotificationMessage(e));
            _view.ViewLostFocusNotificacionMessage += (s, e) => _messenger.Send(new ViewContainerLostFocusNotificacionMessage(e));

            //Para llamados exteriores vía Messenger
            _messenger.Subscribe<ViewContainerCloseRequestMessage>(CloseHostForm);
        }

        private void CloseHostForm(ViewContainerCloseRequestMessage payload)
        {
            _view.CloseWholeForm(payload);
        }

        public void MinimizeWindow(object sender, EventArgs e)
        {
            _view.MinimizeWindow();
            OnMinimizingWindow?.Invoke(this, EventArgs.Empty); //ACÀ MISMO DEBERÌA ENVIAR UN MENSAJE DE CHANGEVISIBILYSTATEMESSAGE PARA QUE MAINFORM SEPA CUANDO MARCARLO EN LAS PESTAÑITAS CON LA NUEBA BARRITA SUBRAYADA
        }

        public void ExpandWindow(object sender, EventArgs e)
        {
            _view.ExpandWindow();
            OnExpandingWindow?.Invoke(this, EventArgs.Empty);
        }

        public void ContractWindow(object sender, EventArgs e)
        {
            _view.ContractWindow();
            OnContractingWindow?.Invoke(this, EventArgs.Empty);
        }

        public void CloseView(object sender, EventArgs e)
        {
            _view.CloseWindow();
        }

        public void RestoreWindowFromMinimized(object sender, EventArgs e)
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
