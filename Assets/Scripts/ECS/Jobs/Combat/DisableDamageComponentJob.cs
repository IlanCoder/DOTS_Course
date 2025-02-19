using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;

namespace ECS.Jobs.Combat {
    [BurstCompile]
    public partial struct DisableDamageComponentJob : IJobEntity {
        public void Execute(EnabledRefRW<DamageHealth> enabled, ref DamageHealth damageHealth) {
            damageHealth.Damage = 0;
            enabled.ValueRW = false;
        }
    }
}