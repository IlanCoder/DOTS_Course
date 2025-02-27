using ECS.Aspects;
using ECS.Authoring;
using ECS.Jobs;
using ECS.Jobs.Movement;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using UnitMoverJob = ECS.Jobs.Movement.UnitMoverJob;

namespace ECS.Systems.Movement {
    [UpdateAfter(typeof(SetUnitsMovementTargetSystem))]
    [UpdateInGroup(typeof(UnitsMovementSystemGroup))]
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
                SystemAPI.GetComponentRW<PhysicsMass>(entity).ValueRW.InverseMass = 0;
                SystemAPI.SetComponentEnabled<TargetPosition>(entity, false);
            }
            state.Dependency = new UnitMoverJob {
                DeltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}