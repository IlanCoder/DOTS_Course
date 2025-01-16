using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

namespace ECS.Systems {
    public partial struct PhysicsMassSetupSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<RigidbodyLocks>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (var(rbLocks, physicsMass, entity) in SystemAPI
                     .Query<RefRO<RigidbodyLocks>, RefRW<PhysicsMass>>().WithEntityAccess()) {
                SetRotationLocks(rbLocks.ValueRO, ref physicsMass.ValueRW);
                SystemAPI.SetComponentEnabled<RigidbodyLocks>(entity, false);
            }
        }
        
        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
        
        [BurstCompile]
        void SetRotationLocks(in RigidbodyLocks rbLocks, ref PhysicsMass physicsMass) {
            if (rbLocks.RotationLocks.x) physicsMass.InverseInertia.x = 0;
            if (rbLocks.RotationLocks.y) physicsMass.InverseInertia.y = 0;
            if (rbLocks.RotationLocks.z) physicsMass.InverseInertia.z = 0;
        }
    }
}