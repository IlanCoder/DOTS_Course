using ECS.Authoring;
using ECS.Jobs.Combat;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace ECS.Systems.Combat {
    [UpdateInGroup(typeof(CombatSystemGroup))]
    public partial struct ShootSystem : ISystem {
        ComponentLookup<DamageHealth> _damageHealthLookUp;
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Shoot>();
            state.RequireForUpdate<Target>();
            _damageHealthLookUp = state.GetComponentLookup<DamageHealth>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            _damageHealthLookUp.Update(ref state);
            state.Dependency= new ShootTargetJob {
                DeltaTime = SystemAPI.Time.DeltaTime,
                DamageHealthLookup = _damageHealthLookUp,
            }.Schedule(state.Dependency);
        }
    }
}