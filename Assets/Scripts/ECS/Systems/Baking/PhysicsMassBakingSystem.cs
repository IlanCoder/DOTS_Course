using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

namespace ECS.Systems.Baking {
    [WorldSystemFilter(WorldSystemFilterFlags.BakingSystem)]
    public partial struct PhysicsMassBakingSystem : ISystem {
        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (var(rbLocks, physicsMass) in SystemAPI
                     .Query<RefRO<RigidbodyLocks>, RefRW<PhysicsMass>>()) {
                SetRotationLocks(rbLocks.ValueRO, ref physicsMass.ValueRW);
            }
        }
        
        [BurstCompile]
        void SetRotationLocks(in RigidbodyLocks rbLocks, ref PhysicsMass physicsMass) {
            if (rbLocks.RotationLocks.x) physicsMass.InverseInertia.x = 0;
            if (rbLocks.RotationLocks.y) physicsMass.InverseInertia.y = 0;
            if (rbLocks.RotationLocks.z) physicsMass.InverseInertia.z = 0;
        }
    }
}