using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerConfirmDialogController : MonoBehaviour
{
    private PlayerInputValues _input;
    private PlayerController player;
    private PlayerInteraction playerInteractions;
    private bool useButton = false;
    private bool dialogActive = false;

    void Start()
    {
        _input = GetComponent<PlayerInputValues>();
        player = GetComponent<PlayerController>();
        playerInteractions = GetComponentInChildren<PlayerInteraction>();
        EventBus<ConfirmDialogStart>.Sub(DialogStart);
        EventBus<ConfirmDialogResult>.Sub(DialogEnd);
    }

    void OnDestroy()
    {
        EventBus<ConfirmDialogStart>.Unsub(DialogStart);
        EventBus<ConfirmDialogResult>.Unsub(DialogEnd);
    }

    private void DialogStart(ConfirmDialogStart message)
    {
        dialogActive = true;
        playerInteractions.enabled = false;
        player.disableControls = true;
        player.GetComponent<Animator>().SetFloat("Speed", 0f);
        useButton = _input.use;
    }

    private void DialogEnd(ConfirmDialogResult message)
    {
        dialogActive = false;
        playerInteractions.enabled = true;
        player.disableControls = false;
    }

    void Update() {
        if (!dialogActive)
            return;

        if (_input.use) {
            if (!useButton) {
                useButton = true;
                EventBus<ConfirmDialogPressedE>.Pub(new ConfirmDialogPressedE());
            }
        } else {
            useButton = false;
        }
    }

}
