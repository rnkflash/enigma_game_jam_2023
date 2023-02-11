using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem {
    [System.Flags]
    public enum SaveTypes{
        Nothing = 0,
        Position = 1,
        Rotation = 2,
        RigidBodyForces = 4,
        Health = 8,
        SomethingElse = 16
    }
}