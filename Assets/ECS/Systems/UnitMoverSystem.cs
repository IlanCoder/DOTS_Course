using ECS.Aspects;
using ECS.Authoring;
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
                
                if (unit.ArrivedToTarget()) {
                    unit.StopUnit();
                    SystemAPI.SetComponentEnabled<TargetPosition>(entity, false);
                    continue;
                }
                
                unit.MoveUnit(SystemAPI.Time.DeltaTime);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}