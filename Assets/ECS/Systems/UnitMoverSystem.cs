using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ECS.Systems {
    public partial struct UnitMoverSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach ((RefRW<LocalTransform> transform, RefRO<MoveSpeed> speed, RefRW<PhysicsVelocity> physicsVelocity)
                     unit in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveSpeed>, RefRW<PhysicsVelocity>>()) {
                
                float3 moveDirection = new float3(5, 0, 5) - unit.transform.ValueRO.Position;
                
                unit.transform.ValueRW.Rotation = math.slerp(unit.transform.ValueRO.Rotation,
                    quaternion.LookRotation(moveDirection, Vector3.up), SystemAPI.Time.DeltaTime);

                unit.physicsVelocity.ValueRW.Linear = unit.transform.ValueRO.Forward() * unit.speed.ValueRO.Val;
                unit.physicsVelocity.ValueRW.Angular = float3.zero;
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}