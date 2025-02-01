using ECS.Aspects;
using Unity.Burst;
using Unity.Entities;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace ECS.Systems {
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public partial class ReadInputSystem : SystemBase {
        InputSystem_Actions _inputs;
        
        [BurstCompile]
        protected override void OnCreate() {
            CreateInputEventsEntity();
        }

        protected override void OnStartRunning() {
            Entity entity = SystemAPI.GetSingletonEntity<InputEventsAspect>();
            _inputs ??= new InputSystem_Actions();
            _inputs.UI.Click.started += i => {
                InputEventsAspect inputEventsAspect = SystemAPI.GetAspect<InputEventsAspect>(entity);
                if (i.interaction is SlowTapInteraction) inputEventsAspect.OnSelectAreaStartCalled = true;
            };
            _inputs.UI.Click.performed += i => {
                InputEventsAspect inputEventsAspect = SystemAPI.GetAspect<InputEventsAspect>(entity);
                if (i.interaction is SlowTapInteraction) {
                    inputEventsAspect.OnSelectAreaEndCalled(false);
                }
                else inputEventsAspect.OnSelectSingleCalled = true;
            };
            _inputs.UI.Click.canceled += i => {
                InputEventsAspect inputEventsAspect = SystemAPI.GetAspect<InputEventsAspect>(entity);
                inputEventsAspect.OnSelectAreaEndCalled(true);
            };
            _inputs.UI.RightClick.performed += i=> {
                if (i.interaction is PressInteraction) {
                    InputEventsAspect inputEventsAspect = SystemAPI.GetAspect<InputEventsAspect>(entity);
                    inputEventsAspect.OnSelectPositionCalled = true;
                }
            };
            _inputs.UI.Enable();
        }

        [BurstCompile]
        protected override void OnUpdate() {}

        [BurstCompile]
        protected override void OnDestroy() {}
        
        [BurstCompile]
        Entity CreateInputEventsEntity() {
            Entity inputEventsEntity = EntityManager.CreateEntity(
                typeof(OnSelectAreaStart),
                typeof(OnSelectPosition),
                typeof(OnSelectAreaEnd),
                typeof(OnSelectSingle)
            );
            
            return inputEventsEntity;
        }
    }
}