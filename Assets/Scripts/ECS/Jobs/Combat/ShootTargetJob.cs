using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;

namespace ECS.Jobs.Combat {
    [BurstCompile]
    public partial struct ShootTargetJob : IJobEntity {
        public float DeltaTime;
        public ComponentLookup<DamageHealth> DamageHealthLookup;
        public void Execute(ref Shoot shoot, in Target target) {
            shoot.CurrentCd -= DeltaTime;
            if (shoot.CurrentCd > 0) return;
            shoot.CurrentCd = shoot.ShootCd;
            if (!DamageHealthLookup.HasComponent(target.Entity)) return;
            if (DamageHealthLookup.IsComponentEnabled(target.Entity)) {
                DamageHealthLookup.GetRefRW(target.Entity).ValueRW.Damage += shoot.Damage;
                return;
            }
            DamageHealthLookup.GetRefRW(target.Entity).ValueRW.Damage = shoot.Damage;
            DamageHealthLookup.SetComponentEnabled(target.Entity, true);
        }
    }
}