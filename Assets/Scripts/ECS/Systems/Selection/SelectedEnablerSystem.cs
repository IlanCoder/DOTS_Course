using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;

namespace ECS.Systems.Selection {
    [UpdateAfter(typeof(UnitSelectionSystem))]
    public partial struct SelectedEnablerSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Selected>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (var (selected, entity) in SystemAPI.Query<RefRO<Selected>>().WithPresent<Selected>()
                     .WithEntityAccess()) {
                if(selected.ValueRO.OnSelected) SystemAPI.SetComponentEnabled<Selected>(entity, true);
                else if(selected.ValueRO.OnDeselected) SystemAPI.SetComponentEnabled<Selected>(entity, false);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
}