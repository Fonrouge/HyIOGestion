
using SharedAbstractions.Enums;
using System;
using System.Threading.Tasks;

namespace SharedAbstractions.ArchitecturalMarkers
{
    public interface IView
    {
        void ApplyTranslation();
        void ThemingNotifiedByConfigurationsModule();
        Guid ViewId { get; set; }
        Task SetFeedbackState(FeedbackState state);
        void ChangeActivationStateFeedbackBar(bool isFocused);
    }
}
