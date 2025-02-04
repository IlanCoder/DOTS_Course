using ECS.Authoring;
using ECS.Jobs;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

namespace ECS.Systems.Selection {
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct ResetSelectedEvents : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Selected>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            JobHandle handle = new ResetSelectedJob().ScheduleParallel(state.Dependency);
            state.Dependency = handle;
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}