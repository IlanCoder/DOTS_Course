using System;
using ECS.Aspects;
using ECS.Authoring;
using ECS.Jobs;
using Statics;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace ECS.Systems.Selection {
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
        protected override void OnUpdate() {
            OnSelectSingle onSelectSingle = SystemAPI.GetSingleton<OnSelectSingle>();
            if (onSelectSingle.Called) SelectSingleUnit();
            OnSelectAreaStart onSelectAreaStart = SystemAPI.GetSingleton<OnSelectAreaStart>();
            if(onSelectAreaStart.Called) SelectAreaStart();
            OnSelectAreaEnd onSelectAreaEnd = SystemAPI.GetSingleton<OnSelectAreaEnd>();
            if(onSelectAreaEnd.Called) SelectMultipleUnits(onSelectAreaEnd.Cancelled);
        }
        
        [BurstCompile]
        void SelectSingleUnit() {
            JobHandle callDeselectJob = new CallDeselectAllJob().ScheduleParallel(Dependency);
            CollisionWorld collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
            ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastInput raycastInput = new RaycastInput {
                Start = ray.origin,
                End = ray.GetPoint(1000f),
                Filter = new CollisionFilter() {
                    BelongsTo = ~0u,
                    CollidesWith = ObjectLayers.UnitsLayer
                }
            };
            callDeselectJob.Complete();
            if (!collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit hit)) return;
            Entity entityHit = hit.Entity;
            if (!SystemAPI.HasComponent<Selected>(entityHit)) return;
            Selected selected = EntityManager.GetComponentData<Selected>(entityHit);
            selected.OnSelected = true;
            EntityManager.SetComponentData(entityHit, selected);
        }
        
        [BurstCompile]
        void SelectMultipleUnits() {
            Dependency = new CallSelectMultipleJob {
                CamPos = _camera.transform.position,
                CamForward = _camera.transform.forward,
                CamProjMatrix = _camera.projectionMatrix,
                CamRight = _camera.transform.right,
                CamUp = _camera.transform.up,
                PixelHeight = _camera.pixelHeight,
                PixelWidth = _camera.pixelWidth,
                SelectionArea = GetSelectionRect()
            }.ScheduleParallel(Dependency);
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
        
        void SelectAreaStart() {
            _selectionStartPos = Input.mousePosition;
            OnSelectionStart?.Invoke(this, EventArgs.Empty);
        }
        
        void SelectMultipleUnits(bool canceled) {
            if (!canceled) SelectMultipleUnits();
            OnSelectionEnd?.Invoke(this, EventArgs.Empty);
        }
    }
}