using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CommentDialogUIController: MonoBehaviour {

    public TMP_Text dialogueText;
    private Queue<string> sentences = new Queue<string>();
    public float timePerCharacter = 0.05f;
    private bool typingSentence = false;
    private string currentSentence;

    void Awake() {
        dialogueText.text = "";
        sentences.Clear();

        
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
        if (message.button == DialogButtonPressed.Type.Submit || message.button == DialogButtonPressed.Type.LeftClick)
            SkipSentence();
    }

    public void StartDialog(CommentDialogueSO data) {
        dialogueText.text = "";
        sentences.Clear();
        foreach (string item in data.sentences)
        {
            sentences.Enqueue(item);
        }
        DisplayNextSentence();
    }

    private void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		currentSentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence());
	}

    public void SkipSentence() {
        if (typingSentence)
        {
            StopAllCoroutines();
            typingSentence = false;
            dialogueText.text = currentSentence;
        } else
            DisplayNextSentence();
    }

	private IEnumerator TypeSentence ()
	{
        typingSentence = true;
		dialogueText.text = "";
		foreach (char letter in currentSentence.ToCharArray())
		{
            if (!typingSentence)
                break;
			dialogueText.text += letter;
			yield return new WaitForSeconds(timePerCharacter);
		}
        typingSentence = false;
	}

	private void EndDialogue()
	{
        StopAllCoroutines();
        sentences.Clear();
        dialogueText.text = "";
        currentSentence = "";
        typingSentence = false;
        EventBus<CommentDialogEnd>.Pub(new CommentDialogEnd());
	}
}