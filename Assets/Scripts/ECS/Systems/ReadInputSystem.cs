using System;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace ECS.Systems {
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public partial class ReadInputSystem : SystemBase {
        InputSystem_Actions _inputs;
        
        #region Events
        public event EventHandler OnSelectSingle;
        public event EventHandler OnSelectAreaStart;
        public event EventHandler<SelectAreaArgs> OnSelectAreaEnd;
        public event EventHandler OnSelectPosition;
        #endregion
        
        [BurstCompile]
        protected override void OnCreate() {
            _inputs ??= new InputSystem_Actions();
            _inputs.UI.Click.started += i => {
                if (i.interaction is SlowTapInteraction) OnSelectAreaStart?.Invoke(this, EventArgs.Empty);
            };
            _inputs.UI.Click.performed += i => {
                if (i.interaction is SlowTapInteraction)
                    OnSelectAreaEnd?.Invoke(this, new SelectAreaArgs { Canceled = false });
                else OnSelectSingle?.Invoke(this,EventArgs.Empty);
            };
            _inputs.UI.Click.canceled += i => {
                OnSelectAreaEnd?.Invoke(this, new SelectAreaArgs { Canceled = true});
            };
            _inputs.UI.RightClick.performed += i=> {
                if(i.interaction is PressInteraction) OnSelectPosition?.Invoke(this, EventArgs.Empty);
            };
            _inputs.UI.Enable();
        }

        [BurstCompile]
        protected override void OnUpdate() {
        }

        [BurstCompile]
        protected override void OnDestroy() {

        }
    }

    public class SelectAreaArgs : EventArgs {
        public bool Canceled;
    }
}