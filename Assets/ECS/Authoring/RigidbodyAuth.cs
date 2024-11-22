using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Authoring {
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyAuth : MonoBehaviour {
        private class RigidbodyAuthBaker : Baker<RigidbodyAuth> {
            RigidbodyConstraints _rbConstraints;
            public override void Bake(RigidbodyAuth authoring) {
                _rbConstraints = GetComponent<Rigidbody>().constraints;
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new RigidbodyLocks {
                    
                    RotationLocks = new bool3((_rbConstraints & RigidbodyConstraints.FreezeRotationX) != RigidbodyConstraints.None, 
                        (_rbConstraints & RigidbodyConstraints.FreezeRotationY) != RigidbodyConstraints.None, 
                        (_rbConstraints & RigidbodyConstraints.FreezeRotationZ) != RigidbodyConstraints.None)
                });
            }
        }
    }

    public struct RigidbodyLocks : IComponentData, IEnableableComponent {
        public bool3 RotationLocks;
    }
}