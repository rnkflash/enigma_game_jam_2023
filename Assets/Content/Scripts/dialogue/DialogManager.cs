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
        commentDialog.gameObject.SetActive(false);
        confirmDialog.gameObject.SetActive(false);
        loreDialog.gameObject.SetActive(false);
        npcDialog.gameObject.SetActive(false);

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

        if (inputValues.use || inputValues.fire) {
            if (!useButtonPressed) {
                useButtonPressed = true;
                OnAnyDialogNext();
            }
        } else {
            useButtonPressed = false;
        }
    }

    private void OnConfirmDialogResult(ConfirmDialogResult message)
    {
        dialogActive = false;
        confirmDialog.gameObject.SetActive(false);
    }

    private void OnConfirmDialogStart(ConfirmDialogStart message)
    {
        dialogActive = true;
        confirmDialog.gameObject.SetActive(true);
        confirmDialog.StartDialog(message.initiator, message.question, message.yes, message.no);
    }

    private void OnCommentDialogStart(CommentDialogStart message)
    {
        dialogActive = true;
        commentDialog.gameObject.SetActive(true);
        commentDialog.StartDialog(message.payload);
    }

    private void OnCommentDialogEnd(CommentDialogEnd message)
    {
        dialogActive = false;
        commentDialog.gameObject.SetActive(false);
    }

    private void OnLoreDialogStart(LoreDialogStart message)
    {
        dialogActive = true;
        loreDialog.gameObject.SetActive(true);
        loreDialog.StartDialog(message.payload);
    }

    private void OnLoreDialogEnd(LoreDialogEnd message)
    {
        dialogActive = false;
        loreDialog.gameObject.SetActive(false);
    }    

    private void OnNpcDialogStart(NpcDialogStart message)
    {
        dialogActive = true;
        npcDialog.gameObject.SetActive(true);
        npcDialog.StartDialog(message.npc, message.inky);
    }

    private void OnNpcDialogEnd(NpcDialogEnd message)
    {
        dialogActive = false;
        npcDialog.gameObject.SetActive(false);
    }  

    private void OnAnyDialogNext()
    {
        if (commentDialog.gameObject.activeSelf) {
            commentDialog.SkipSentence();
        }

        if (loreDialog.gameObject.activeSelf) {
            loreDialog.SkipSentence();
        }

        if (npcDialog.gameObject.activeSelf) {
            npcDialog.SkipSentence();
        }
    }
}
