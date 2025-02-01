using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;

namespace ECS.Systems.Selection {
    [UpdateAfter(typeof(UnitSelectionSystem))]
    public partial struct SelectedVisualSystem : ISystem {
        
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Selected>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (RefRO<Selected> selected in SystemAPI.Query<RefRO<Selected>>().WithPresent<Selected>()) {
                if (selected.ValueRO.OnSelected)
                    SystemAPI.SetComponentEnabled<MaterialMeshInfo>(selected.ValueRO.SelectedVisual, true);
                else if (selected.ValueRO.OnDeselected)
                    SystemAPI.SetComponentEnabled<MaterialMeshInfo>(selected.ValueRO.SelectedVisual, false);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}