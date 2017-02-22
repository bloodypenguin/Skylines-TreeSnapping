using TreeSnapping.OptionsFramework.Attibutes;

namespace TreeSnapping
{
    [Options("TreeSnapping")]
    public class Options
    {
        public Options()
        {
            dontUpdateYOnTerrainModification = true;
        }


        [Checkbox("Don't update tree Y coordinate on terrain modification")]
        public bool dontUpdateYOnTerrainModification { set; get; }

    }
}