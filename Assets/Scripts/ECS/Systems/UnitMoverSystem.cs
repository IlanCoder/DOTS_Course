using ECS.Aspects;
using ECS.Authoring;
using ECS.Jobs;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

namespace ECS.Systems {
    public partial struct UnitMoverSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<MoveSpeed>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach ((MovableUnitAspect unit, Entity entity) in SystemAPI.Query<MovableUnitAspect>().WithEntityAccess()) {
                if (!unit.ArrivedToTarget()) continue;
                unit.StopUnit();
                SystemAPI.SetComponentEnabled<TargetPosition>(entity, false);
            }
            JobHandle unitMoverJob = new UnitMoverJob {
                DeltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel(state.Dependency);
            state.Dependency = unitMoverJob;
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}