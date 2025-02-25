using Unity.Entities;
using Unity.Transforms;

namespace ECS.SystemGroups {
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial class UnitsMovementSystemGroup : ComponentSystemGroup { }
    
    [UpdateBefore(typeof(UnitsMovementSystemGroup))]
    public partial class SelectionSystemGroup : ComponentSystemGroup { }
    
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial class CombatSystemGroup : ComponentSystemGroup { }
    
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial class DestroyEntitiesSystemGroup : ComponentSystemGroup { }
    
    [UpdateInGroup(typeof(LateSimulationSystemGroup)),
    UpdateBefore(typeof(DestroyEntitiesSystemGroup))]
    public partial class CreateEntitiesSystemGroup : ComponentSystemGroup { }
}