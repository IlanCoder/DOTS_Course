using ECS.Authoring;
using ECS.Jobs;
using ECS.Tags;
using Statics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Systems {
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct SearchTargetSystem : ISystem {
        EntityQuery _soldiers;
        EntityQuery _zombies;

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<PhysicsWorldSingleton>();
            state.RequireForUpdate<FindTarget>();
            SetEntityQueries(ref state);
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

        [BurstCompile]
        void SetEntityQueries(ref SystemState state) {
            EntityQueryDesc desc = new EntityQueryDesc {
                All = new[] {
                    ComponentType.ReadOnly<Soldier>(),
                    ComponentType.ReadOnly<LocalTransform>(),
                    ComponentType.ReadOnly<FindTarget>()
                },
                Present = new[] { ComponentType.ReadWrite<Target>() }
            };
            _soldiers = state.GetEntityQuery(desc);
            desc.All[0] = ComponentType.ReadOnly<Zombie>();
            _zombies = state.GetEntityQuery(desc);
        }
    }
}