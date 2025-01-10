using System;
using ECS.Authoring;
using ECS.Jobs;
using ECS.Tags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using UnityEngine;
using Ray = UnityEngine.Ray;
using RaycastHit = Unity.Physics.RaycastHit;

namespace ECS.Systems {
    [BurstCompile]
    public partial class SetUnitsMovementTargetSystem : SystemBase {
        Camera _camera;
        Ray ray;
        
        [BurstCompile]
        protected override void OnCreate() {
            _camera = Camera.main;
        }
        
        [BurstCompile]
        protected override void OnUpdate() {
            if (!Input.GetMouseButtonDown(1)) return;
            RaycastInput raycastInput = CreateRaycastInput();
            NativeArray<RaycastHit> hits = new NativeArray<RaycastHit>(1, Allocator.TempJob);
            JobHandle jobHandle = new RaycastJob {
                CollisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld,
                RayInput = raycastInput,
                Results = hits
            }.Schedule();
            jobHandle.Complete();
            
            EnableUnitsMovement(ref hits);
            hits.Dispose();
        }

        [BurstCompile]
        RaycastInput CreateRaycastInput() {
            ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastInput raycastInput = new RaycastInput {
                Start = ray.origin,
                End = ray.GetPoint(100),
                Filter = new CollisionFilter {
                    BelongsTo = ~0u,
                    CollidesWith = (uint)LayerMask.GetMask("Ground")
                }
            };
            return raycastInput;
        }

        [BurstCompile]
        void EnableUnitsMovement(ref NativeArray<RaycastHit> hits) {
            EntityQueryDesc queryDesc = new EntityQueryDesc {
                All= new ComponentType[] {typeof(Unit), typeof(Selected)}
            };
            EntityQuery query = GetEntityQuery(queryDesc);
            foreach (Entity entity in query.ToEntityArray(Allocator.Temp)) {
                RefRW<TargetPosition> targetPosition = SystemAPI.GetComponentRW<TargetPosition>(entity);
                targetPosition.ValueRW.Target = hits[0].Position;
                if (!SystemAPI.IsComponentEnabled<TargetPosition>(entity))
                    SystemAPI.SetComponentEnabled<TargetPosition>(entity, true);
            }
        }
    }
}