using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInit : MonoBehaviour
{
    
    void Start()
    {
        var toExit = Player.Instance.toExit;
        if (toExit != null) {
            var exit = GameObject.FindGameObjectsWithTag("Exit")
                .Select(gameObject => gameObject.GetComponent<ExitTrigger>())
                .First(exitTrigger => exitTrigger.exitName == toExit);
            var cc = GetComponent<CharacterController>();
            cc.enabled = false;
            transform.position = exit.spawnPoint.position;
            transform.rotation = exit.spawnPoint.rotation;
            cc.enabled = true;
        }
    }
}
