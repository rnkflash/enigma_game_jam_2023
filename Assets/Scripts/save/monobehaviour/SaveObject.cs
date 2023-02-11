using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

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
                }
            }
        }
    }
}