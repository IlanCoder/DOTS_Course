using ECS.Aspects;
using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace ECS.Jobs.Movement {
    [BurstCompile]
    public partial struct UnitMoverJob : IJobEntity {
        public float DeltaTime;

        public void Execute(MovableUnitAspect unit) {
            unit.MoveUnit(DeltaTime);
        }
    }
}