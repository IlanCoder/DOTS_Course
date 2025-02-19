using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;

namespace ECS.Jobs.Combat {
    [BurstCompile]
    public partial struct DealHealthDamageJob : IJobEntity {
        public void Execute(in DamageHealth damage, ref Health health) {
            health.CurrentHp -= damage.Damage;
        }
    }
}