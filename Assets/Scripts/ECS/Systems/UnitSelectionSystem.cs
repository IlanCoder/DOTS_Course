using System;
using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using Unit = ECS.Tags.Unit;

namespace ECS.Systems {
    [BurstCompile]
    public partial class UnitSelectionSystem : SystemBase {
        public static UnitSelectionSystem Instance { get; private set;}

        Camera _camera;
        UnityEngine.Ray ray;
        Vector2 _selectionStartPos;
        
    #region Events
        public event EventHandler OnSelectionStart;
        public event EventHandler OnSelectionEnd;
    #endregion
        
        [BurstCompile]
        protected override void OnCreate() {
            Instance ??= this;
            EntityQuery entityQuery = EntityManager.CreateEntityQuery(typeof(Selected), typeof(Unit));
            RequireForUpdate(entityQuery);
            _camera = Camera.main;
        }

        [BurstCompile]
        protected override void OnStartRunning() {

        }

        [BurstCompile]
        protected override void OnUpdate() { }
        
        [BurstCompile]
        protected override void OnStopRunning() {
            
        }
        
        [BurstCompile]
        void SelectNewUnits() {
            Vector2 unitScreenPos;
            Rect selectionAreaRect = GetSelectionRect();
            
            foreach (var (localTransform, entity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Unit>()
                     .WithPresent<Selected>().WithEntityAccess()) {
                unitScreenPos = _camera.WorldToScreenPoint(localTransform.ValueRO.Position);
                if (!selectionAreaRect.Contains(unitScreenPos)) {
                    if(SystemAPI.IsComponentEnabled<Selected>(entity)) SystemAPI.SetComponentEnabled<Selected>(entity, false);
                    continue;
                };
                SystemAPI.SetComponentEnabled<Selected>(entity, true);
            }
        }

        [BurstCompile]
        Rect GetSelectionRect() {
            Vector2 mousePos = Input.mousePosition;
            Vector2 lowerLeftCorner = new Vector2 {
                x = math.min(mousePos.x, _selectionStartPos.x),
                y = math.min(mousePos.y, _selectionStartPos.y)
            };
            Vector2 topRightCorner = new Vector2 {
                x = math.max(mousePos.x, _selectionStartPos.x),
                y = math.max(mousePos.y, _selectionStartPos.y)
            };
            return new Rect {
                position = lowerLeftCorner,
                width = topRightCorner.x - lowerLeftCorner.x,
                height = topRightCorner.y - lowerLeftCorner.y,
            };
        }

        [BurstCompile]
        void Handle_SelectSingleUnit(object sender, EventArgs args) {
            CollisionWorld collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
            ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastInput raycastInput = new RaycastInput {
                Start = ray.origin,
                End = ray.GetPoint(1000f),
                Filter = new CollisionFilter() {
                    BelongsTo = ~0u,
                    CollidesWith = (uint)LayerMask.GetMask("Unit")
                }
            };
            if (!collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit hit)) return;
            Entity entityHit = hit.Entity;
            if (!SystemAPI.HasComponent<Selected>(entityHit)) return;
            SystemAPI.SetComponentEnabled<Selected>(entityHit, !SystemAPI.IsComponentEnabled<Selected>(entityHit));
        }

        void Hande_SelectAreaStart(object sender, EventArgs e) {
            _selectionStartPos = Input.mousePosition;
            OnSelectionStart?.Invoke(this, EventArgs.Empty);
        }
        
        /*void Handle_SelectMultipleUnits(object sender, SelectAreaArgs e) {
            if (!e.Canceled) SelectNewUnits();
            OnSelectionEnd?.Invoke(this, EventArgs.Empty);
        }*/
    }
}