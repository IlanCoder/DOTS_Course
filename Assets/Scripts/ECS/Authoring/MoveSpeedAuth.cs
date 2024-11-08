using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring {
    public class MoveSpeedAuth : MonoBehaviour {
        [SerializeField] float val;
        public class Baker: Baker<MoveSpeedAuth> {
            override public void Bake(MoveSpeedAuth auth) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new MoveSpeed {
                    Val = auth.val
                });
            }
        }
    }
    public struct MoveSpeed : IComponentData {
        public float Val;
    }
    
}
