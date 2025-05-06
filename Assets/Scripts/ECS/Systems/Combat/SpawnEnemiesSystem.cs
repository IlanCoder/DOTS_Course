using System.Collections;
using ECS.Authoring;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace ECS.Systems.Combat {
    public partial struct SpawnEnemiesSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EnemySpawnerData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            state.Dependency = new SpawnEnemiesJob {
                DeltaTime = SystemAPI.Time.DeltaTime,
                Ecb = ecb
            }.Schedule(state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }
    }
    
    [BurstCompile]
    public partial struct SpawnEnemiesJob : IJobEntity {
        [ReadOnly] public float DeltaTime;
        public EntityCommandBuffer Ecb;

        public void Execute(ref EnemySpawnerData spawner) {
            spawner.CurrentCd -=DeltaTime;
            if(spawner.CurrentCd > 0) return;
            spawner.CurrentCd = spawner.SpawnCd;

            Entity enemy = Ecb.Instantiate(spawner.EnemyPref);
            Ecb.SetComponent(enemy, LocalTransform.FromPosition(spawner.SpawnPos));
        }
    }
}