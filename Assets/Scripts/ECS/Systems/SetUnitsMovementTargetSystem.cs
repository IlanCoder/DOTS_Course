using System;
using ECS.Authoring;
using ECS.Jobs;
using ECS.Tags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using Ray = UnityEngine.Ray;
using RaycastHit = Unity.Physics.RaycastHit;

namespace ECS.Systems {
    [BurstCompile]
    public partial class SetUnitsMovementTargetSystem : SystemBase {
        Camera _camera;
        Ray _ray;
        readonly float _columnOffset = 2.2f;
        readonly float _rowOffset = 2.2f;
        JobHandle _positionsJobHandle;
        
        
        [BurstCompile]
        protected override void OnCreate() {
            _camera = Camera.main;
            
        }

        [BurstCompile]
        protected override void OnStartRunning() {
            World.GetExistingSystemManaged<ReadInputSystem>().OnSelectPosition += Handle_TargetPositionRayCast;
        }
        
        [BurstCompile]
        protected override void OnUpdate() { }

        [BurstCompile]
        protected override void OnStopRunning() {
            World.GetExistingSystemManaged<ReadInputSystem>().OnSelectPosition -= Handle_TargetPositionRayCast;
        }

        [BurstCompile]
        void Handle_TargetPositionRayCast(object sender, EventArgs e) {
            CollisionWorld collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
            _ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastInput raycastInput = new RaycastInput {
                Start = _ray.origin,
                End = _ray.GetPoint(100),
                Filter = new CollisionFilter {
                    BelongsTo = ~0u,
                    CollidesWith = (uint)LayerMask.GetMask("Ground")
                }
            };
            if (!collisionWorld.CastRay(raycastInput, out RaycastHit closestHit)) return;
            SetUnitsTargetPosition(closestHit.Position);
        }

        [BurstCompile]
        void SetUnitsTargetPosition(float3 targetPos) {
            EntityQueryDesc queryDesc = new EntityQueryDesc {
                All= new ComponentType[] {typeof(Unit), typeof(Selected)}
            };
            NativeArray<Entity> selectedUnits = GetEntityQuery(queryDesc).ToEntityArray(Allocator.TempJob);
            NativeArray<float3> positions =
                GenerateTargetPositions(targetPos, selectedUnits.Length);
            EnableUnitsMovement(selectedUnits, positions);
            positions.Dispose();
            selectedUnits.Dispose();
        }

        [BurstCompile]
        void EnableUnitsMovement(NativeArray<Entity> units, NativeArray<float3> positions) {
            _positionsJobHandle.Complete();
            for (int i = 0; i < units.Length; i++) {
                RefRW<TargetPosition> targetPosition = SystemAPI.GetComponentRW<TargetPosition>(units[i]);
                targetPosition.ValueRW.Target = positions[i];
                if (!SystemAPI.IsComponentEnabled<TargetPosition>(units[i]))
                    SystemAPI.SetComponentEnabled<TargetPosition>(units[i], true);
            }
        }

        [BurstCompile]
        NativeArray<float3> GenerateTargetPositions(float3 targetPosition, int posCount) {
            NativeArray<float3> positions = new NativeArray<float3>(posCount, Allocator.TempJob);
            if(posCount <= 0) return positions;
            int unitsPerRow = (int)math.ceil(math.sqrt(posCount));
            _positionsJobHandle = new CalculatePositionsJob {
                Positions = positions,
                InitPosition = targetPosition,
                UnitsPerRow = unitsPerRow,
                ColumnOffset = _columnOffset,
                RowOffset = _rowOffset
            }.Schedule(positions.Length, 64);
            return positions;
        }
    }
}