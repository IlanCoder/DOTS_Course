using Unity.Entities;
using Unity.Physics;
using UnityEngine;

namespace ECS.Authoring {
    public class MoveSpeedAuth : MonoBehaviour {
        [SerializeField] float moveSpeed;
        [SerializeField] float rotateSpeed;
        public class Baker: Baker<MoveSpeedAuth> {
            override public void Bake(MoveSpeedAuth auth) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new MoveSpeed {
                    TranslateSpeed = auth.moveSpeed,
                    RotateSpeed = auth.rotateSpeed
                });
            }
        }
    }
    public struct MoveSpeed : IComponentData {
        public float TranslateSpeed;
        public float RotateSpeed;
    }
    
}
