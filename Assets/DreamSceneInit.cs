using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamSceneInit : MonoBehaviour
{
    
    void Awake()
    {
        Player.Instance.dreamScenePassed = false;
        Player.Instance.cosmonaft = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
