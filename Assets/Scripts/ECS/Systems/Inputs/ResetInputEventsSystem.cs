using ECS.Aspects;
using Unity.Burst;
using Unity.Entities;

namespace ECS.Systems.Inputs {
    [UpdateInGroup(typeof(PresentationSystemGroup), OrderLast = true)]
    public partial struct ResetInputEventsSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<OnSelectAreaStart>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            Entity inputEntity = SystemAPI.GetSingletonEntity<OnSelectAreaStart>();
            InputEventsAspect inputEventsAspect = SystemAPI.GetAspect<InputEventsAspect>(inputEntity);
            inputEventsAspect.ResetInputCalls();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}