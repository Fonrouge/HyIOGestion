namespace Shared.ArchitecturalMarkers
{
    public interface IAppEnvironment
    {
        bool IsInitialized { get; }
        
        void Initialize(object dashBoard, object rightToolBar, object palette, int formType, object icon = null);
    }
}