using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDialogController : MonoBehaviour
{
    private PlayerInputValues _input;
    private PlayerController player;
    private PlayerInteraction playerInteractions;
    private bool useButton = false;

    private bool dialogOnGoing = false;

    void Start()
    {
        _input = GetComponent<PlayerInputValues>();
        player = GetComponent<PlayerController>();
        playerInteractions = GetComponentInChildren<PlayerInteraction>();
        EventBus<LoreDialogStart>.Sub(DialogStart);
        EventBus<LoreDialogEnd>.Sub(DialogEnd);
    }

    void OnDestroy()
    {
        EventBus<LoreDialogStart>.Unsub(DialogStart);
        EventBus<LoreDialogEnd>.Unsub(DialogEnd);
    }

    private void DialogStart(LoreDialogStart message)
    {
        playerInteractions.enabled = false;
        player.disableControls = true;
        dialogOnGoing = true;
        player.GetComponent<Animator>().SetFloat("Speed", 0f);
        useButton = _input.use;
    }

    private void DialogEnd(LoreDialogEnd message)
    {
        playerInteractions.enabled = true;
        player.disableControls = false;
        dialogOnGoing = false;
    }

    private void DialogNext() {
        EventBus<LoreDialogNext>.Pub(new LoreDialogNext());
    }

    void Update() {
        if (!dialogOnGoing) 
            return;
        
        if (_input.use) {
            if (!useButton) {
                useButton = true;
                DialogNext();
            }
        } else {
            useButton = false;
        }
    }
}
