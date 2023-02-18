using System;
using UnityEngine;

namespace SaveSystem {
    [Serializable]
    public class Door: BaseSaveType {
        public override string type { get => "Door"; }
        
        public bool locked;
    }
}