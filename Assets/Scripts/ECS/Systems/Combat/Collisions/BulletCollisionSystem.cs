using ECS.Authoring;
using ECS.Jobs.Triggers;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace ECS.Systems.Combat.Collisions {
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    public partial struct BulletCollisionSystem : ISystem {
        ComponentLookup<DamageHealth> _hpDmgLookUp;
        ComponentLookup<BulletDamageInfo> _bulletLookUp;
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            _bulletLookUp = state.GetComponentLookup<BulletDamageInfo>();
            _hpDmgLookUp = state.GetComponentLookup<DamageHealth>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            _bulletLookUp.Update(ref state);
            _hpDmgLookUp.Update(ref state);
            EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            
            state.Dependency = new BulletHpDmgJob {
                BulletLookUp = _bulletLookUp,
                HpDmgLookUp = _hpDmgLookUp,
                Ecb = ecb
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}