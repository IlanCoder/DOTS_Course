using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace ECS.Authoring {
    public class SelectedAuth : MonoBehaviour {
        public GameObject selectedVisual;
        private class SelectedAuthBaker : Baker<SelectedAuth> {
            public override void Bake(SelectedAuth authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Selected {
                    SelectedVisual = GetEntity(authoring.selectedVisual, TransformUsageFlags.None)
                });
                SetComponentEnabled<Selected>(entity, false);
            }
        }
    }

    public struct Selected : IComponentData, IEnableableComponent {
        public Entity SelectedVisual;
    }
}