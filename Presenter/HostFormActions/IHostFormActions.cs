using System;

namespace Shared.ArchitecturalMarkers
{
    public interface IHostFormActions
    {
        event EventHandler ContractRequested;
        event EventHandler ExpandRequested;
        event EventHandler CloseWindowRequested;
        event EventHandler RestoreWindowFromMinimizedRequested;
        event EventHandler MinimizeWindowRequested;              

        bool IsExpanded { get; set; }
        bool IsMinimized { get; set; }
        bool IsMaximized { get; set; }


        void ContractWindow();
        void ExpandWindow();
        void CloseWindow();
        void RestoreWindowFromMinimized();
        void MinimizeWindow();


        void SetTitle(string Title);
        void Initialize(IAppEnvironment ae);

        void SetContent(object content); // Recibe el formulario de caso de uso (C)
                        
    }
}
