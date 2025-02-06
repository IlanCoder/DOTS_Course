using ECS.Authoring;
using ECS.Jobs;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Rendering;

namespace ECS.Systems.Selection {
    [UpdateAfter(typeof(UnitSelectionSystem))]
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