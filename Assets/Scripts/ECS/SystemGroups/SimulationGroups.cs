using Unity.Entities;
using Unity.Transforms;

namespace ECS.SystemGroups {
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial class UnitsMovementSystemGroup : ComponentSystemGroup { }
    
    [UpdateBefore(typeof(UnitsMovementSystemGroup))]
    public partial class SelectionSystemGroup : ComponentSystemGroup { }
    
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial class CombatSystemGroup : ComponentSystemGroup { }
}