using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring {
    public class ShootTargetAuth : MonoBehaviour {
        [SerializeField] float shootCd;
        private class ShootTargetAuthBaker : Baker<ShootTargetAuth> {
            public override void Bake(ShootTargetAuth authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Shoot {
                    ShootCd = authoring.shootCd
                });
            }
        }
    }

    public struct Shoot : IComponentData {
        public float ShootCd;
        public float CurrentCd;
    }
}