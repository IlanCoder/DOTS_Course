using ECS.Authoring;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace ECS.Jobs.Combat {
    [BurstCompile]
    [UpdateInGroup(typeof(CreateEntitiesSystemGroup))]
    public partial struct ShootTargetJob : IJobEntity {
        [ReadOnly] public float DeltaTime;
        public EntityCommandBuffer Ecb;
 
        public void Execute(ref Shoot shoot, in Target target) {
            shoot.CurrentCd -= DeltaTime;
            if (shoot.CurrentCd > 0) return;
            shoot.CurrentCd = shoot.ShootCd;
            Entity bullet = Ecb.Instantiate(shoot.BulletEntity);
            //TODO bullet spawn location on Unit
            //TODO rotate bullet towards target
        }
    }
}