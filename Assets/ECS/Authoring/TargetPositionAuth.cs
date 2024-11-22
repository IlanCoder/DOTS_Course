using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Authoring {
    public class TargetPositionAuth : MonoBehaviour {
        [SerializeField] float stoppingDistance;
        private class TargetPositionAuthBaker : Baker<TargetPositionAuth> {
            public override void Bake(TargetPositionAuth authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new TargetPosition {
                    StoppingDistance = authoring.stoppingDistance,
                    Target = authoring.transform.position
                });
                AddComponent(entity, new MovableUnit());
            }
        }
    }

    public struct TargetPosition : IComponentData, IEnableableComponent {
        public float StoppingDistance;
        public float3 Target;
    }
    
    public struct MovableUnit : IComponentData {}
}