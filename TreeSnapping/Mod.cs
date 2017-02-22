using ICities;
using TreeSnapping.OptionsFramework.Extensions;

namespace TreeSnapping
{
    public class Mod : IUserMod
    {
        public string Name => "Tree Snapping";
        public string Description => "Allows to snap trees to buildings, roads - just like in asset editor!";

        public void OnSettingsUI(UIHelperBase helper)
        {
            helper.AddOptionsGroup<Options>();
        }
    }
}