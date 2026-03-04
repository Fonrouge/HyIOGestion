using Shared.ArchitecturalMarkers;

namespace Shared.Factories
{
    public interface IAppEnvironmentFactory
    {        
        IAppEnvironment CreateCustom
        (
            object DashBoard,
            object SlotForTabs,
            object Palette,
            int FormType,
            object Icon = null
        );

        IAppEnvironment GetDefault();

        void SetMainContainers(object dashBoard, object rightToolBar);
    }
}