using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;

namespace ECS.Jobs.Selection {
    [WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
    [BurstCompile]
    public partial struct EnableSelectVisualJob : IJobEntity {
        public ComponentLookup<MaterialMeshInfo> MeshInfoLookup;
        public void Execute(in Selected selected) {
            if (selected.OnSelected) MeshInfoLookup.SetComponentEnabled(selected.SelectedVisual, true);
            else if(selected.OnDeselected)MeshInfoLookup.SetComponentEnabled(selected.SelectedVisual, false);
        }
    }
}