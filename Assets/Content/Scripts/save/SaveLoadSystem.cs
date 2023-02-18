using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem {
    using SaveMap = Dictionary<SaveTypes, BaseSaveType>;
    using SceneSaveMap = Dictionary<string, Dictionary<SaveTypes, BaseSaveType>>;

    public class SaveLoadSystem : Singleton<SaveLoadSystem>
    {
        public Dictionary<string, SceneSaveMap> savedScenes = new Dictionary<string, SceneSaveMap>();

        public void SaveScene() {
            string scene = SceneManager.GetActiveScene().name;
            SaveObject[] saveObjects = FindObjectsOfType<SaveObject>();
            foreach (SaveObject saveObject in saveObjects) 
            {
                if (!savedScenes.ContainsKey(scene))
                    savedScenes[scene] = new SceneSaveMap();
                savedScenes[scene][saveObject.gameObject.name] = saveObject.Save();
            }
        }

        public void LoadScene() {
            string scene = SceneManager.GetActiveScene().name;

            if (!savedScenes.ContainsKey(scene))
                return;

            SceneSaveMap sceneSaveMap = savedScenes[scene];
            SaveObject[] saveObjects = FindObjectsOfType<SaveObject>();
            foreach (SaveObject saveObject in saveObjects) 
            {
                string objectName = saveObject.gameObject.name;
                if (!sceneSaveMap.ContainsKey(objectName))
                    continue;
                saveObject.Load(sceneSaveMap[objectName]);
            }
        }
    }
}