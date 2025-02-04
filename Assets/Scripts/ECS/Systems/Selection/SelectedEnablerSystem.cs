using ECS.Authoring;
using ECS.Jobs;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

namespace ECS.Systems.Selection {
    [UpdateAfter(typeof(UnitSelectionSystem))]
    public partial struct SelectedEnablerSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Selected>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            JobHandle jobHandle = new SelectedEnablerJob().ScheduleParallel(state.Dependency);
            state.Dependency = jobHandle;
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}