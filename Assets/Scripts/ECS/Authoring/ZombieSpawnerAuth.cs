using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace ECS.Authoring {
    public class ZombieSpawnerAuth : MonoBehaviour {
        [SerializeField] float spawnCooldown;
        private class ZombieSpawnerAuthBaker : Baker<ZombieSpawnerAuth> {
            public override void Bake(ZombieSpawnerAuth authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new ZombieSpawnerData {
                    SpawnPos = authoring.transform.position,
                    SpawnCd = authoring.spawnCooldown
                });
            }
        }

        public struct ZombieSpawnerData : IComponentData {
            public float3 SpawnPos;
            public float SpawnCd;
            public float CurrentCd;
        }
    }
}