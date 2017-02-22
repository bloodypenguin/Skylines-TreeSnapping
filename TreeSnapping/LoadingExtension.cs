using ICities;
using TreeSnapping.Detour;
using TreeSnapping.Redirection;

namespace TreeSnapping
{
    public class LoadingExtension : LoadingExtensionBase
    {
        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            if (loading.currentMode != AppMode.Game)
            {
                return;
            }
            Redirector<TreeToolDetour>.Deploy();
            Redirector<TreeInstanceDetour>.Deploy();
        }

        public override void OnReleased()
        {
            base.OnReleased();
            Redirector<TreeInstanceDetour>.Revert();
            Redirector<TreeToolDetour>.Revert();            
        }
    }
}