using ECS.Authoring;
using ECS.Jobs;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using SelectedEnablerJob = ECS.Jobs.Selection.SelectedEnablerJob;

namespace ECS.Systems.Selection {
    [UpdateInGroup(typeof(SelectionSystemGroup))]
    public partial struct SelectedEnablerSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Selected>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            state.Dependency = new SelectedEnablerJob().ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}