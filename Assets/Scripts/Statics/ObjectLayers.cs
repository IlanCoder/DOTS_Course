using UnityEngine;

namespace Statics {
    public static class ObjectLayers {
        public readonly static uint FriendlyLayer = (uint)LayerMask.GetMask("Friendly");
        public readonly static uint ZombieLayer = (uint)LayerMask.GetMask("Zombie");
        public readonly static uint GroundLayer = (uint)LayerMask.GetMask("Ground");
    }
}