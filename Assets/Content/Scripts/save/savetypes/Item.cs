using System;
using UnityEngine;

namespace SaveSystem {
    [Serializable]
    public class Item: BaseSaveType {
        public override string type { get => "Item"; }
        
        public bool pickedUp;
    }
}