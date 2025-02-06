using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Authoring {
    public class MovableUnitAuth : MonoBehaviour {
        [SerializeField] float stoppingDistance;
        
        class MovableUnitAuthBaker : Baker<MovableUnitAuth> {
            public override void Bake(MovableUnitAuth authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new TargetPosition {
                    StoppingDistance = authoring.stoppingDistance,
                    Target = authoring.transform.position
                });
            }
        }
    }

    public struct TargetPosition : IComponentData, IEnableableComponent {
        public float StoppingDistance;
        public float3 Target;
    }

}