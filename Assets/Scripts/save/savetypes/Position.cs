using System;
using UnityEngine;

namespace SaveSystem {
    [Serializable]
    public class Position: BaseSaveType {
        public override string type { get => "Position"; }
        
        public Vector3 position;
    }
}