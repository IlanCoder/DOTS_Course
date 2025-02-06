using System;
using ECS.Tags;
using Enums;
using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring {
    public class UnitAuth : MonoBehaviour {
        [SerializeField] Faction faction;
        private class UnitAuthBaker : Baker<UnitAuth> {
            public override void Bake(UnitAuth authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent<Unit>(entity);
                switch (authoring.faction) {
                    case Faction.Soldier: AddComponent<Soldier>(entity);
                        break;
                    case Faction.Zombie: AddComponent<Zombie>(entity);
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
    
    public struct Unit : IComponentData {}
}