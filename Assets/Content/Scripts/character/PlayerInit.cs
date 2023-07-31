using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInit : MonoBehaviour
{
    void Start()
    {
        GetComponent<PlayerController>().SetupCostume(Player.Instance.cosmonaft, Player.Instance.HasItemOfType(ItemId.Pistol));
        var toExit = Player.Instance.toExit;
        if (toExit == null) return;
        var exit = GameObject.FindGameObjectsWithTag("Exit")
            .Select(gameObject => gameObject.GetComponent<ExitTrigger>())
            .Where(exitTrigger => exitTrigger!=null)
            .FirstOrDefault(exitTrigger => exitTrigger.exitName == toExit);
        if (exit == null) return;
        
        var cc = GetComponent<CharacterController>();
        cc.enabled = false;
        transform.position = exit.exitPoint.position;
        transform.rotation = exit.exitPoint.rotation;
        cc.enabled = true;

        var door = exit.GetComponentInParent<DoorObject>();
        if (door != null) {
            door.EnterSequence(GetComponent<PlayerController>());
        } else
            exit.EnterSequence(GetComponent<PlayerController>());
    }
}
