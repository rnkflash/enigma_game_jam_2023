using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Ink.Runtime;

public class NpcDialogUIController : MonoBehaviour, BlinkingTextButton.IBlinkingButtonParent
{
    public TMP_Text nameText;
    public TMP_Text dialogText;
    public BlinkingTextButton[] choiceButtons;
    private Story story;
    private NpcObject currentNpc;
    private bool dialogIsPlaying = false;

    private bool typingSentence = false;
    private string currentSentence;
    private float timePerCharacter = 0.01f;
    private int currentIndex = 0;
    private int currentChoiceNumber = 0;

    void Awake()
    {
        dialogIsPlaying = false;
        foreach (var button in choiceButtons)
        {
            button.SetParent(this);
            button.SetText("");
            button.gameObject.SetActive(false);
        }
    }

    void OnDestroy() {
        foreach (var button in choiceButtons)
        {
            button.SetParent(null);
        }
    }

    public void SetActive(bool active) {
        if (active) {
            EventBus<DialogButtonPressed>.Sub(OnButtonPressed);
            currentIndex = 0;
        }
        else {
            EventBus<DialogButtonPressed>.Unsub(OnButtonPressed);
            HideAllChoices();
        }
        
        gameObject.SetActive(active);
    }

    private void OnButtonPressed(DialogButtonPressed message)
    {
            if (message.button == DialogButtonPressed.Type.Submit || typingSentence && message.button == DialogButtonPressed.Type.LeftClick) {
                SkipSentence();
            } else 
            if (currentChoiceNumber > 0) {
                if (message.button == DialogButtonPressed.Type.Down) {
                    SelectChoice(currentIndex + 1);
                }
                else
                if (message.button == DialogButtonPressed.Type.Up) {
                    SelectChoice(currentIndex - 1);
                }
            }
    }

    private void SelectChoice(int newIndex) {
        choiceButtons[currentIndex].StopBlinking();
        currentIndex = newIndex;
        if (currentIndex < 0)
            currentIndex = currentChoiceNumber - 1;
        if (currentIndex >= currentChoiceNumber)
            currentIndex = 0;
        choiceButtons[currentIndex].StartBlinking();
    }

    public void StartDialog(NpcObject npc, TextAsset inky)
    {
        nameText.text = npc.name;
        dialogIsPlaying = true;
        story = new Story(inky.text);
        currentNpc = npc;
        typingSentence = false;
        currentSentence = "";
        ContinueStory();
    }

    private void EndDialogue() {
        var tempNpc = currentNpc;
        dialogIsPlaying = false;
        currentNpc = null;
        dialogText.text = "";
        foreach (var button in choiceButtons)
        {
            button.SetText("");
        }
        EventBus<NpcDialogEnd>.Pub(new NpcDialogEnd() {npc = tempNpc});
    }

    private void ContinueStory() {
        if (story.canContinue) {
            currentSentence = story.Continue();
            StopAllCoroutines();
		    StartCoroutine(TypeSentence());
        } else {
            EndDialogue();
        }
    }

    public void SkipSentence() {
        if (typingSentence)
        {
            StopAllCoroutines();
            dialogText.text = currentSentence;
            typingSentence = false;
            ShowChoices();
        } else {
            if (currentChoiceNumber > 0)
            {
                story.ChooseChoiceIndex(currentIndex);
                HideAllChoices();
            }
            ContinueStory();
        }
    }

    private IEnumerator TypeSentence ()
	{
        typingSentence = true;
		dialogText.text = "";
		foreach (char letter in currentSentence.ToCharArray())
		{
            if (!typingSentence)
                break;
			dialogText.text += letter;
			yield return new WaitForSeconds(timePerCharacter);
		}
        typingSentence = false;

        ShowChoices();
	}

    private void ShowChoices() {
        currentIndex = 0;
        var choices = story.currentChoices;
        currentChoiceNumber = choices.Count;
        if (choices.Count > choiceButtons.Length) {
            Debug.LogError($"too many choices: {choices.Count}. Maximum allowed: {choiceButtons.Length}.");
        }
        var index = 0;
        foreach (var choice in choices)
        {
            choiceButtons[index].gameObject.SetActive(true);
            choiceButtons[index].StopBlinking();
            choiceButtons[index].SetText(choice.text);
            index++;
        }

        choiceButtons[currentIndex].StartBlinking();

        for (int i = index; i < choiceButtons.Length; i++)
        {
            choiceButtons[index].StopBlinking();
            choiceButtons[index].SetText("");
            choiceButtons[index].gameObject.SetActive(false);
        }
    }

    private void HideAllChoices() {
        currentChoiceNumber = 0;
        foreach (var button in choiceButtons)
        {
            if (button.gameObject.activeSelf) {
                button.StopBlinking();
                button.SetText("");
                button.gameObject.SetActive(false);
            }
        }
    }

    public void MouseSelected(BlinkingTextButton button)
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (choiceButtons[i] == button) {
                SelectChoice(i);
                break;
            }
        }
    }
}
