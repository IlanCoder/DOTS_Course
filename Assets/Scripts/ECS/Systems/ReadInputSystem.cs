using System;
using Unity.Burst;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace ECS.Systems {
    [BurstCompile]
    public partial class ReadInputSystem : SystemBase {
        InputSystem_Actions _inputs;
        public static ReadInputSystem Instance;
        
        #region Events
        public event EventHandler OnSelectSingle;
        public event EventHandler OnSelectAreaStart;
        public event EventHandler<SelectAreaArgs> OnSelectAreaEnd;
        #endregion
        
        
        
        [BurstCompile]
        protected override void OnCreate() {
            Instance ??= this;
            _inputs ??= new InputSystem_Actions();
            _inputs.UI.SelectSingle.performed += i => { OnSelectSingle?.Invoke(this,EventArgs.Empty);};
            _inputs.UI.SelectArea.started += i => {
                OnSelectAreaStart?.Invoke(this, EventArgs.Empty);
            };
            _inputs.UI.SelectArea.performed += i => {
                OnSelectAreaEnd?.Invoke(this, new SelectAreaArgs { Canceled = false});
            };
            _inputs.UI.SelectArea.canceled += i => {
                OnSelectAreaEnd?.Invoke(this, new SelectAreaArgs { Canceled = true});
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