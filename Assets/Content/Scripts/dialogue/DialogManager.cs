using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    private CommentDialogUIController commentDialog;
    private ConfirmDialogUIController confirmDialog;
    private LoreDialogUIController loreDialog;
    
    void Start()
    {
        commentDialog = GetComponentInChildren<CommentDialogUIController>();
        confirmDialog = GetComponentInChildren<ConfirmDialogUIController>();
        loreDialog = GetComponentInChildren<LoreDialogUIController>();

        commentDialog.gameObject.SetActive(false);
        confirmDialog.gameObject.SetActive(false);
        loreDialog.gameObject.SetActive(false);

        EventBus<ConfirmDialogStart>.Sub(OnConfirmDialogStart);
        EventBus<ConfirmDialogResult>.Sub(OnConfirmDialogResult);

        EventBus<CommentDialogStart>.Sub(OnCommentDialogStart);
        EventBus<CommentDialogEnd>.Sub(OnCommentDialogEnd);

        EventBus<LoreDialogStart>.Sub(OnLoreDialogStart);
        EventBus<LoreDialogEnd>.Sub(OnLoreDialogEnd);

        EventBus<AnyDialogNext>.Sub(OnAnyDialogNext);
    }

    void OnDestroy() {
        EventBus<ConfirmDialogStart>.Unsub(OnConfirmDialogStart);
        EventBus<ConfirmDialogResult>.Unsub(OnConfirmDialogResult);

        EventBus<CommentDialogStart>.Unsub(OnCommentDialogStart);
        EventBus<CommentDialogEnd>.Unsub(OnCommentDialogEnd);

        EventBus<LoreDialogStart>.Unsub(OnLoreDialogStart);
        EventBus<LoreDialogEnd>.Unsub(OnLoreDialogEnd);

        EventBus<AnyDialogNext>.Unsub(OnAnyDialogNext);
    }

    private void OnConfirmDialogResult(ConfirmDialogResult message)
    {
        confirmDialog.gameObject.SetActive(false);
    }

    private void OnConfirmDialogStart(ConfirmDialogStart message)
    {
        confirmDialog.gameObject.SetActive(true);
        confirmDialog.StartDialog(message.initiator, message.question, message.yes, message.no);
    }

    private void OnCommentDialogStart(CommentDialogStart message)
    {
        commentDialog.gameObject.SetActive(true);
        commentDialog.StartDialog(message.payload);
    }

    private void OnCommentDialogEnd(CommentDialogEnd message)
    {
        commentDialog.gameObject.SetActive(false);
    }

    private void OnLoreDialogStart(LoreDialogStart message)
    {
        loreDialog.gameObject.SetActive(true);
        loreDialog.StartDialog(message.payload);
    }

    private void OnLoreDialogEnd(LoreDialogEnd message)
    {
        loreDialog.gameObject.SetActive(false);
    }    

    private void OnAnyDialogNext(AnyDialogNext message)
    {
        if (commentDialog.gameObject.activeSelf) {
            commentDialog.SkipSentence();
        }

        if (loreDialog.gameObject.activeSelf) {
            loreDialog.SkipSentence();
        }
    }
}
