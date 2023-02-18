using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem {
    [System.Flags]
    public enum SaveTypes{
        Nothing = 0,
        Position = 1,
        Rotation = 2,
        RigidBody = 4,
        Item = 8,
        Door = 16
    }
}