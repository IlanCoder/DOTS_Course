using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace ECS.Aspects {
    
    [BurstCompile]
    public readonly partial struct MovableUnitAspect : IAspect {
        readonly RefRW<LocalTransform> _localTransform; 
        readonly RefRO<MoveSpeed> _moveSpeed;
        readonly RefRO<TargetPosition> _targetPosition;
        readonly RefRW<PhysicsVelocity> _physicsVelocity;

        [BurstCompile]
        public bool ArrivedToTarget() {
            return math.distance(_localTransform.ValueRO.Position, _targetPosition.ValueRO.Target) <=
                   _targetPosition.ValueRO.StoppingDistance;
        }

        [BurstCompile]
        public void StopUnit() {
            _physicsVelocity.ValueRW.Linear = float3.zero;
        }
        
        [BurstCompile]
        public void MoveUnit(float deltaTime) {
            float3 moveDirection = _targetPosition.ValueRO.Target - _localTransform.ValueRO.Position;
                
            _localTransform.ValueRW.Rotation = math.slerp(_localTransform.ValueRO.Rotation,
            quaternion.LookRotation(moveDirection, new float3(0, 1, 0)), deltaTime);

            _physicsVelocity.ValueRW.Linear = _localTransform.ValueRO.Forward() * _moveSpeed.ValueRO.Val;
            _physicsVelocity.ValueRW.Angular = float3.zero;
        }
    }
}