using UnityEngine;

namespace SaveSystem {
    public class Rotation: BaseSaveType {
        public override string type { get => "Rotation"; }
        
        public Quaternion rotation;
    }
}