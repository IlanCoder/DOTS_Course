using ECS.Authoring;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace ECS.Systems.Combat {
    [UpdateInGroup(typeof(CombatSystemGroup))]
    public partial struct DamageHealthSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Health>();
            state.RequireForUpdate<DamageHealth>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (var (health, damage, entity) in SystemAPI.Query<RefRW<Health>, RefRW<DamageHealth>>()
                     .WithEntityAccess()) {
                health.ValueRW.CurrentHp -= damage.ValueRO.Damage;
                damage.ValueRW.Damage = 0;
                SystemAPI.SetComponentEnabled<DamageHealth>(entity, false);
            }
        }
    }
}