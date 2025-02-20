using ECS.Authoring;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace ECS.Systems {
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    public partial struct BulletCollisionSystem : ISystem {
        ComponentLookup<Unit> _unitLookUp;
        ComponentLookup<BulletDamageInfo> _bulletLookUp;
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            _bulletLookUp = state.GetComponentLookup<BulletDamageInfo>();
            _unitLookUp = state.GetComponentLookup<Unit>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            _bulletLookUp.Update(ref state);
            _unitLookUp.Update(ref state);
            EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            
            state.Dependency = new BulletCollisionJob {
                BulletLookUp = _bulletLookUp,
                UnitLookUp = _unitLookUp,
                Ecb = ecb
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }

    public struct BulletCollisionJob : ITriggerEventsJob {
        [ReadOnly] public ComponentLookup<BulletDamageInfo> BulletLookUp;
        [ReadOnly] public ComponentLookup<Unit> UnitLookUp;
        public EntityCommandBuffer Ecb;
        
        public void Execute(TriggerEvent triggerEvent) {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;
            
            if (!BulletLookUp.HasComponent(entityA) && !BulletLookUp.HasComponent(entityB)) return;
            if (!UnitLookUp.HasComponent(entityA) && !UnitLookUp.HasComponent(entityB)) return;
            
            if(BulletLookUp.HasComponent(entityA)) Ecb.DestroyEntity(entityA);
            else if(BulletLookUp.HasComponent(entityB)) Ecb.DestroyEntity(entityB);
            
        }
    }
}