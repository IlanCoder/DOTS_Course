using ECS.Authoring;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.Systems.Combat {
    [UpdateInGroup(typeof(CombatSystemGroup))]
    [UpdateBefore(typeof(ShootSystem))]
    public partial struct StopToShootSystem : ISystem {
        ComponentLookup<LocalTransform> _transformLookup;
        EntityQuery _possibleShootersQuery;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Shoot>();
            _transformLookup = state.GetComponentLookup<LocalTransform>();
            _possibleShootersQuery = state.GetEntityQuery(new EntityQueryBuilder(Allocator.Temp)
                .WithAll<LocalTransform,Target,TargetPosition>()
                .WithPresent<Shoot>()
            );
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            _transformLookup.Update(ref state);
            state.Dependency = new StopToShootJob {
                LocalTransformLookup = _transformLookup
            }.Schedule(_possibleShootersQuery, state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
    
    [BurstCompile]
    public partial struct StopToShootJob : IJobEntity {
        public ComponentLookup<LocalTransform> LocalTransformLookup;

        public void Execute(Entity entity, EnabledRefRW<Shoot> shootEnabled, in Target target, ref TargetPosition targetPosition,
        ref Shoot shoot) {
            float3 targetPos = LocalTransformLookup.GetRefRO(target.Entity).ValueRO.Position;
            float3 shooterPos = LocalTransformLookup.GetRefRO(entity).ValueRO.Position;
            float distanceSq = math.distancesq(shooterPos, targetPos);
            if (distanceSq > math.pow(shoot.ShootRange, 2)) {
                if (shootEnabled.ValueRO) shootEnabled.ValueRW = false;
                return;
            }
            targetPosition.Target = shooterPos;
            if (!shootEnabled.ValueRO) shootEnabled.ValueRW = true;
        }
    }
}