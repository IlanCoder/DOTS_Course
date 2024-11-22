using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

namespace ECS.Systems {
    public partial struct PhysicsMassSetupSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (var(rbLocks, physicsMass, entity) in SystemAPI
                     .Query<RefRO<RigidbodyLocks>, RefRW<PhysicsMass>>().WithEntityAccess()) {
                if (rbLocks.ValueRO.RotationLocks.x) physicsMass.ValueRW.InverseInertia.x = 0;
                if (rbLocks.ValueRO.RotationLocks.y) physicsMass.ValueRW.InverseInertia.y = 0;
                if (rbLocks.ValueRO.RotationLocks.z) physicsMass.ValueRW.InverseInertia.z = 0;
                SystemAPI.SetComponentEnabled<RigidbodyLocks>(entity, false);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}