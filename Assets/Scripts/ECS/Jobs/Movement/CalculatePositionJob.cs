using ECS.Authoring;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.Jobs.Movement {
    [BurstCompile]
    public struct CalculatePositionsJob : IJobParallelFor {
        public NativeArray<float3> Positions;
        public float3 InitPosition;
        public int UnitsPerRow;
        public float ColumnOffset;
        public float RowOffset;
        
        public void Execute(int index) {
            int row = index/UnitsPerRow;
            int column = index%UnitsPerRow;
            Positions[index] = InitPosition + new float3(column * ColumnOffset, 0, -row * RowOffset);
        }
    }

    [BurstCompile]
    public partial struct SetChaseTargetPosJob : IJobEntity {
        public ComponentLookup<LocalTransform> TransformLookup;

        public void Execute(ref TargetPosition targetPosition, in Target target) {
            targetPosition.Target = TransformLookup.GetRefRO(target.Entity).ValueRO.Position;
        }
    }
}