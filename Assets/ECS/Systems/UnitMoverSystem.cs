using ECS.Aspects;
using ECS.Authoring;
using ECS.Jobs;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Systems {
    public partial struct UnitMoverSystem : ISystem {
        
        
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach ((MovableUnitAspect unit, Entity entity) in SystemAPI.Query<MovableUnitAspect>().WithEntityAccess()) {
                if (!unit.ArrivedToTarget()) continue;
                unit.StopUnit();
                SystemAPI.SetComponentEnabled<TargetPosition>(entity, false);
            }
            UnitMoverJob unitMoverJob = new UnitMoverJob {
                DeltaTime = SystemAPI.Time.DeltaTime
            };
            unitMoverJob.ScheduleParallel();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}