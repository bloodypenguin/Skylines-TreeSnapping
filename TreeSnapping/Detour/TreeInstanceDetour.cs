using ColossalFramework;
using TreeSnapping.OptionsFramework;
using TreeSnapping.Redirection;
using UnityEngine;

namespace TreeSnapping.Detour
{
    [TargetType(typeof(TreeInstance))]
    public struct TreeInstanceDetour
    {
        [RedirectMethod]
        public void CalculateTree(uint treeID)
        {
//begin mod
//end mod
        }

        [RedirectMethod]
        public static void AfterTerrainUpdated(ref TreeInstance prop, ushort treeID, float minX, float minZ, float maxX, float maxZ)
        {
            if (((int)prop.m_flags & 3) != 1 || ((int)prop.m_flags & 32) != 0)
                return;
            Vector3 position = prop.Position;
            position.y = Singleton<TerrainManager>.instance.SampleDetailHeight(position);
            ushort num = (ushort)Mathf.Clamp(Mathf.RoundToInt(position.y * 64f), 0, (int)ushort.MaxValue);
            if ((int)num == (int)prop.m_posY)
                return;
            int growState1 = prop.GrowState;
            //begin mod
            if (ToolsModifierControl.GetCurrentTool<TerrainTool>() == null || OptionsWrapper<Options>.Options.dontUpdateYOnTerrainModification)
            {
                prop.m_posY = prop.m_posY > num ? prop.m_posY : num;
            }
            else
            {
                prop.m_posY = num;
            }
            //end mod
            CheckOverlap(ref prop, treeID);
            int growState2 = prop.GrowState;
            if (growState2 != growState1)
            {
                Singleton<TreeManager>.instance.UpdateTree(treeID);
            }
            else
            {
                if (growState2 == 0)
                    return;
                Singleton<TreeManager>.instance.UpdateTreeRenderer(treeID, true);
            }
        }

        [RedirectReverse]
        private static void CheckOverlap(ref TreeInstance prop, ushort propID)
        {
            UnityEngine.Debug.LogError("Failed to redirect CheckOverlap()");
        }
    }
}