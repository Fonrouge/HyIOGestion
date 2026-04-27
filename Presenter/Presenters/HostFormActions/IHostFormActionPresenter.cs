using System;

namespace Presenter.HostFormActions
{
    public interface IHostFormActionPresenter
    {
        EventHandler OnClosingHostForm { get; set; }
        EventHandler OnMinimizingWindow{ get; set; }         
        EventHandler OnRestoringFromMinimized{ get; set; }
        EventHandler OnExpandingWindow{ get; set; }
        EventHandler OnContractingWindow { get; set; }
        EventHandler OnGotFocus { get; set; } 
        EventHandler OnLostFocus { get; set; }
    }
}





