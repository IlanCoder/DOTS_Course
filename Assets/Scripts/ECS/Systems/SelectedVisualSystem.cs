using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;

namespace ECS.Systems {
    public partial struct SelectedVisualSystem : ISystem {
        
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (RefRO<Selected> selected in SystemAPI.Query<RefRO<Selected>>()) {
                if (SystemAPI.IsComponentEnabled<MaterialMeshInfo>(selected.ValueRO.SelectedVisual)) continue;
                SystemAPI.SetComponentEnabled<MaterialMeshInfo>(selected.ValueRO.SelectedVisual, true);
            }
            foreach (RefRO<Selected> selected in SystemAPI.Query<RefRO<Selected>>().WithDisabled<Selected>()) {
                if (!SystemAPI.IsComponentEnabled<MaterialMeshInfo>(selected.ValueRO.SelectedVisual)) continue;
                SystemAPI.SetComponentEnabled<MaterialMeshInfo>(selected.ValueRO.SelectedVisual, false);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}