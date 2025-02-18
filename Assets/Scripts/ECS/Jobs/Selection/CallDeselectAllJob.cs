using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;

namespace ECS.Jobs.Selection {
    [BurstCompile]
    public partial struct CallDeselectAllJob : IJobEntity {
        public void Execute(ref Selected selected) {
            selected.OnDeselected = true;
        }
    }
}