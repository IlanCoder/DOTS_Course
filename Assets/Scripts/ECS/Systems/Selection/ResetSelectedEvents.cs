using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;

namespace ECS.Systems.Selection {
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct ResetSelectedEvents : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Selected>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (RefRW<Selected> selected in SystemAPI.Query<RefRW<Selected>>().WithPresent<Selected>()) {
                selected.ValueRW.OnSelected = false;
                selected.ValueRW.OnDeselected = false;
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}