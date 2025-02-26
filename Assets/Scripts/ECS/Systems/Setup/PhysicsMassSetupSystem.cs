using ECS.Authoring;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

namespace ECS.Systems.Setup {
    [UpdateInGroup(typeof(SetupComponentsSystemGroup))]
    public partial struct PhysicsMassSetupSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<RigidbodyLocks>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            foreach (var(rbLocks, physicsMass, entity) in SystemAPI
                     .Query<RefRO<RigidbodyLocks>, RefRW<PhysicsMass>>().WithEntityAccess()) {
                SetRotationLocks(rbLocks.ValueRO, ref physicsMass.ValueRW);
                ecb.RemoveComponent<RigidbodyLocks>(entity);
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