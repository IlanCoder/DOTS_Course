using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;

namespace ECS.Jobs.Selection {
    [WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
    [BurstCompile]
    public partial struct ResetSelectedJob : IJobEntity {
        public void Execute(ref Selected selected) {
            selected.OnSelected = false;
            selected.OnDeselected = false;
        }
    }
}