using ECS.Authoring;
using ECS.Jobs;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using ResetSelectedJob = ECS.Jobs.Selection.ResetSelectedJob;

namespace ECS.Systems.Selection {
    [UpdateInGroup(typeof(SelectionSystemGroup), OrderLast = true)]
    public partial struct ResetSelectedEvents : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Selected>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            state.Dependency = new ResetSelectedJob().ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}