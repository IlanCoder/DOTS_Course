using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Authoring {
    public class ShootTargetAuth : MonoBehaviour {
        [SerializeField] int damage;
        [SerializeField] float shootCd;
        [SerializeField] float shootRange;
        [SerializeField] GameObject bulletPref;
        [SerializeField] Transform bulletSpawnPos;
        
        private class ShootTargetAuthBaker : Baker<ShootTargetAuth> {
            public override void Bake(ShootTargetAuth authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Shoot {
                    ShootCd = authoring.shootCd,
                    Damage = authoring.damage,
                    BulletEntity = GetEntity(authoring.bulletPref, TransformUsageFlags.Dynamic),
                    SpawnPos = authoring.bulletSpawnPos.localPosition,
                    ShootRange = authoring.shootRange
                });
                SetComponentEnabled<Shoot>(entity, false);
            }
        }
    }

    public struct Shoot : IComponentData, IEnableableComponent {
        //TODO: Look At Shoot Target
        public int Damage;
        public float ShootCd;
        public float CurrentCd;
        public float ShootRange;
        public float3 SpawnPos;
        public Entity BulletEntity;
    }
}