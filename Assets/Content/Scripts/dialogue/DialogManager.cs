using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public CommentDialogUIController commentDialog;
    public ConfirmDialogUIController confirmDialog;
    public LoreDialogUIController loreDialog;
    public NpcDialogUIController npcDialog;

    private PlayerInputValues inputValues;
    private bool dialogActive = false;
    private bool useButtonPressed = true;
    
    void Start()
    {
        commentDialog.SetActive(false);
        confirmDialog.SetActive(false);
        loreDialog.SetActive(false);
        npcDialog.SetActive(false);

        inputValues = GameObject.FindObjectOfType<PlayerInputValues>();

        EventBus<ConfirmDialogStart>.Sub(OnConfirmDialogStart);
        EventBus<ConfirmDialogResult>.Sub(OnConfirmDialogResult);

        EventBus<CommentDialogStart>.Sub(OnCommentDialogStart);
        EventBus<CommentDialogEnd>.Sub(OnCommentDialogEnd);

        EventBus<LoreDialogStart>.Sub(OnLoreDialogStart);
        EventBus<LoreDialogEnd>.Sub(OnLoreDialogEnd);

        EventBus<NpcDialogStart>.Sub(OnNpcDialogStart);
        EventBus<NpcDialogEnd>.Sub(OnNpcDialogEnd);

    }

    void OnDestroy() {

        inputValues = null;

        EventBus<ConfirmDialogStart>.Unsub(OnConfirmDialogStart);
        EventBus<ConfirmDialogResult>.Unsub(OnConfirmDialogResult);

        EventBus<CommentDialogStart>.Unsub(OnCommentDialogStart);
        EventBus<CommentDialogEnd>.Unsub(OnCommentDialogEnd);

        EventBus<LoreDialogStart>.Unsub(OnLoreDialogStart);
        EventBus<LoreDialogEnd>.Unsub(OnLoreDialogEnd);

        EventBus<NpcDialogStart>.Unsub(OnNpcDialogStart);
        EventBus<NpcDialogEnd>.Unsub(OnNpcDialogEnd);

    }

    void Update() {
        if (!dialogActive)
            return;

        if (inputValues.use || inputValues.fire || inputValues.move.sqrMagnitude > 0 || inputValues.aim) {
            if (!useButtonPressed) {
                useButtonPressed = true;
                if (inputValues.use) {
                    EventBus<DialogButtonPressed>.Pub(new DialogButtonPressed() {button = DialogButtonPressed.Type.Submit});
                } else
                if (inputValues.fire) {
                    EventBus<DialogButtonPressed>.Pub(new DialogButtonPressed() {button = DialogButtonPressed.Type.LeftClick});
                } else
                if (inputValues.move.sqrMagnitude > 0) {
                    if (inputValues.move.x > 0)
                        EventBus<DialogButtonPressed>.Pub(new DialogButtonPressed() {button = DialogButtonPressed.Type.Right});
                    else
                    if (inputValues.move.x < 0)
                        EventBus<DialogButtonPressed>.Pub(new DialogButtonPressed() {button = DialogButtonPressed.Type.Left});
                    else
                    if (inputValues.move.y > 0)
                        EventBus<DialogButtonPressed>.Pub(new DialogButtonPressed() {button = DialogButtonPressed.Type.Up});
                    else
                    if (inputValues.move.y < 0)
                        EventBus<DialogButtonPressed>.Pub(new DialogButtonPressed() {button = DialogButtonPressed.Type.Down});
                    
                } else
                if (inputValues.aim) {
                    EventBus<DialogButtonPressed>.Pub(new DialogButtonPressed() {button = DialogButtonPressed.Type.RightClick});
                } 
            }
        } else {
            useButtonPressed = false;
        }
    }

    private void OnConfirmDialogResult(ConfirmDialogResult message)
    {
        dialogActive = false;
        confirmDialog.SetActive(false);
    }

    private void OnConfirmDialogStart(ConfirmDialogStart message)
    {
        dialogActive = true;
        confirmDialog.SetActive(true);
        confirmDialog.StartDialog(message.initiator, message.question, message.yes, message.no);
    }

    private void OnCommentDialogStart(CommentDialogStart message)
    {
        dialogActive = true;
        commentDialog.SetActive(true);
        commentDialog.StartDialog(message.payload);
    }

    private void OnCommentDialogEnd(CommentDialogEnd message)
    {
        dialogActive = false;
        commentDialog.SetActive(false);
    }

    private void OnLoreDialogStart(LoreDialogStart message)
    {
        dialogActive = true;
        loreDialog.SetActive(true);
        loreDialog.StartDialog(message.payload);
    }

    private void OnLoreDialogEnd(LoreDialogEnd message)
    {
        dialogActive = false;
        loreDialog.SetActive(false);
    }    

    private void OnNpcDialogStart(NpcDialogStart message)
    {
        dialogActive = true;
        npcDialog.SetActive(true);
        npcDialog.StartDialog(message.npc, message.inky);
    }

    private void OnNpcDialogEnd(NpcDialogEnd message)
    {
        dialogActive = false;
        npcDialog.SetActive(false);
    }  
}
