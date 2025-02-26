using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Authoring {
    public class BulletAuth : MonoBehaviour {
        [SerializeField] float speed;
        private class BulletAuthBaker : Baker<BulletAuth> {
            public override void Bake(BulletAuth authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new PhysicsVelocityInfo {
                    Speed = authoring.speed
                });
                AddComponent<BulletDamageInfo>(entity);
                AddComponent<PhysicsVelocityDirectionInfo>(entity);
            }
        }
    }

    public struct PhysicsVelocityInfo : IComponentData {
        public float Speed;
    }

    public struct PhysicsVelocityDirectionInfo : IComponentData {
        public float3 TargetDirection;
    }

    public struct BulletDamageInfo : IComponentData {
        public int Damage;
    }
}