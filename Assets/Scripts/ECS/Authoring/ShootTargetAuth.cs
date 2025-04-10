using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring {
    public class ShootTargetAuth : MonoBehaviour {
        [SerializeField] int damage;
        [SerializeField] float shootCd;
        [SerializeField] float shootRange;
        [SerializeField] GameObject bulletPref;
        
        private class ShootTargetAuthBaker : Baker<ShootTargetAuth> {
            public override void Bake(ShootTargetAuth authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Shoot {
                    ShootCd = authoring.shootCd,
                    Damage = authoring.damage,
                    BulletEntity = GetEntity(authoring.bulletPref, TransformUsageFlags.None),
                    ShootRange = authoring.shootRange
                });
                SetComponentEnabled<Shoot>(entity, false);
            }
        }
    }

    public struct Shoot : IComponentData, IEnableableComponent {
        public int Damage;
        public float ShootCd;
        public float CurrentCd;
        public float ShootRange;
        public Entity BulletEntity;
        //BUG: BulletEntity magically disappears after units shoot for a while
    }
}