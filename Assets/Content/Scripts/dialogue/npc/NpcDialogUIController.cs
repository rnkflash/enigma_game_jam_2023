using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Ink.Runtime;

public class NpcDialogUIController : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogText;
    public BlinkingTextButton[] choiceButtons;
    private Story story;
    private NpcObject currentNpc;
    private bool dialogIsPlaying = false;

    void Start()
    {
        dialogIsPlaying = false;
        foreach (var button in choiceButtons)
        {
            button.SetText("");
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
        
    }


    public void SkipSentence() {

    }

    internal void StartDialog(NpcObject npc, TextAsset inky)
    {
        dialogIsPlaying = true;
        story = new Story(inky.text);
        currentNpc = npc;

        ContinueStory();
    }

    private void EndDialogue() {
        dialogIsPlaying = false;
        currentNpc = null;
        dialogText.text = "";
        foreach (var button in choiceButtons)
        {
            button.SetText("");
        }
    }

    private void ContinueStory() {
        if (story.canContinue) {
            dialogText.text = story.Continue();
        } else {
            EndDialogue();
        }
    }
}
