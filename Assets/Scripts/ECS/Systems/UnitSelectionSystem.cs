using System;
using ECS.Authoring;
using ECS.Tags;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Systems {
    [BurstCompile]
    public partial class UnitSelectionSystem : SystemBase {
        public static UnitSelectionSystem Instance { get; private set;}

        Camera _camera;
        
        Vector2 _selectionStartPos;
        
    #region Events
        public event EventHandler OnSelectionStart;
        public event EventHandler OnSelectionEnd;
    #endregion
        
        [BurstCompile]
        protected override void OnCreate() {
            Instance ??= this;
            _camera = Camera.main;
        }

        [BurstCompile]
        protected override void OnUpdate() {
            if (Input.GetMouseButtonDown(0)) {
                _selectionStartPos = Input.mousePosition;
                OnSelectionStart?.Invoke(this, EventArgs.Empty);
            }
            else if (Input.GetMouseButtonUp(0)) {
                SelectNewUnits();
                OnSelectionEnd?.Invoke(this, EventArgs.Empty);
            }
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
        public Rect GetSelectionRect() {
            Vector2 mousePos = Input.mousePosition;
            Vector2 lowerLeftCorner = new Vector2 {
                x = MathF.Min(mousePos.x, _selectionStartPos.x),
                y = MathF.Min(mousePos.y, _selectionStartPos.y)
            };
            Vector2 topRightCorner = new Vector2 {
                x = MathF.Max(mousePos.x, _selectionStartPos.x),
                y = MathF.Max(mousePos.y, _selectionStartPos.y)
            };
            return new Rect {
                position = lowerLeftCorner,
                width = topRightCorner.x - lowerLeftCorner.x,
                height = topRightCorner.y - lowerLeftCorner.y,
            };
        }
    }
}