using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring {
    public class FindTargetAuth : MonoBehaviour {
        [SerializeField] float searchRadius;
        private class FindTargetAuthBaker : Baker<FindTargetAuth> {
            public override void Bake(FindTargetAuth authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new FindTarget {
                    SearchRange = authoring.searchRadius
                });
                AddComponent(entity, new Target());
                SetComponentEnabled<Target>(entity, false);
            }
        }
    }

    public struct FindTarget : IComponentData {
        public float SearchRange;
    }

    public struct Target : IComponentData, IEnableableComponent {
        public Entity Entity;
    }
}