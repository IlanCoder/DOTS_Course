using ECS.Authoring;
using ECS.Jobs.Combat;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace ECS.Systems.Combat {
    [UpdateInGroup(typeof(CombatSystemGroup))]
    public partial struct ShootSystem : ISystem {
        ComponentLookup<LocalTransform> _transformLookup;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Shoot>();
            state.RequireForUpdate<Target>();
            _transformLookup = state.GetComponentLookup<LocalTransform>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            _transformLookup.Update(ref state);
            EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            state.Dependency = new ShootTargetJob {
                DeltaTime = SystemAPI.Time.DeltaTime,
                Ecb = ecb,
                TransformLookup = _transformLookup
            }.Schedule(state.Dependency);
        }
    }
}