using ECS.Authoring;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

namespace ECS.Jobs.Triggers {
    public struct BulletHpDmgJob : ITriggerEventsJob {
        [ReadOnly] public ComponentLookup<BulletDamageInfo> BulletLookUp;
        public ComponentLookup<DamageHealth> HpDmgLookUp;
        public EntityCommandBuffer Ecb;
        
        public void Execute(TriggerEvent triggerEvent) {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;
            
            if (!BulletLookUp.HasComponent(entityA) && !BulletLookUp.HasComponent(entityB)) return;
            if (!HpDmgLookUp.HasComponent(entityA) && !HpDmgLookUp.HasComponent(entityB)) return;

            if (BulletLookUp.HasComponent(entityA)) {
                DamageHealthWithBullet(ref entityA, ref entityB);
            } else {
                DamageHealthWithBullet(ref entityB, ref entityA);
            }
        }

        void DamageHealthWithBullet(ref Entity bulletEntity, ref Entity damagedEntity) {
            if (HpDmgLookUp.IsComponentEnabled(damagedEntity)) {
                HpDmgLookUp.GetRefRW(damagedEntity).ValueRW.Damage += BulletLookUp.GetRefRO(bulletEntity).ValueRO.Damage;
                return;
            }
            HpDmgLookUp.GetRefRW(damagedEntity).ValueRW.Damage = BulletLookUp.GetRefRO(bulletEntity).ValueRO.Damage;
            HpDmgLookUp.SetComponentEnabled(damagedEntity, true);
            Ecb.DestroyEntity(bulletEntity);
        }
    }
}