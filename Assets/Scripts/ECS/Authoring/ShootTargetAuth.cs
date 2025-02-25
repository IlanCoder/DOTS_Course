using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring {
    public class ShootTargetAuth : MonoBehaviour {
        [SerializeField] int damage;
        [SerializeField] float shootCd;
        [SerializeField] GameObject bulletPref;
        private class ShootTargetAuthBaker : Baker<ShootTargetAuth> {
            public override void Bake(ShootTargetAuth authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Shoot {
                    ShootCd = authoring.shootCd,
                    Damage = authoring.damage,
                    BulletEntity = GetEntity(authoring.bulletPref, TransformUsageFlags.None)
                });
            }
        }
    }

    public struct Shoot : IComponentData {
        public int Damage;
        public float ShootCd;
        public float CurrentCd;
        public Entity BulletEntity;
    }
}