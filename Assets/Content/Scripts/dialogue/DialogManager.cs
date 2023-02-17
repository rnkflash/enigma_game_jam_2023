using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    private LoreDialogController loreDialog;
    private ConfirmDialogController confirmDialog;
    
    void Start()
    {
        loreDialog = GetComponentInChildren<LoreDialogController>();
        confirmDialog = GetComponentInChildren<ConfirmDialogController>();

        loreDialog.gameObject.SetActive(false);
        confirmDialog.gameObject.SetActive(false);

        EventBus<ConfirmDialogStart>.Sub(OnConfirmDialogStart);
        EventBus<ConfirmDialogResult>.Sub(OnConfirmDialogResult);

        EventBus<LoreDialogStart>.Sub(OnLoreDialogStart);
        EventBus<LoreDialogEnd>.Sub(OnLoreDialogEnd);
        EventBus<LoreDialogNext>.Sub(OnLoreDialogNext);
    }

    void OnDestroy() {
        EventBus<ConfirmDialogStart>.Unsub(OnConfirmDialogStart);
        EventBus<ConfirmDialogResult>.Unsub(OnConfirmDialogResult);

        EventBus<LoreDialogStart>.Unsub(OnLoreDialogStart);
        EventBus<LoreDialogEnd>.Unsub(OnLoreDialogEnd);
        EventBus<LoreDialogNext>.Unsub(OnLoreDialogNext);
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

    private void OnLoreDialogStart(LoreDialogStart message)
    {
        loreDialog.gameObject.SetActive(true);
        loreDialog.ShowLore(message.payload);
    }

    private void OnLoreDialogNext(LoreDialogNext message)
    {
        loreDialog.SkipSentence();
    }

    private void OnLoreDialogEnd(LoreDialogEnd message)
    {
        loreDialog.gameObject.SetActive(false);
    }

}
