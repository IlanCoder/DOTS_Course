using Unity.Entities;
using Unity.Physics;
using UnityEngine;

namespace ECS.Authoring {
    public class MoveSpeedAuth : MonoBehaviour {
        [SerializeField] float val;
        public class Baker: Baker<MoveSpeedAuth> {
            override public void Bake(MoveSpeedAuth auth) {
                Entity entity = GetEntity(TransformUsageFlags.None);
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
