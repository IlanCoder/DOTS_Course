using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Physics;

namespace ECS.Jobs {
    [BurstCompile]
    public struct RaycastJob : IJob {
        [ReadOnly] public CollisionWorld CollisionWorld;
        [ReadOnly] public RaycastInput RayInput;
        
        public NativeArray<RaycastHit> Results;
        
        public void Execute() {
            if (!CollisionWorld.CastRay(RayInput, out RaycastHit raycastHit)) return;
            Results[0] = raycastHit;
        }
    }
}