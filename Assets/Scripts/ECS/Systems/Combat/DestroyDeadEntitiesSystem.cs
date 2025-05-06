using ECS.Authoring;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;

namespace ECS.Systems.Combat {
    [UpdateInGroup(typeof(DestroyEntitiesSystemGroup))]
    public partial struct DestroyDeadEntitiesSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Health>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            EntityCommandBuffer commandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            foreach (var(hp, entity) in SystemAPI.Query<RefRO<Health>>().WithEntityAccess()) {
                if(hp.ValueRO.CurrentHp > 0) continue;
                commandBuffer.DestroyEntity(entity);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}