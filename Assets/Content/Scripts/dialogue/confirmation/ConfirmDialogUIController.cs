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

    private BlinkingTextButton[] choices;
    private int index;
    
    void Awake()
    {
        yesButton.onClickListeners += YesButtonPressed;
        noButton.onClickListeners += NoButtonPressed;

        choices = new BlinkingTextButton[2] {
            yesButton, noButton
        };

        index = 0;
    }

    void OnDestroy() {
        yesButton.onClickListeners -= YesButtonPressed;
        noButton.onClickListeners -= NoButtonPressed;

    }

    public void SetActive(bool active) {
        if (active)
            EventBus<DialogButtonPressed>.Sub(OnButtonPressed);
        else
            EventBus<DialogButtonPressed>.Unsub(OnButtonPressed);
        
        gameObject.SetActive(active);
    }

    private void OnButtonPressed(DialogButtonPressed message)
    {
        if (message.button == DialogButtonPressed.Type.Submit) {
            if (choices[index] == yesButton)
                YesButtonPressed();
            else
                NoButtonPressed();
        } else 
        if (message.button == DialogButtonPressed.Type.Left) {
            choices[index].StopBlinking();
            index--;
            if (index < 0)
                index = 1;
            choices[index].StartBlinking();
        }
        else
        if (message.button == DialogButtonPressed.Type.Right) {
            choices[index].StopBlinking();
            index++;
            if (index > 1)
                index = 0;
            choices[index].StartBlinking();
        }
    }

    public void StartDialog(IConfirmDialogInitiator initiator, string question, string yes, string no) {
        this.initiator = initiator;
        questionText.text = question;
        yesButton.SetText(yes);
        noButton.SetText(no);
        yesButton.StopBlinking();
        noButton.StopBlinking();
        index = 0;
        choices[index].StartBlinking();
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
