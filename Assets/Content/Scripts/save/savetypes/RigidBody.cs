using System;
using UnityEngine;

namespace SaveSystem {
    [Serializable]
    public class RigidBody: BaseSaveType {
        public override string type { get => "RigidBody"; }
        
        public Vector3 velocity;
        public Vector3 angularVelocity;
    }
}