using ECS.Authoring;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;

namespace ECS.Systems.Combat {
    [UpdateInGroup(typeof(DisableEntitiesSystemGroup))]
    public partial struct DisableDeadEntities : ISystem {
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
                commandBuffer.AddComponent<Disabled>(entity);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}