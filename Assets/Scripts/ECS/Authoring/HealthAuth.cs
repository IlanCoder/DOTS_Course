using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring {
    public class HealthAuth : MonoBehaviour {
        [SerializeField] int maxHp;
        private class HealthAuthBaker : Baker<HealthAuth> {
            public override void Bake(HealthAuth authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Health {
                    CurrentHp = authoring.maxHp
                });
                AddComponent<DamageHealth>(entity);
                SetComponentEnabled<DamageHealth>(entity, false);
            }
        }
    }

    public struct Health : IComponentData {
        public int CurrentHp;
    }

    public struct DamageHealth : IComponentData, IEnableableComponent {
        public int Damage;
    }
}