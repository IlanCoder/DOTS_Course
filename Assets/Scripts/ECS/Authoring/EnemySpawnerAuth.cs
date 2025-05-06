using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace ECS.Authoring {
    public class EnemySpawnerAuth : MonoBehaviour {
        [SerializeField] float spawnCooldown;
        [SerializeField] GameObject enemyPref;
        private class EnemySpawnerAuthBaker : Baker<EnemySpawnerAuth> {
            public override void Bake(EnemySpawnerAuth authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new EnemySpawnerData {
                    SpawnPos = authoring.transform.position,
                    SpawnCd = authoring.spawnCooldown,
                    EnemyPref = GetEntity(authoring.enemyPref, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
    
    public struct EnemySpawnerData : IComponentData {
        public float3 SpawnPos;
        public float SpawnCd;
        public float CurrentCd;
        public Entity EnemyPref;
    }
}