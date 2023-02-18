using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using PickupItem = Item;

namespace SaveSystem {
    public class SaveObject : MonoBehaviour
    {
        public SaveTypes type;

        public Dictionary<SaveTypes, BaseSaveType> Save() {
            Dictionary<SaveTypes, BaseSaveType> saveMap = new Dictionary<SaveTypes, BaseSaveType>();
            foreach (SaveTypes flag in type.GetUniqueFlags())
            {
                BaseSaveType obj = null;
                switch (flag) {
                    case SaveTypes.Position: 
                        obj = new Position() {position = transform.position};
                        break;
                    case SaveTypes.Rotation: 
                        obj = new Rotation() {rotation = transform.rotation};
                        break;
                    case SaveTypes.RigidBody: 
                        var rigidBody = GetComponent<UnityEngine.Rigidbody>();
                        if (rigidBody != null)
                            obj = new RigidBody() {velocity = rigidBody.velocity, angularVelocity = rigidBody.angularVelocity};
                        break;
                    case SaveTypes.Item: 
                        var item = GetComponent<PickupItem>();
                        if (item != null)
                            obj = new SaveSystem.Item() { pickedUp = item.pickedUp};
                        break;
                    case SaveTypes.Door: 
                        var door = GetComponent<DoorObject>();
                        if (door != null)
                            obj = new SaveSystem.Door() { locked = door.locked};
                        break;
                }
                if (obj != null)
                    saveMap[flag] = obj;
            }
            return saveMap;
        }

        public void Load(Dictionary<SaveTypes, BaseSaveType> saveMap) {
            foreach (SaveTypes flag in type.GetUniqueFlags())
            {
                switch (flag) {
                    case SaveTypes.Position: 
                        transform.position = (saveMap[flag] as Position).position;
                        break;
                    case SaveTypes.Rotation: 
                        transform.rotation = (saveMap[flag] as Rotation).rotation;
                        break;
                    case SaveTypes.RigidBody: 
                        var savedRigidBody = (saveMap[flag] as RigidBody);
                        var rigidBody = GetComponent<UnityEngine.Rigidbody>();
                        if (rigidBody != null) {
                            rigidBody.velocity = savedRigidBody.velocity;
                            rigidBody.angularVelocity = savedRigidBody.angularVelocity;
                        }
                        break;
                    case SaveTypes.Item: 
                        var itemSave = (saveMap[flag] as SaveSystem.Item);
                        var item = GetComponent<PickupItem>();
                        if (item != null) {
                            item.SetPickedUp(itemSave.pickedUp);
                        }
                        break;
                    case SaveTypes.Door: 
                        var doorSave = (saveMap[flag] as SaveSystem.Door);
                        var door = GetComponent<DoorObject>();
                        if (door != null) {
                            door.SetLocked(doorSave.locked);
                        }
                        break;
                }
            }
        }
    }
}