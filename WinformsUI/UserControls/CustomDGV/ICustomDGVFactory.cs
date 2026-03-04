using Shared;
using Shared.Services.Searching;
using WinformsUI.Infrastructure.Translations;

namespace WinformsUI.UserControls.CustomDGV
{
    public interface ICustomDGVFactory
    {
        CustomDGVForm Create(ITranslatableControlsManager transMgr);
    }
}
