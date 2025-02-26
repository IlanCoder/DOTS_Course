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
 
        public void Execute(Entity shooter, ref Shoot shoot, in Target target) {
            shoot.CurrentCd -= DeltaTime;
            if (shoot.CurrentCd > 0) return;
            shoot.CurrentCd = shoot.ShootCd;
            
            float3 shooterPos = TransformLookup.GetRefRO(shooter).ValueRO.Position;
            
            Entity bullet = Ecb.Instantiate(shoot.BulletEntity);
            Ecb.SetComponent(bullet, LocalTransform.FromPosition(shooterPos));

            float3 targetPos = TransformLookup.GetRefRO(target.Entity).ValueRO.Position;
            Ecb.SetComponent(bullet, new PhysicsVelocityDirectionInfo {
                TargetDirection = math.normalize(targetPos - shooterPos)
            });
        }
    }
}