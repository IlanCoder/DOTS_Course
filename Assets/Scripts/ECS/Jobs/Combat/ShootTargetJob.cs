using ECS.Authoring;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace ECS.Jobs.Combat {
    [BurstCompile]
    [UpdateInGroup(typeof(CreateEntitiesSystemGroup))]
    public partial struct ShootTargetJob : IJobEntity {
        [ReadOnly] public float DeltaTime;
        public EntityCommandBuffer Ecb;
        public ComponentLookup<LocalTransform> TransformLookup;
 
        public void Execute(Entity entity, ref Shoot shoot, in Target target) {
            //Look At Target
            
            shoot.CurrentCd -= DeltaTime;
            if (shoot.CurrentCd > 0) return;
            shoot.CurrentCd = shoot.ShootCd;
            
            /*BUG: BulletEntity Reference gets lost when selecting a new entity in the entity hierarchy.
             If clicking another entity after loosing the reference with this patch of != Entity.Null,
             Soldier shoots 1 bullet each interaction with the hierarchy and entity, 
             yet the zombie looses a lot of health */
            if (shoot.BulletEntity == Entity.Null) return; 
            
            LocalTransform shooterTransform = TransformLookup.GetRefRO(entity).ValueRO;
            float3 spawnPos = shooterTransform.TransformPoint(shoot.SpawnPos);
            
            Entity bullet = Ecb.Instantiate(shoot.BulletEntity);
            Ecb.SetComponent(bullet, LocalTransform.FromPosition(spawnPos));
            
            float3 targetPos = TransformLookup.GetRefRO(target.Entity).ValueRO.Position;
            targetPos.y += shoot.SpawnPos.y;
            Ecb.SetComponent(bullet, new PhysicsVelocityDirectionInfo {
                TargetDirection = math.normalize(targetPos - spawnPos)
            });
            
            Ecb.SetComponent(bullet, new BulletDamageInfo {
                Damage = shoot.Damage
            });
        }
    }
}