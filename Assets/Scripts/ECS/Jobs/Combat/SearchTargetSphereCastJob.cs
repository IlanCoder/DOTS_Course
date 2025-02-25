using ECS.Authoring;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

namespace ECS.Jobs.Combat {
    [WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
    [BurstCompile]
    public partial struct SearchTargetSphereCastJob : IJobEntity {
        public CollisionWorld CollisionWorld;
        public CollisionFilter CollisionFilter;
        public NativeList<DistanceHit> DistanceHits;
        float _closestDistance;
        
        public void Execute(EnabledRefRW<Target> targetEnabled, ref Target target, in LocalTransform transform, in FindTarget findTarget) {
            _closestDistance = findTarget.SearchRange + 1;
            if (CollisionWorld.OverlapSphere(transform.Position, findTarget.SearchRange, ref DistanceHits,
                CollisionFilter)) {
                foreach (DistanceHit distanceHit in DistanceHits) {
                    if(distanceHit.Distance >= _closestDistance) continue;
                    _closestDistance = distanceHit.Distance;
                    target.Entity= distanceHit.Entity;
                }
                targetEnabled.ValueRW = true;
            } else {
                targetEnabled.ValueRW = false;
                target.Entity = Entity.Null;
            }
            DistanceHits.Clear();
        }
    }
}