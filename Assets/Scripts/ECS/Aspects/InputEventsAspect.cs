using Unity.Entities;

namespace ECS.Aspects {
    public readonly partial struct InputEventsAspect : IAspect {
        readonly RefRW<OnSelectAreaStart> _onSelectAreaStart;
        readonly RefRW<OnSelectAreaEnd> _onSelectAreaEnd;
        readonly RefRW<OnSelectSingle> _onSelectSingle;
        readonly RefRW<OnSelectPosition> _onSelectPosition;

        public bool OnSelectAreaStartCalled {
            get => _onSelectAreaStart.ValueRO.Called;
            set => _onSelectAreaStart.ValueRW.Called = value;
        }
        public void OnSelectAreaEndCalled (bool cancelled){
            _onSelectAreaEnd.ValueRW.Called = true;
            _onSelectAreaEnd.ValueRW.Cancelled = cancelled;
        }
        public bool OnSelectSingleCalled {
            get => _onSelectSingle.ValueRO.Called;
            set => _onSelectSingle.ValueRW.Called = value;
        }
        public bool OnSelectPositionCalled {
            get => _onSelectPosition.ValueRO.Called;
            set => _onSelectPosition.ValueRW.Called = value;
        }
        public void ResetInputCalls() {
            _onSelectAreaStart.ValueRW.Called = false;
            _onSelectAreaEnd.ValueRW.Called = false;
            _onSelectSingle.ValueRW.Called = false;
            _onSelectPosition.ValueRW.Called = false;
        }
    }
    
    public struct OnSelectAreaStart : IComponentData {
        public bool Called;
    }
    public struct OnSelectAreaEnd : IComponentData{
        public bool Called;
        public bool Cancelled;
    }
    public struct OnSelectSingle : IComponentData {
        public bool Called;
    }
    public struct OnSelectPosition : IComponentData {
        public bool Called;
    }
}