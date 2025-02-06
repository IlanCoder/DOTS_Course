using UnityEngine;

namespace Statics {
    public static class ObjectLayers {
        public static uint UnitsLayer = (uint)LayerMask.GetMask("Unit");
        public static uint GroundLayer = (uint)LayerMask.GetMask("Ground");
    }

    public static class LayerGroups {
        public static int SoldierGroup = -1;
        public static int ZombieGroup = -2;
    }
}