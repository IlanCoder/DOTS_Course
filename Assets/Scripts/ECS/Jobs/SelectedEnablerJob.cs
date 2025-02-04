using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;

namespace ECS.Jobs {
    [WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
    [BurstCompile]
    public partial struct SelectedEnablerJob : IJobEntity {
        public void Execute(ref Selected selected, EnabledRefRW<Selected> enabled) {
            if (selected.OnSelected) enabled.ValueRW = true;
            else if (selected.OnDeselected) enabled.ValueRW = false;
        }
    }
}