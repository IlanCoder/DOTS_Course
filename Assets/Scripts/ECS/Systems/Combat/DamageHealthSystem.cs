using ECS.Authoring;
using ECS.Jobs.Combat;
using ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

namespace ECS.Systems.Combat {
    [UpdateInGroup(typeof(CombatSystemGroup))]
    public partial struct DamageHealthSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Health>();
            state.RequireForUpdate<DamageHealth>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            JobHandle hpHandle = new DealHealthDamageJob().ScheduleParallel(state.Dependency);
            state.Dependency = new DisableDamageComponentJob().ScheduleParallel(hpHandle);
        }
    }
}