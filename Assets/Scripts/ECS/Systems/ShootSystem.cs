using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;

namespace ECS.Systems {
    public partial struct ShootSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Shoot>();
            state.RequireForUpdate<Target>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (var (shoot, target) in SystemAPI.Query<RefRW<Shoot>, RefRO<Target>>()) {
                shoot.ValueRW.CurrentCd -= SystemAPI.Time.DeltaTime;
                if (shoot.ValueRO.CurrentCd > 0) continue;
                shoot.ValueRW.CurrentCd = shoot.ValueRO.ShootCd;
                
            }
        }
    }
}