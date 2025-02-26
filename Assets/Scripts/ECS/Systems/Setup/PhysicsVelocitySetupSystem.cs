using ECS.Authoring;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

namespace ECS.Systems.Setup {
    [UpdateInGroup(typeof(SetupComponentsSystemGroup))]
    public partial struct PhysicsVelocitySetupSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<PhysicsVelocityInfo>();
            state.RequireForUpdate<PhysicsVelocityDirectionInfo>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);
            foreach (var (speed, direction, physicsVelocity, entity) in SystemAPI
                     .Query<RefRO<PhysicsVelocityInfo>, RefRO<PhysicsVelocityDirectionInfo>, RefRW<PhysicsVelocity>>()
                     .WithEntityAccess()) {
                physicsVelocity.ValueRW.Linear = direction.ValueRO.TargetDirection * speed.ValueRO.Speed;
                ecb.RemoveComponent<PhysicsVelocityInfo>(entity);
                ecb.RemoveComponent<PhysicsVelocityDirectionInfo>(entity);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}