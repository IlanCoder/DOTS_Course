using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

namespace ECS.Systems.Baking {
    [WorldSystemFilter(WorldSystemFilterFlags.BakingSystem)]
    public partial struct SetPhysicsVelocityBakingSystem : ISystem {
        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (var (vel, setVel) in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRO<SetPhysicsVelocity>>()) {
                vel.ValueRW.Linear = setVel.ValueRO.Velocity;
            }
        }
        
    }
}