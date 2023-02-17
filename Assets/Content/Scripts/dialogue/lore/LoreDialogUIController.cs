using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoreDialogUIController: MonoBehaviour {

    public TMP_Text dialogueText;
    private Queue<string> sentences = new Queue<string>();

    void Awake() {
        dialogueText.text = "";
        sentences.Clear();
    }

    public void StartDialog(LoreDialogueSO data) {
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
		dialogueText.text = sentences.Dequeue();
	}

    public void SkipSentence() {
        DisplayNextSentence();
    }

	private void EndDialogue()
	{
        sentences.Clear();
        dialogueText.text = "";
        EventBus<LoreDialogEnd>.Pub(new LoreDialogEnd());
	}
}