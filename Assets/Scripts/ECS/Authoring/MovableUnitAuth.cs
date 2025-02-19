using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Authoring {
    public class MovableUnitAuth : MonoBehaviour {
        [Header("Speeds")]
        [SerializeField] float moveSpeed;
        [SerializeField] float rotateSpeed;
        
        [Header("Settings")]
        [SerializeField] float stoppingDistance;
        
        class MovableUnitAuthBaker : Baker<MovableUnitAuth> {
            public override void Bake(MovableUnitAuth authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new TargetPosition {
                    StoppingDistance = authoring.stoppingDistance,
                    Target = authoring.transform.position
                });
                AddComponent(entity, new MoveSpeed {
                    TranslateSpeed = authoring.moveSpeed,
                    RotateSpeed = authoring.rotateSpeed
                });
            }
        }
    }

    public struct TargetPosition : IComponentData, IEnableableComponent {
        public float StoppingDistance;
        public float3 Target;
    }

    public struct MoveSpeed : IComponentData {
        public float TranslateSpeed;
        public float RotateSpeed;
    }
}