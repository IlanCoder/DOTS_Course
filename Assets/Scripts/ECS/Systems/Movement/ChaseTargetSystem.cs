using ECS.Aspects;
using ECS.Authoring;
using ECS.Jobs.Movement;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace ECS.Systems.Movement {
    [UpdateInGroup(typeof(UnitsMovementSystemGroup))]
    [UpdateBefore(typeof(UnitMoverSystem))]
    public partial struct ChaseTargetSystem : ISystem {
        ComponentLookup<LocalTransform> _transformLookup;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Target>();
            _transformLookup = state.GetComponentLookup<LocalTransform>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            _transformLookup.Update(ref state);
            state.Dependency= new SetChaseTargetPosJob {
                TransformLookup = _transformLookup
            }.Schedule (state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}