using ECS.Authoring;
using ECS.Jobs.Combat;
using ECS.Tags;
using Statics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;

namespace ECS.Systems.Combat {
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct SearchTargetSystem : ISystem {
        EntityQuery _soldiers;
        EntityQuery _zombies;

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<PhysicsWorldSingleton>();
            state.RequireForUpdate<FindTarget>();
            _soldiers = state.GetEntityQuery(new EntityQueryBuilder(Allocator.Temp)
                .WithAll<Soldier, LocalTransform, FindTarget>()
                .WithPresent<Target>()
            );
            _zombies = state.GetEntityQuery(new EntityQueryBuilder(Allocator.Temp)
                .WithAll<Zombie, LocalTransform, FindTarget>()
                .WithPresent<Target>()
            );
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            CollisionWorld collisionWorld = physicsWorld.CollisionWorld;
            NativeList<DistanceHit> distanceHits = new NativeList<DistanceHit>(Allocator.TempJob);
            JobHandle searchTargetJobs = new SearchTargetSphereCastJob {
                CollisionWorld = collisionWorld,
                CollisionFilter = new CollisionFilter {
                    BelongsTo = ObjectLayers.FriendlyLayer,
                    CollidesWith = ObjectLayers.ZombieLayer
                },
                DistanceHits = distanceHits
            }.Schedule(_soldiers, state.Dependency);
            searchTargetJobs = new SearchTargetSphereCastJob {
                CollisionWorld = collisionWorld,
                CollisionFilter = new CollisionFilter {
                    BelongsTo = ObjectLayers.ZombieLayer,
                    CollidesWith = ObjectLayers.FriendlyLayer
                },
                DistanceHits = distanceHits
            }.Schedule(_zombies, searchTargetJobs);
            searchTargetJobs.Complete();
            distanceHits.Dispose();
        }
    }
}