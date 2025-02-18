using ECS.Authoring;
using ECS.Jobs;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Rendering;
using EnableSelectVisualJob = ECS.Jobs.Selection.EnableSelectVisualJob;

namespace ECS.Systems.Selection {
    [UpdateInGroup(typeof(SelectionSystemGroup))]
    public partial struct SelectedVisualSystem : ISystem {
        ComponentLookup<MaterialMeshInfo> meshInfoLookup;
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Selected>();
            meshInfoLookup = state.GetComponentLookup<MaterialMeshInfo>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            meshInfoLookup.Update(ref state);
            state.Dependency = new EnableSelectVisualJob {
                MeshInfoLookup = meshInfoLookup
            }.Schedule(state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}