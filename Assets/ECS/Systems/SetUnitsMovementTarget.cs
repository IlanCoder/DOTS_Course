using ECS.Aspects;
using ECS.Authoring;
using ECS.Jobs;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using UnityEngine;
using Ray = UnityEngine.Ray;
using RaycastHit = Unity.Physics.RaycastHit;

namespace ECS.Systems {
    public partial class SetUnitsMovementTarget : SystemBase {
        Camera _camera;
        Ray ray;
        
        protected override void OnCreate() {
            _camera = Camera.main;
        }
        
        protected override void OnUpdate() {
            if (!Input.GetMouseButtonDown(0)) return;
            ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastInput raycastInput = new RaycastInput {
                Start = ray.origin,
                End = ray.GetPoint(100),
                Filter = new CollisionFilter {
                    BelongsTo = ~0u,
                    CollidesWith = (uint)LayerMask.GetMask("Ground")
                }
            };
            NativeArray<RaycastHit> hits = new NativeArray<RaycastHit>(1, Allocator.TempJob);
            JobHandle jobHandle = new RaycastJob {
                CollisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld,
                RayInput = raycastInput,
                Results = hits
            }.Schedule();
            jobHandle.Complete();
            foreach (var(movableUnitTag, entity) in SystemAPI.Query<RefRO<MovableUnit>>().WithEntityAccess()) {
                TargetPosition targetPosition = SystemAPI.GetComponent<TargetPosition>(entity);
                targetPosition.Target = hits[0].Position;
                SystemAPI.SetComponent(entity, targetPosition);
                if (!SystemAPI.IsComponentEnabled<TargetPosition>(entity))
                                    SystemAPI.SetComponentEnabled<TargetPosition>(entity, true);
            }
            hits.Dispose();
        }
    }
}