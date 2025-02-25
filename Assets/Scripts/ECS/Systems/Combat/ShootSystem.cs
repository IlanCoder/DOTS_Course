using ECS.Authoring;
using ECS.Jobs.Combat;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace ECS.Systems.Combat {
    [UpdateInGroup(typeof(CombatSystemGroup))]
    public partial struct ShootSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Shoot>();
            state.RequireForUpdate<Target>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            state.Dependency= new ShootTargetJob {
                DeltaTime = SystemAPI.Time.DeltaTime,
                Ecb = ecb
            }.Schedule(state.Dependency);
        }
    }
}