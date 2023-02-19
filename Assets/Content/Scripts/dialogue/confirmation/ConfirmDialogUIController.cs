using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConfirmDialogUIController : MonoBehaviour, BlinkingTextButton.IBlinkingButtonParent
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

        foreach (var button in choices)
        {
            button.SetParent(this);
        }
    }

    void OnDestroy() {
        yesButton.onClickListeners -= YesButtonPressed;
        noButton.onClickListeners -= NoButtonPressed;

        foreach (var button in choices)
        {
            button.SetParent(null);
        }
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
            SelectButton(index + 1);
        }
        else
        if (message.button == DialogButtonPressed.Type.Right) {
            SelectButton(index - 1);
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

    private void SelectButton(int newIndex) {
        choices[index].StopBlinking();
        index = newIndex;
        if (index < 0)
            index = choices.Length - 1;
        if (index >= choices.Length)
            index = 0;
        choices[index].StartBlinking();
    }

    public void MouseSelected(BlinkingTextButton button)
    {
        for (int i = 0; i < choices.Length; i++)
        {
            if (choices[i] == button) {
                SelectButton(i);
                break;
            }
        }
    }
}
