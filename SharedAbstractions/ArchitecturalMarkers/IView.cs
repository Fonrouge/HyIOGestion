
using System;

namespace SharedAbstractions.ArchitecturalMarkers
{
    public interface IView
    {
        void ApplyTranslation();
        void ThemingNotifiedByConfigurationsModule();
        Guid ViewId { get; set; }
    }
}
