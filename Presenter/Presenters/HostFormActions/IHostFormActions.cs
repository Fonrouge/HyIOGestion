using Presenter.Messaging;
using System;

namespace Shared.ArchitecturalMarkers
{
    public interface IHostFormActions
    {
        event EventHandler ContractRequested;
        event EventHandler ExpandRequested;
        event EventHandler CloseWindowRequested;
        event EventHandler RestoreFromMinimizedRequested;
        event EventHandler MinimizeRequested;
        event EventHandler<Guid> ViewGotFocusNotificacionMessage;
        event EventHandler<Guid> ViewLostFocusNotificacionMessage;

        bool IsMinimized { get; set; }
        bool IsMaximized { get; set; }

        void ContractWindow();
        void ExpandWindow();
        void CloseWindow();
        void RestoreWindowFromMinimized();
        void MinimizeWindow();
        void CloseWholeForm(ViewContainerCloseRequestMessage message);

        void SetTitle(string Title);
        string GetTitle();
        void Initialize(IAppEnvironment ae);

        void SetContent(object content); // Recibe el formulario de caso de uso (C)
        void SetViewId(Guid Id);
        Guid GetViewId();
        Guid _viewId { get; set; }

    }
}
