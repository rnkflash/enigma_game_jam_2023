using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConfirmDialogUIController : MonoBehaviour
{
    public TMP_Text questionText;
    public BlinkingTextButton yesButton;
    public BlinkingTextButton noButton;
    private IConfirmDialogInitiator initiator;
    
    void Awake()
    {
        yesButton.onClickListeners += YesButtonPressed;
        noButton.onClickListeners += NoButtonPressed;

        EventBus<ConfirmDialogPressedE>.Sub(OnEPressed);
    }

    void OnDestroy() {
        yesButton.onClickListeners -= YesButtonPressed;
        noButton.onClickListeners -= NoButtonPressed;

        EventBus<ConfirmDialogPressedE>.Unsub(OnEPressed);
    }

    private void OnEPressed(ConfirmDialogPressedE message)
    {
        YesButtonPressed();
    }

    public void StartDialog(IConfirmDialogInitiator initiator, string question, string yes, string no) {
        this.initiator = initiator;
        questionText.text = question;
        yesButton.SetText(yes);
        noButton.SetText(no);
        yesButton.StopBlinking();
        noButton.StopBlinking();
    }

    private void YesButtonPressed()
    {
        EventBus<ConfirmDialogResult>.Pub(new ConfirmDialogResult() { result = true});
        initiator?.ConfirmDialog(true);
    }

    private void NoButtonPressed()
    {
        EventBus<ConfirmDialogResult>.Pub(new ConfirmDialogResult() { result = false});
        initiator?.ConfirmDialog(false);
    }
}
