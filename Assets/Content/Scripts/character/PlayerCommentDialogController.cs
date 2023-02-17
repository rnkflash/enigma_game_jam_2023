using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCommentDialogController : MonoBehaviour
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
        EventBus<CommentDialogStart>.Sub(DialogStart);
        EventBus<CommentDialogEnd>.Sub(DialogEnd);
    }

    void OnDestroy()
    {
        EventBus<CommentDialogStart>.Unsub(DialogStart);
        EventBus<CommentDialogEnd>.Unsub(DialogEnd);
    }

    private void DialogStart(CommentDialogStart message)
    {
        playerInteractions.enabled = false;
        player.disableControls = true;
        dialogOnGoing = true;
        player.GetComponent<Animator>().SetFloat("Speed", 0f);
        useButton = _input.use;
    }

    private void DialogEnd(CommentDialogEnd message)
    {
        playerInteractions.enabled = true;
        player.disableControls = false;
        dialogOnGoing = false;
    }

    private void DialogNext() {
        EventBus<AnyDialogNext>.Pub(new AnyDialogNext());
    }

    void Update() {
        if (!dialogOnGoing) 
            return;
        
        if (_input.use || _input.fire) {
            if (!useButton) {
                useButton = true;
                DialogNext();
            }
        } else {
            useButton = false;
        }
    }
}
