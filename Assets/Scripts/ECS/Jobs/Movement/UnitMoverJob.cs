using ECS.Aspects;
using Unity.Burst;
using Unity.Entities;

namespace ECS.Jobs.Movement {
    [BurstCompile]
    public partial struct UnitMoverJob : IJobEntity {
        public float DeltaTime;

        public void Execute(MovableUnitAspect unit) {
            unit.MoveUnit(DeltaTime);
        }
    }
}