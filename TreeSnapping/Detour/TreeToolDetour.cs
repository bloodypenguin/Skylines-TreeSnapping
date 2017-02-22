using System.Reflection;
using ColossalFramework;
using ColossalFramework.Math;
using TreeSnapping.Redirection;
using UnityEngine;

namespace TreeSnapping.Detour
{
    [TargetType(typeof(TreeTool))]
    public class TreeToolDetour : TreeTool
    {
        [RedirectMethod]
        public override void SimulationStep()
        {
            if (this.m_prefab == null)
            {
                this.m_wasPrefab = (TreeInfo)null;
                this.m_treeInfo = (TreeInfo)null;
            }
            else
            {
                if (this.m_treeInfo == null || this.m_wasPrefab != this.m_prefab)
                {
                    this.m_wasPrefab = this.m_prefab;
                    Randomizer r = this.m_randomizer;
                    this.m_treeInfo = (Singleton<ToolManager>.instance.m_properties.m_mode & ItemClass.Availability.AssetEditor) == ItemClass.Availability.None ? this.m_prefab.GetVariation(ref r) : this.m_prefab;
                    this.m_randomizer = r;
                }
                ToolBase.RaycastInput input = new ToolBase.RaycastInput(this.m_mouseRay, this.m_mouseRayLength);
                //begin mod
                input.m_ignoreBuildingFlags = Building.Flags.None;
                input.m_ignoreNodeFlags = NetNode.Flags.None;
                input.m_ignoreSegmentFlags = NetSegment.Flags.None;
                input.m_buildingService = new RaycastService(ItemClass.Service.None, ItemClass.SubService.None, ItemClass.Layer.Default);
                input.m_netService = new RaycastService(ItemClass.Service.None, ItemClass.SubService.None, ItemClass.Layer.Default);
                input.m_netService2 = new RaycastService(ItemClass.Service.None, ItemClass.SubService.None, ItemClass.Layer.Default);
                //end mod

                ulong[] collidingSegments;
                ulong[] collidingBuildings;
                this.m_toolController.BeginColliding(out collidingSegments, out collidingBuildings);
                try
                {
                    ToolBase.RaycastOutput output;
                    if (this.m_mouseRayValid && ToolBase.RayCast(input, out output))
                    {
                        if (this.m_mode == TreeTool.Mode.Brush)
                        {
                            this.m_mousePosition = output.m_hitPos;
                            this.m_placementErrors = !Singleton<TreeManager>.instance.CheckLimits() ? ToolBase.ToolErrors.TooManyObjects : ToolBase.ToolErrors.Pending;
                            if (this.m_mouseLeftDown == this.m_mouseRightDown)
                                return;
                            this.ApplyBrush();
                        }
                        else
                        {
                            if (this.m_mode != TreeTool.Mode.Single)
                                return;
                            //begin mod
                            //end mod
                            Randomizer r = this.m_randomizer;
                            uint id = Singleton<TreeManager>.instance.m_trees.NextFreeItem(ref r);
                            
                            ToolBase.ToolErrors toolErrors = TreeTool.CheckPlacementErrors(this.m_treeInfo, output.m_hitPos, /*output.m_currentEditObject*/false, id, collidingSegments, collidingBuildings);
                            if ((Singleton<ToolManager>.instance.m_properties.m_mode & ItemClass.Availability.Game) != ItemClass.Availability.None)
                            {
                                int constructionCost = this.m_treeInfo.GetConstructionCost();
                                if (constructionCost != 0 && constructionCost != Singleton<EconomyManager>.instance.PeekResource(EconomyManager.Resource.Construction, constructionCost))
                                    toolErrors |= ToolBase.ToolErrors.NotEnoughMoney;
                            }
                            if (!Singleton<TreeManager>.instance.CheckLimits())
                                toolErrors |= ToolBase.ToolErrors.TooManyObjects;
                            this.m_mousePosition = output.m_hitPos;
                            this.m_placementErrors = toolErrors;
                            //begin mod
                            //end mod
                        }
                    }
                    else
                        this.m_placementErrors = ToolBase.ToolErrors.RaycastFailed;
                }
                finally
                {
                    this.m_toolController.EndColliding();
                }
            }
        }

        private ToolBase.ToolErrors m_placementErrors
        {
            set
            {
                typeof(TreeTool).GetField("m_placementErrors", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(this, value);
            }
            get
            {
                return (ToolBase.ToolErrors)typeof(TreeTool).GetField("m_placementErrors", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(this);
            }
        }

        //struct
        private Vector3 m_mousePosition
        {
            set
            {
                typeof(TreeTool).GetField("m_mousePosition", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(this, value);
            }
            get
            {
                return (Vector3)typeof(TreeTool).GetField("m_mousePosition", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(this);
            }
        }

        private TreeInfo m_treeInfo
        {

            get
            {
                return (TreeInfo)typeof(TreeTool).GetField("m_treeInfo", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(this);
            }
            set
            {
                typeof(TreeTool).GetField("m_treeInfo", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(this, value);
            }
        }

        //struct
        private Randomizer m_randomizer
        {

            get
            {
                return (Randomizer)typeof(TreeTool).GetField("m_randomizer", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(this);
            }
            set
            {
                typeof(TreeTool).GetField("m_randomizer", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(this, value);
            }
        }

        private TreeInfo m_wasPrefab
        {

            get
            {
                return (TreeInfo)typeof(TreeTool).GetField("m_wasPrefab", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(this);
            }
            set
            {
                typeof(TreeTool).GetField("m_wasPrefab", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(this, value);
            }
        }

        private bool m_mouseRayValid => (bool)typeof(TreeTool).GetField("m_mouseRayValid", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(this);

        private bool m_mouseLeftDown
        {
            get
            {
                return
                    (bool)typeof(TreeTool).GetField("m_mouseLeftDown", BindingFlags.NonPublic | BindingFlags.Instance)
                        .GetValue(this);
            }

            set
            {
                typeof(TreeTool).GetField("m_mouseLeftDown", BindingFlags.NonPublic | BindingFlags.Instance)
                        .SetValue(this, value);
            }
        }

        private bool m_mouseRightDown
        {
            get
            {
                return
                    (bool)typeof(TreeTool).GetField("m_mouseRightDown", BindingFlags.NonPublic | BindingFlags.Instance)
                        .GetValue(this);
            }
            set
            {
                typeof(TreeTool).GetField("m_mouseRightDown", BindingFlags.NonPublic | BindingFlags.Instance)
                        .SetValue(this, value);
            }
        }

        private float m_mouseRayLength => (float)typeof(TreeTool).GetField("m_mouseRayLength", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(this);

        private Ray m_mouseRay => (Ray)typeof(TreeTool).GetField("m_mouseRay", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(this);

        [RedirectReverse]
        private void ApplyBrush()
        {
            UnityEngine.Debug.LogError("Failed to redirect ApplyBrush()");
        }
    }
}