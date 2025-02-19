using ECS.Authoring;
using ECS.Jobs.Combat;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace ECS.Systems.Combat {
    [UpdateInGroup(typeof(CombatSystemGroup))]
    public partial struct ShootSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Shoot>();
            state.RequireForUpdate<Target>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            state.Dependency= new ShootTargetJob {
                DeltaTime = SystemAPI.Time.DeltaTime,
                DamageHealthLookup = SystemAPI.GetComponentLookup<DamageHealth>()
            }.Schedule(state.Dependency);
        }
    }
}