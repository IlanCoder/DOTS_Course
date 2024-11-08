using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ECS.Systems {
    public partial struct UnitMoverSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach ((RefRO<LocalTransform> transform, RefRO<MoveSpeed> speed, RefRW<PhysicsVelocity> physicsVelocity)
                     unit in SystemAPI.Query<RefRO<LocalTransform>, RefRO<MoveSpeed>, RefRW<PhysicsVelocity>>()) {
                unit.physicsVelocity.ValueRW.Linear = unit.transform.ValueRO.Forward() * unit.speed.ValueRO.Val;
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}