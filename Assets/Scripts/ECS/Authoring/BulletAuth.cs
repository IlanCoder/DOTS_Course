using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Authoring {
    public class BulletAuth : MonoBehaviour {
        [SerializeField] float speed;
        private class BulletAuthBaker : Baker<BulletAuth> {
            public override void Bake(BulletAuth authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new SetPhysicsVelocity {
                    Velocity = new float3(0, 0, authoring.speed)
                });
                AddComponent<BulletDamageInfo>(entity);
            }
        }
    }

    public struct SetPhysicsVelocity : IComponentData {
        public float3 Velocity;
    }

    public struct BulletDamageInfo : IComponentData {
        public int Damage;
    }
}